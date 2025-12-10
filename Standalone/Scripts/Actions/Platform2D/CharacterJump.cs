using Band.Components;
using Band.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.Events;
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

        private bool canJump;

        private BandTimer jumpTimer;

        private float coyoteTimer;

        [SerializeField]
        private float jumpTime;

        [SerializeField]
        private float coyoteTime;

        private Vector3 gravity;
        public Vector3 Gravity { get { return gravity; } set { gravity = value; } }

        private BandTimer PrepareTimer(string name, float time,UnityAction action=null)
        {

            GameObject game=new GameObject(name);
            game.transform.SetParent(this.transform);
            BandTimer timer = game.AddComponent<BandTimer>();
            timer.TotalTime = time;
            if (action != null)
                timer.AddTimeoutListener(action);
            return timer;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            velocity = Vector3.zero;
            grounded = false;
            rigidBody = this.GetComponent<Rigidbody2D>();
            collider = this.GetComponent<Collider2D>();
            jumpTimer = PrepareTimer("JumpTimer", jumpTime,OnJumpTimerTimeout);
        }

        // Update is called once per frame
        protected override void Update()
        {
            wasJumping = isJumping;

            isJumping = GetInput<bool>();
            if (grounded)
            {
                coyoteTimer = 0;
            }
            else coyoteTimer += Time.deltaTime;

            if (coyoteTimer < coyoteTime && isJumping && !wasJumping)
            {
                ComputeJump();
                coyoteTimer = coyoteTime;
                onActionStarted.Invoke();
                //coyoteTimer.Stop();
            }
            else if (isJumping && Vector3.Dot(gravity.normalized, velocity.normalized) <= 0 && jumpTimer.IsRunning )
               MantainJump();
            else StopJump();
            CheckGround();
            if (output == Vector3.zero)
                output = velocity;
            
        }

        private void ComputeJump()
        {
            velocity =  (Vector2) this.transform.up * Mathf.Sqrt(2 * jumpHeight*(-gravity).magnitude/2);
            jumpTimer.Run();
        }

        private void MantainJump()
        {
            velocity += this.transform.up * jumpHeight * jumpBufferMultiplier*(1-jumpTimer.CurrentTime/jumpTimer.TotalTime)*Time.deltaTime+(gravity / 2)*Time.deltaTime;
            if(velocity.y >= 2*jumpHeight)
                velocity.y = 2*jumpHeight;
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

        private void OnJumpTimerTimeout()
        {
            StopJump();
        }

        private void OnCoyoteTimerTimeout()
        {
            canJump = false;
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
            /*if(rigidBody.linearVelocity.magnitude>gravity.magnitude && Vector3.Dot(rigidBody.linearVelocity,gravity)>0)
            {
                rigidBody.linearVelocity=gravity;
            }*/
        }
    }
}
