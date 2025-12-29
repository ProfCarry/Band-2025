using UnityEngine;




#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Band.Sensor
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class VisionCone2DSensor : Sensor2DBase
    {
        [SerializeField]
        private float maxLength;

        [SerializeField]
        [Range(0f, 180f)]
        private float maxAngle;


        private CircleCollider2D circleCollider;

        private void Awake()
        {
           circleCollider=this.GetComponent<CircleCollider2D>();
           circleCollider.isTrigger = true;
           circleCollider.radius=maxLength;
        }

        private bool IsVisible(Collider2D target)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(transform.right, direction);
            if (angleToTarget > maxAngle * 0.5f)
                return false;
            float distance = Vector2.Distance(transform.position, target.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                direction,
                distance,
                OcclusionMask
            );
            return hit.collider == null;
        }

        public override GameObject Detect()
        {
            if (!dirty || candidates.Count == 0)
                return null;
            dirty = false;
            foreach(Collider2D target in candidates)
            {
                if (target == null) continue;
                if (!this.IsVisible(target)) continue;
                return target.gameObject;
            }
            return null;
        }



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 origin = transform.position;
            Vector3 forward = transform.right;

            int segments = 20; // higher = smoother cone
            float halfAngle = maxAngle / 2f;

            Vector3[] vertices = new Vector3[segments + 2];
            vertices[0] = origin; // cone tip

            for (int i = 0; i <= segments; i++)
            {
                float angle = -halfAngle + (maxAngle / segments) * i;
                Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * forward;
                vertices[i + 1] = origin + dir * maxLength;
            }

            Handles.color = lastDetectedObject != null ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            Handles.DrawAAConvexPolygon(vertices);
        }
#endif
    }


}
