using FUtility;
using KSerialization;
using UnityEngine;

namespace Backwalls.Buildings
{
    public class Backwall : KMonoBehaviour
    {
        [Serialize]
        public string tag;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            GetComponent<KBatchedAnimController>().enabled = false;

            if (tag.IsNullOrWhiteSpace() || Assets.GetBuildingDef(tag) == null)
            {
                tag = TileConfig.ID;
            }

            var def = Assets.GetBuildingDef(tag);
            Mod.renderer.AddBlock((int)Grid.SceneLayer.Backwall, def, tag, Grid.PosToCell(this));
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();

            Mod.renderer.RemoveBlock(tag, Grid.PosToCell(this));
        }

        private void OnCopySettings(object obj)
        {
            if (((GameObject)obj).TryGetComponent(out Backwall wall))
            {
                SetDef(wall.tag);
            }
        }

        public void SetDef(string tag)
        {
            var cell = Grid.PosToCell(this);
            Mod.renderer.RemoveBlock(this.tag, cell);
            this.tag = tag;
            var def = Assets.GetBuildingDef(tag);
            Mod.renderer.AddBlock((int)Grid.SceneLayer.Backwall, def, def.PrefabID, cell);
        }
    }
}
