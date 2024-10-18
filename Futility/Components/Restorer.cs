using UnityEngine;

namespace FUtility.Components
{
    public class Restorer : KMonoBehaviour
    {
        //public delegate void RestorerFn(GameObject go);

        //[SerializeField]
        //public RestorerFn onSaveGame;

        //[SerializeField]
        //public RestorerFn onRestore;

        public virtual void OnSaveGame()
        {
            //onSaveGame?.Invoke(gameObject);
        }

        public virtual void OnRestore()
        {
            //onRestore?.Invoke(gameObject);
        }
    }
}
