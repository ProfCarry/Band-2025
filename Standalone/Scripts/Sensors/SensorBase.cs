using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

namespace Band.Sensor
{
    public abstract class SensorBase : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<GameObject> onSensorDetect;

        public UnityEvent<GameObject> OnSensorDetect { get { return onSensorDetect; } }

        [SerializeField]
        private LayerMask detectionMask;

        public LayerMask DetectionMask { get { return detectionMask; } set { detectionMask = value; } }

        [SerializeField]
        private float detectionInterval = 0;

        private GameObject lastDetectedObject;

        [SerializeField]
        private bool autoActivate = false;

        public bool AutoActivate { get { return autoActivate; } set { autoActivate = value; } }

        private Coroutine detectionCoroutine;

        public abstract GameObject Detect();

        private void Start()
        {
            if (autoActivate && detectionCoroutine == null)
                detectionCoroutine = StartCoroutine(DetectionCoroutine());
        }

        private IEnumerator DetectionCoroutine()
        {
            bool end = false;
            while (autoActivate && !end)
            {
                float timeInterval = detectionInterval > 0 ? detectionInterval : Time.deltaTime;
                yield return new WaitForSecondsRealtime(timeInterval);
                GameObject game = Detect();
                if (game != null)
                    OnSensorDetect.Invoke(game);
            }
        }

        public bool StartDetection()
        {
            if (detectionCoroutine != null)
                Start();
            return detectionCoroutine != null;
        }

        public void StopDetection()
        {
            if (detectionCoroutine != null)
            {
                StopCoroutine(detectionCoroutine);
                detectionCoroutine = null;
            }
        }

    }
}
