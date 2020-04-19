using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asphalt
{
    internal static class CloneExtension
    {
        public static T Clone<T>(this T obj)
        {
            var inst = obj.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            return (T)inst?.Invoke(obj, null);
        }
    }
}
