using Band.Components;
using Band.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UIElements;

namespace Band.Platform2D.Actions
{
    [RequireComponent(typeof(ConstantForce2D))]
    [RequireComponent(typeof(Collider2D))]
    public class CharacterJump : CharacterAction
    {

        [SerializeField]
        private float jumpHeight;

        private Vector3 velocity;

        private bool grounded;

        [SerializeField]
        private LayerMask groundLayerMask;

        private Rigidbody2D rigidBody;

        private Collider2D collider;

        [SerializeField]
        [Range(0.1f, 10)]
        private float jumpWeight;

        [SerializeField]
        [Range(1f, 10)]
        private float jumpBufferMultiplier;

        private bool wasJumping;

        private bool isJumping;

        private BandTimer jumpTimer;

        [SerializeField]
        private float jumpTime;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            velocity = Vector3.zero;
            grounded = false;
            rigidBody = this.GetComponent<Rigidbody2D>();
            collider = this.GetComponent<Collider2D>();
            jumpTimer=this.GetComponentInChildren<BandTimer>();
            jumpTimer.AddTimeoutListener(OnTimeout);
        }

        // Update is called once per frame
        protected override void Update()
        {
            wasJumping = isJumping;
            isJumping = GetInput<bool>();
            if (grounded)
            {
                if (isJumping && !wasJumping)
                { 
                    ComputeJump();
                    onActionStarted.Invoke();
                }
            }
            else if (isJumping && Vector3.Dot(-this.transform.up, velocity.normalized) <= 0 && jumpTimer.IsRunning)
               MantainJump();
            else StopJump();
            CheckGround();
            output = velocity;
        }

        private void ComputeJump()
        {
            velocity =  (Vector2) this.transform.up * Mathf.Sqrt(2 * jumpHeight);
            jumpTimer.Run();
        }

        private void MantainJump()
        {
            velocity += this.transform.up * jumpHeight * jumpBufferMultiplier*(1-jumpTimer.CurrentTime/jumpTimer.TotalTime)*Time.deltaTime;
        }

        private void StopJump()
        {
            DecreaseJump();
            jumpTimer.Stop();
        }

        private void DecreaseJump()
        {
            velocity = Vector3.zero;
        }

        private void OnTimeout()
        {
            StopJump();
        }

        private void CheckGround()
        {
            grounded = false;
            CapsuleCollider2D collider = this.GetComponent<CapsuleCollider2D>();
            Vector3 size = this.transform.localScale;
            Vector2 position = (Vector2)this.transform.position + collider.offset * size.magnitude;
            Collider2D[] hits = Physics2D.OverlapCapsuleAll(position,size*collider.size, collider.direction,0,groundLayerMask);
            for(int i=0;i<hits.Length && !grounded;i++)
            {
                Collider2D hit = hits[i];
                if (hit == collider)
                    continue;
                ColliderDistance2D colliderDistance = hit.Distance(collider);
                if (Vector3.Angle(colliderDistance.normal, this.transform.up) == 0)
                {
                    grounded = true;
                    onActionFinished.Invoke();
                }
                Debug.DrawRay(position, -this.transform.up.normalized * transform.localScale.magnitude / 2, grounded ? UnityEngine.Color.green : UnityEngine.Color.red);
            }
        }




        private void OnDrawGizmos()
        {
            CapsuleCollider2D collider = this.GetComponent<CapsuleCollider2D>();
            Vector3 size = (this.transform.localScale * collider.size);
            Vector3 origin = (Vector2)this.transform.position + collider.offset * size.magnitude;
            Vector3 direction = -this.transform.up.normalized * size.magnitude;
            Gizmos.color= grounded ? UnityEngine.Color.green: UnityEngine.Color.red;
            Gizmos.DrawWireCube(origin, size);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ContactPoint2D contactPoint = collision.contacts[0];
            Vector2 pointPosition = contactPoint.point;
            Vector2 position=collider.offset + (Vector2) this.transform.position;
            Vector3 vector=pointPosition - position;
            if (Vector3.Dot(vector.normalized, this.transform.up) > 0)
                StopJump();
        }

        private void FixedUpdate()
        {
            if(velocity.magnitude!=0)
            {
                Vector3 vector = rigidBody.linearVelocity;
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, velocity.y);
            }
        }
    }
}
