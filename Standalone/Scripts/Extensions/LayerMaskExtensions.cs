using UnityEngine;

namespace Band.Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool ContainLayer(this LayerMask mask1, LayerMask mask2)
        {
            return ((mask1.value & (1 << mask2)) > 0);
        }
    }
}
