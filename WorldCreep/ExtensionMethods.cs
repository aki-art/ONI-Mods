using UnityEngine;

namespace WorldCreep
{
    public static class ExtensionMethods
    {
        public static bool IsAlmost(this float input, float compareTo, float threshold = 0.01f)
        {
            return Mathf.Abs(compareTo - input) <= threshold;
        }

        // TODO: replace with real TryGetComponent if Unity 2019 is used
        public static bool TryGetComponent<T>(this GameObject gameObject, out T component)
        {
            component = default;
            if (gameObject == null)
                return false;

            component = gameObject.GetComponent<T>();
            if (component == null)
                return false;

            return true;
        }

        public static bool TryGetComponent<T>(this Component sourceComponent, out T component)
        {
            return sourceComponent.gameObject.TryGetComponent(out component);
        }
    }
}
