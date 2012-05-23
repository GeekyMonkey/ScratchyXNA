using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ScratchyXna
{
    public static class EnumerationExtensions
    {
        public static IEnumerable<T> GetAllValues<T>(this T enumeration)
        {
            List<T> enumerations = new List<T>();
            foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumerations.Add((T)fieldInfo.GetValue(enumeration));
            }
            return enumerations;
        }
    }
}
