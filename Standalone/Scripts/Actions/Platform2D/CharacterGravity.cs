using UnityEngine;

namespace Band.Platform2D.Actions
{
    public class CharacterGravity : CharacterAction
    {
        [SerializeField]
        private Vector3 gravity;

        [SerializeField]
        private LayerMask groundLayerMask;

        private bool grounded;

        private Vector3 velocity;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            velocity = Vector3.zero;
        }

        // Update is called once per frame
        protected override void Update()
        {
            if(!grounded)
            {
                velocity += gravity * Time.deltaTime;
            }
            else velocity = Vector3.zero;
            if (velocity.magnitude > gravity.magnitude)
                velocity = gravity;
            output = velocity;
            //output = gravity;
            CheckGround();
        }

        private void CheckGround()
        {
            grounded = false;
            CapsuleCollider2D collider = this.GetComponent<CapsuleCollider2D>();
            Vector3 size = this.transform.localScale;
            Vector2 position = (Vector2)this.transform.position + collider.offset * size.magnitude;
            Collider2D[] hits = Physics2D.OverlapCapsuleAll(position, size * collider.size, collider.direction, 0, groundLayerMask);
            for (int i = 0; i < hits.Length && !grounded; i++)
            {
                Collider2D hit = hits[i];
                if (hit == collider)
                    continue;
                ColliderDistance2D colliderDistance = hit.Distance(collider);
                if (Vector3.Angle(colliderDistance.normal, this.transform.up) == 0)
                    grounded = true;
                //grounded = Physics2D.Raycast(position, gravity.normalized * (0.1f + transform.localScale.magnitude) / 2, groundLayerMask);
                Debug.DrawRay(position, gravity.normalized * transform.localScale.magnitude / 2, grounded ? UnityEngine.Color.green : UnityEngine.Color.red);
            }
        }
    }
}
