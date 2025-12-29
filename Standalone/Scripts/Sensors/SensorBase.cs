using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Band.Sensor
{

    public abstract class SensorBase : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<GameObject> onSensorDetect;

        [SerializeField]
        private UnityEvent<GameObject> onSensorLost;


        public UnityEvent<GameObject> OnSensorDetect { get { return onSensorDetect; } }
        public UnityEvent<GameObject> OnSensorLost { get { return onSensorLost; } }

        [SerializeField]
        private LayerMask detectionMask;

        public LayerMask DetectionMask { get { return detectionMask; } set { detectionMask = value; } }

        [SerializeField]
        private LayerMask occlusionMask;

        public LayerMask OcclusionMask { get { return occlusionMask; } set { occlusionMask = value; } } 

        [SerializeField]
        private float detectionInterval = 0;

        protected HashSet<Collider2D> candidates = new();
        protected bool dirty = false;
        private float lastZRotation;
        private Vector2 lastPosition;

        protected GameObject lastDetectedObject=null;

        public void MarkDirty()
        {
            dirty = true;
            Evaluate();
        }

        protected void Evaluate()
        {
            GameObject seen = Detect();
            GameObject previous = lastDetectedObject;
            if (seen != null)
            {
                if(previous!=seen)
                    OnSensorDetect.Invoke(seen);
                lastDetectedObject = seen;
            }
            else
            {
                if(previous != null)
                    OnSensorLost.Invoke(previous);
                lastDetectedObject = null;
            }
            dirty = false;
        }

        private void LateUpdate()
        {
            Vector2 currentPos = transform.position;
            float currentRot = transform.eulerAngles.z;

            if (currentPos != lastPosition || !Mathf.Approximately(currentRot, lastZRotation))
            {
                lastPosition = currentPos;
                lastZRotation = currentRot;
                MarkDirty();
            }
        }

        public abstract GameObject Detect();

    }
}
