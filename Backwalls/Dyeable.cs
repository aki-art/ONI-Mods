using KSerialization;
using UnityEngine;

namespace Backwalls
{
    public class Dyeable : KMonoBehaviour
    {
        [Serialize]
        public Color color = new Color(0.7372549f, 0.7372549f, 0.7372549f);

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            SetColor(color);
        }

        public void SetColor(Color color)
        {
            var cell = Grid.PosToCell(this);
            Mod.renderer.colorInfos[cell] = color;
            Mod.renderer.Rebuild(cell);

            this.color = color;
        }

        private void OnCopySettings(object obj)
        {
            if (((GameObject)obj).TryGetComponent(out Dyeable dyable))
            {
                SetColor(dyable.color);
            }
        }
    }
}
