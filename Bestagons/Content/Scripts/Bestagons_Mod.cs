using Bestagons.Content.Map;
using Bestagons.Content.ModDb;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Bestagons.Content.Scripts
{
    public class Bestagons_Mod : KMonoBehaviour
    {
        public static Bestagons_Mod Instance;

        public Dictionary<HashedString, int> playerOwnedCurrenciesLookup;

        [Serialize] private List<PurchasableHex.Price> playerOwnedCurrenciesSerialized;
        [Serialize] private List<(int worldId, WorldState state)> worldStates;
        [Serialize] public bool[] hiddenCells;
        [Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = new();
        private bool hiddenCellsDirty = false;

        private bool infiniteMoney;

        public enum WorldState
        {
            UnInitialized,
            Skip,
            Generated,
            AllPurchased
        }

        public WorldState GetWorldState(int worldId)
        {
            foreach(var worldState in worldStates)
            {
                if (worldState.worldId == worldId)
                    return worldState.state;
            }

            return WorldState.UnInitialized;
        }

        public void SetWorldState(int worldId, WorldState state)
        {
            for (int i = 0; i < worldStates.Count; i++)
            {
                var worldState = worldStates[i];
                if (worldState.worldId == worldId)
                {
                    worldState.state = state;
                    return;
                }
            }

            worldStates.Add((worldId, state));
        }

        public void ToggleInfiniteMoney(bool enableCheat)
        {
            infiniteMoney = enableCheat;
            Trigger(ModHashes.onCurrencyChanged);
        }

        public int GetCurrency(HashedString currency) => playerOwnedCurrenciesLookup.TryGetValue(currency, out var value) ? value : 0;

        public bool CanPlayerAfford(List<PurchasableHex.Price> price)
        {
            if (infiniteMoney)
                return true;

            foreach (var partialPrice in price)
                if (GetCurrency(partialPrice.currencyId) - partialPrice.amount < 0)
                    return false;

            return true;
        }

        public void AddCurrency(HashedString currencyId, int amount)
        {
            playerOwnedCurrenciesLookup[currencyId] += amount;
            Trigger(ModHashes.onCurrencyChanged);
        }

        public void SpendCurrency(HashedString currencyId, int amount)
        {
            Mathf.Max(0, playerOwnedCurrenciesLookup[currencyId] -= amount);
            Trigger(ModHashes.onCurrencyChanged);
        }

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            worldStates ??= new();
            playerOwnedCurrenciesSerialized ??= new();
            hiddenCells = new bool[Grid.CellCount];

            var go = new GameObject("Bestagons_Mod");
            go.transform.parent = transform;
            DontDestroyOnLoad(go);

            SanitizeCurrencyLookup();
            hiddenCellsDirty = true;

        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        private void SanitizeCurrencyLookup()
        {
            playerOwnedCurrenciesLookup ??= new();
            foreach (var currencyType in BDb.currencies.resources)
                if (!playerOwnedCurrenciesLookup.ContainsKey(currencyType.Id))
                    playerOwnedCurrenciesLookup[currencyType.Id] = 0;
        }

        [OnSerializing]
        public void OnSerializing()
        {
            playerOwnedCurrenciesSerialized = playerOwnedCurrenciesLookup.Select(x => new PurchasableHex.Price()
            {
                amount = x.Value,
                currencyId = HashCache.Get().Get(x.Key)
            }).ToList();
        }

        [OnDeserializing]
        public void OnDeserializing()
        {
            playerOwnedCurrenciesLookup = new();

            if (playerOwnedCurrenciesSerialized == null)
                return;

            foreach (var price in playerOwnedCurrenciesSerialized)
                playerOwnedCurrenciesLookup[price.currencyId] = price.amount;
        }

        internal void HideWorld(int worldId)
        {
            var world = ClusterManager.Instance.GetWorld(worldId);

            if (world == null) 
                return;

            var min = world.minimumBounds;
            var max = world.maximumBounds;

            for(var x = min.x; x <= max.x; x++)
            {
                for(var y = min.y; y <= max.y; y++)
                {
                    var cell = Grid.XYToCell((int)x, (int)y);
                    if (Grid.IsValidCell(cell))
                        HideCell(cell);
                }
            }

        }

        private void HideCell(int cell)
        {
            if (cell >= hiddenCells.Length || cell < 0)
                return;

            hiddenCells[cell] = true;
            hiddenCellsDirty = true;
        }

        public void AddZoneOverride(int cell, ZoneType zone)
        {
            zoneTypeOverrides ??= new();
            zoneTypeOverrides[cell] = zone;
        }

        public void RegenerateBackwallTexture()
        {
            if (World.Instance.zoneRenderData == null)
            {
                Debug.Log("Subworld zone render data is not yet initialized.");
                return;
            }

            var zoneRenderData = World.Instance.zoneRenderData;

            var zoneIndices = zoneRenderData.colourTex.GetRawTextureData();
            var colors = zoneRenderData.indexTex.GetRawTextureData();

            foreach (var tile in zoneTypeOverrides)
            {
                var cell = tile.Key;
                var zoneType = (byte)tile.Value;

                var color = World.Instance.zoneRenderData.zoneColours[zoneType];
                colors[cell] = (tile.Value == ZoneType.Space) ? byte.MaxValue : zoneType;

                zoneIndices[cell * 3] = color.r;
                zoneIndices[cell * 3 + 1] = color.g;
                zoneIndices[cell * 3 + 2] = color.b;

                World.Instance.zoneRenderData.worldZoneTypes[cell] = tile.Value;
            }

            zoneRenderData.colourTex.LoadRawTextureData(zoneIndices);
            zoneRenderData.indexTex.LoadRawTextureData(colors);
            zoneRenderData.colourTex.Apply();
            zoneRenderData.indexTex.Apply();

            zoneRenderData.OnShadersReloaded();
        }

    }
}
