using System;
using Unity.VisualScripting;
using UnityEngine;

public static class ObjectExtensions
{
    public static bool HasComponent(this UnityEngine.Object obj, Type type)
    {
        return obj.GetComponent(type) != null;
    }

    public static bool HasComponent<TObject>(this UnityEngine.Object obj)
    {
        return HasComponent(obj, typeof(TObject));
    }
}
