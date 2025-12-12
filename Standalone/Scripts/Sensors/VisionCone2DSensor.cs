using UnityEngine;
using Band.Extensions;
using Unity.VisualScripting;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Band.Sensor
{
    public class VisionCone2DSensor : SensorBase
    {
        [SerializeField]
        private float maxLength;

        private float length;

        [SerializeField]
        [Range(0f, 90f)]
        private float maxAngle;

        private float angle;

        [SerializeField]
        private bool usedFixedLength = false;

        [SerializeField]
        private bool usedFixedAngle = false;

        public override GameObject Detect()
        {
            if (!usedFixedLength)
            {
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.right, maxLength);
                if (hit)
                {
                    length = Vector3.Distance(hit.transform.position, this.transform.position);
                    if (length > maxLength)
                        length = maxLength;
                }
                else length = maxLength;
            }
            else length = maxLength;
            if (!usedFixedAngle)
                angle = maxAngle / length;
            else angle = maxAngle;
            Collider2D collider = Physics2D.OverlapCircle(this.transform.position,length,DetectionMask);

            GameObject gameObject = null;
            if (collider != null)
            {
                Vector3 direction = (collider.transform.position - this.transform.position).normalized;
                float angle = Vector3.Angle(this.transform.right, direction);
                if (Mathf.Abs(angle) < this.angle / 2)
                    gameObject = collider.gameObject;
            }
            return gameObject;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            GameObject game=Detect();
            Handles.color = game==null ? Color.red : Color.green;
            Vector3 origin = this.transform.position;
            Vector3 forward = this.transform.right;
            Vector3 leftDirection = Quaternion.AngleAxis(-angle / 2, Vector3.forward) * forward;
            Vector3 rightDirection = Quaternion.AngleAxis(angle / 2, Vector3.forward) * forward;
            Vector3 leftPoint = origin + leftDirection * length;
            Vector3 rightPoint = origin + rightDirection * length;
            Vector3 middlePoint = origin + this.transform.right * length;
            Vector3 leftMiddlePoint = origin + (leftDirection + this.transform.right).normalized * length;
            Vector3 rightMiddlePoint = origin + (rightDirection + this.transform.right).normalized * length;
            Vector3[] vertices = new Vector3[]
            {
                origin,
                leftPoint,
                leftMiddlePoint,
                middlePoint,
                rightMiddlePoint,
                rightPoint
            };
            Handles.DrawAAConvexPolygon(vertices);
            Handles.color = Color.black;
            //Handles.DrawAAPolyLine(origin, leftPoint, rightPoint, origin);
#endif
        }

    }
}
