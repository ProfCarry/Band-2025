using UnityEngine;

namespace Band.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 NonZeroAddition(this Vector3 vector, Vector3 value)
        {
            Vector3 ret = vector;
            if (value.x != 0)
                ret.x = value.x;
            if (value.y != 0)
                ret.y = value.y;
            if (value.z != 0)
                ret.z = value.z;
            return ret;
        }
    }
}
