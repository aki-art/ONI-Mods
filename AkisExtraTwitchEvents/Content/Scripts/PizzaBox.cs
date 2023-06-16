using KSerialization;
using System;

namespace Twitchery.Content.Scripts
{
    public class PizzaBox : KMonoBehaviour
    {
        [MyCmpReq]
        public Storage storage;

        public override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged);
        }

        private void OnStorageChanged(object obj)
        {
            if(storage.IsEmpty())
            {
                Util.KDestroyGameObject(gameObject);
            }
        }
    }
}
