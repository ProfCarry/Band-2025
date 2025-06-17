using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Band.Extensions
{
    public static class ListExtensions
    {
        public static object FindByType(this List<object> list, Type type)
        {
            object found = null;
            for (int i = 0; i < list.Count && found != null; i++)
            {
                object obj = list[i];
                if (obj.GetType().Equals(type))
                    found = obj;
            }
            return found;
        }

        public static object FindByType<T>(this List<object> list)
        {
            return FindByType(list, typeof(T));
        }
    }
}
