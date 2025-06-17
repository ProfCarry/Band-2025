using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

namespace Band.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool IsContainedLayer(this GameObject gameobject, LayerMask mask)
        {
            return LayerMaskExtensions.ContainLayer(mask,gameobject.layer);
        }

    }
}
