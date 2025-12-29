using Band.Extensions;
using UnityEngine;

namespace Band.Sensor
{

    public abstract class Sensor2DBase : SensorBase
    {
        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.IsContainedLayer(DetectionMask))
            {
                candidates.Add(collision);
                MarkDirty();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (candidates.Contains(collision))
            {
                candidates.Remove(collision);
                MarkDirty();
            }
        }

    }
}
