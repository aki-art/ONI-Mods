using HarmonyLib;
using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using static ProcGen.SubWorld;

namespace IndustrialArea
{
    public class IndustrialArea_ZoneTypeTracker : KMonoBehaviour, IRender200ms
    {
        [HarmonyPatch(typeof(SubworldZoneRenderData), nameof(SubworldZoneRenderData.OnActiveWorldChanged))]
        public class SubworldZoneRenderData_OnActiveWorldChanged_Patch
        {
            public static void Postfix()
            {
                IndustrialArea_ZoneTypeTracker.Instance.RegenerateBackwallTexture();
            }
        }

        [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
        public class SaveGame_OnPrefabInit_Patch
        {
            public static void Postfix(SaveGame __instance)
            {
                __instance.gameObject.AddOrGet<IndustrialArea_ZoneTypeTracker>();
            }
        }

        [HarmonyPatch(typeof(RoomProber), "CreateRoom")]
        public class RoomProber_CreateRoom_Patch
        {
            // refreshing destroys and recreates rooms, so this applies to changes
            public static void Postfix(CavityInfo cavity)
            {
                if (IndustrialArea_ZoneTypeTracker.Instance == null)
                    return;

                IndustrialArea_ZoneTypeTracker.Instance.CheckCavity(cavity);
            }
        }

        //[HarmonyPatch(typeof(RoomProber), "ClearRoom")]
        public class RoomProber_ClearRoom_Patch
        {
            public static void Prefix(Room room)
            {
                //if (!IndustrialArea_ZoneTypeTracker.Instance.IsCavityIndustrialized(room.cavity))
                //    return;

                foreach (var cell in room.cavity.cells)
                    IndustrialArea_ZoneTypeTracker.Instance.RemoveToneTypeOverride(cell);
            }
        }

        public static IndustrialArea_ZoneTypeTracker Instance;

        [Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = [];
        [Serialize] public Dictionary<int, ZoneType> pendingZoneTypeOverrides = [];
        public Dictionary<int, List<ZoneTile>> zoneTiles = [];

        private bool zoneTypesDirty;

        // Moonlet compat
        public delegate void AddBiomeOverrideDelegate(int cell, ZoneType zoneType);
        public AddBiomeOverrideDelegate addBiomeOverrideFn;


        public void MarkCell(int cell)
        {
            if (World.Instance.zoneRenderData.worldZoneTypes[cell] == ZoneType.Space)
                return;

            Instance.AddZoneTypeOverride(cell, ZoneType.IceCaves);
        }

        public static HashSet<HashedString> exceptionRoomTypes = new HashSet<HashedString>() 
        { 
            "NatureReserve" 
        };

        public bool IsCavityIndustrialized(CavityInfo info)
        {
            if (info == null) 
                return false;

            if(info.room == null)
                return false;

            if(info.cells == null || info.cells.Count < 12)
                return false;

            if(info.buildings == null || info.buildings.Count < 1 )
                return false;

            if (exceptionRoomTypes.Contains(info.room.roomType.IdHash))
                return false;

            return true;
        }

        public IEnumerator UpdateZoneTypes()
        {
            yield return SequenceUtil.waitForEndOfFrame;

            pendingZoneTypeOverrides = new Dictionary<int, ZoneType>(zoneTypeOverrides);
            zoneTypesDirty = zoneTypeOverrides.Count > 0 || pendingZoneTypeOverrides.Count > 0;
        }

        public void RemoveToneTypeOverride(int cell)
        {
            // get og
            pendingZoneTypeOverrides.Remove(cell);
            zoneTypesDirty = true;
        }

        public void AddZoneTypeOverride(int cell, ZoneType zoneType)
        {
            if (addBiomeOverrideFn != null)
            {
                addBiomeOverrideFn(cell, zoneType);
                zoneTypeOverrides[cell] = zoneType; // saving for reload if Moonlet is gone;
            }
            else
            {
                pendingZoneTypeOverrides[cell] = zoneType;
                zoneTypesDirty = true;
            }

        }

        public override void OnPrefabInit()
        {
            Instance = this;

            var moonletType = Type.GetType("Moonlet, Moonlet.ModAPI");
            if (moonletType != null)
            {
                var m_AddBiomeOverride = moonletType.GetMethod("AddBiomeOverride", [typeof(int), typeof(ZoneType)]);
                addBiomeOverrideFn = (AddBiomeOverrideDelegate)Delegate.CreateDelegate(typeof(AddBiomeOverrideDelegate), m_AddBiomeOverride);
            }
        }

        public override void OnSpawn()
        {
            if (addBiomeOverrideFn == null)
                StartCoroutine(UpdateZoneTypes());

            foreach(var room in Game.Instance.roomProber.rooms)
            {
                CheckCavity(room.cavity);
            }
        }

        public void CheckCavity(CavityInfo cavity)
        {
            if (!IsCavityIndustrialized(cavity))
                return;

            foreach (var cell in cavity.cells)
                MarkCell(cell);
        }

        // run even when paused
        public void Render200ms(float dt)
        {
            if (zoneTypesDirty)
            {
                foreach (var pending in pendingZoneTypeOverrides)
                {
                    if (!zoneTiles.ContainsKey(pending.Key))
                        SimMessages.ModifyCellWorldZone(pending.Key, pending.Value == ZoneType.Space ? byte.MaxValue : (byte)pending.Value);
                }

                RegenerateBackwallTexture(pendingZoneTypeOverrides);

                foreach (var item in pendingZoneTypeOverrides)
                    zoneTypeOverrides[item.Key] = item.Value;

                pendingZoneTypeOverrides.Clear();

                World.Instance.zoneRenderData.OnActiveWorldChanged();

                zoneTypesDirty = false;
            }
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void RegenerateBackwallTexture() => RegenerateBackwallTexture(zoneTypeOverrides);

        public void RegenerateBackwallTexture(Dictionary<int, ZoneType> overrides)
        {
            if (World.Instance.zoneRenderData == null)
            {
                Debug.Log("Subworld zone render data is not yet initialized.");
                return;
            }

            var zoneRenderData = World.Instance.zoneRenderData;

            var colorData = zoneRenderData.colourTex.GetRawTextureData();
            var indexData = zoneRenderData.indexTex.GetRawTextureData();

            foreach (var tile in overrides)
            {
                var cell = tile.Key;
                var zoneType = (byte)tile.Value;

                var color = World.Instance.zoneRenderData.zoneColours[zoneType];

                var index = (tile.Value == ZoneType.Space)
                    ? byte.MaxValue
                    : (byte)World.Instance.zoneRenderData.zoneTextureArrayIndices[zoneType];

                indexData[cell * SubworldZoneRenderData.NUM_COLOUR_BYTES + 0] = index;
                indexData[cell * SubworldZoneRenderData.NUM_COLOUR_BYTES + 1] = index; // ?
                indexData[cell * SubworldZoneRenderData.NUM_COLOUR_BYTES + 2] = (byte)zoneType;

                colorData[cell * SubworldZoneRenderData.NUM_COLOUR_BYTES + 0] = color.r;
                colorData[cell * SubworldZoneRenderData.NUM_COLOUR_BYTES + 1] = color.g;
                colorData[cell * SubworldZoneRenderData.NUM_COLOUR_BYTES + 2] = color.b;

                World.Instance.zoneRenderData.worldZoneTypes[cell] = tile.Value;
            }

            zoneRenderData.colourTex.LoadRawTextureData(colorData);
            zoneRenderData.indexTex.LoadRawTextureData(indexData);
            zoneRenderData.colourTex.Apply();
            zoneRenderData.indexTex.Apply();

            zoneRenderData.OnShadersReloaded();
        }
    }
}
