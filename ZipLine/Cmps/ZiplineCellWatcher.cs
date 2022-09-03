using ZipLine.Buildings.ZiplinePost;

namespace ZipLine.Cmps
{
    internal class ZiplineCellWatcher : KMonoBehaviour
    {
        public static ZiplineCellWatcher Instance;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void AddCell(int cell, ZiplineAnchor anchor)
        {

        }
    }
}
