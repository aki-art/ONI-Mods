using FUtility;
using KSerialization;
using UnityEngine;

namespace Backwalls.Buildings
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Backwall : KMonoBehaviour, IDyable
    {
        [Serialize]
        public HashedString variantID;

        [SerializeField]
        public BackwallVariant variant;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            GetComponent<KBatchedAnimController>().enabled = false;

            if(variant == null)
            {
                if(variantID == null)
                {
                    variantID = TileConfig.ID;
                }

                var newVariant = Mod.variants.Find(v => v.ID == variantID);
                if(newVariant != null)
                {
                    Log.Debuglog("SERT VARIANT TO " + newVariant.name);
                    SetVariant(newVariant);
                }
            }
        }

        public void SetVariant(BackwallVariant variant)
        {
            Log.Debuglog("setting variant: " + variant?.ID);
            if(variant == null || variant.atlas == null)
            {
                return;
            }

            var cell = Grid.PosToCell(this);

            if (this.variant != null)
            {
                Mod.renderer.RemoveBlock(this.variant, cell);
            }

            Mod.renderer.AddBlock((int)Grid.SceneLayer.Backwall, variant, cell);
            this.variant = variant;
            variantID = variant.ID;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();

            Mod.renderer.RemoveBlock(variant, Grid.PosToCell(this));
        }

        private void OnCopySettings(object obj)
        {
            if (((GameObject)obj).TryGetComponent(out Backwall wall))
            {
                SetVariant(wall.variant);
            }
        }
    }
}
