using Band.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Band.Platform2D.Actions
{
    //[RequireComponent(typeof(ConstantForce2D))]
    [RequireComponent(typeof(Collider2D))]
    public class CharacterJump : CharacterAction
    {
        [SerializeField]
        private Vector3 gravity;

        [SerializeField]
        private float jumpHeight;

        private Vector3 velocity;

        private bool grounded;

        [SerializeField]
        private LayerMask groundLayerMask;

        //private ConstantForce2D force;

        private Rigidbody2D rigidBody;

        private Collider2D collider;
        public Vector3 jumpDirection { get { return -gravity.normalized; } }

        [SerializeField]
        [Range(0.1f, 10)]
        private float jumpWeight;

        private bool wasJumping;

        private bool isJumping;

        private BandTimer jumpTimer;

        [SerializeField]
        private float jumpTime;

        private EnlargeReduce enlargeReduce;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            velocity = Vector3.zero;
            grounded = false;
            //force=this.GetComponent<ConstantForce2D>();
            //force.force=gravity*weight;
            rigidBody = this.GetComponent<Rigidbody2D>();
            collider = this.GetComponent<Collider2D>();
            jumpTimer=this.GetComponentInChildren<BandTimer>();
            jumpTimer.AddTimeoutListener(OnTimeout);
            enlargeReduce=this.GetComponent<EnlargeReduce>();   
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
            else if (isJumping && Vector3.Dot(gravity.normalized, velocity.normalized) <= 0 && jumpTimer.IsRunning)
               MantainJump();
            //MantainJump();
            else StopJump();
            DecreaseJump();
            CheckGround();
            output = velocity;
            //base.Update();
        }

        private void ComputeJump()
        {
            velocity =  (Vector2) jumpDirection * Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(gravity.magnitude));
            jumpTimer.Run();
        }

        private void MantainJump()
        {
            //velocity = rigidBody.linearVelocity;
            //velocity = (Vector2)jumpDirection * Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(gravity.magnitude));
            //velocity += jumpDirection * (jumpHeight) * Time.fixedDeltaTime;
            //velocity += jumpDirection * Mathf.Sqrt(2 * jumpHeight)
            //    * Time.deltaTime * (1 - jumpTimer.CurrentTime/jumpTimer.TotalTime);
            velocity += jumpDirection * jumpHeight * Time.deltaTime;

        }

        private void StopJump()
        {
            //velocity = rigidBody.linearVelocity;
            //velocity = (Vector2)jumpDirection * Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(gravity.magnitude));
            DecreaseJump();
            //velocity = Vector3.zero;
            jumpTimer.Stop();
        }

        private void DecreaseJump()
        {
            if(rigidBody.constraints.Equals(RigidbodyConstraints2D.FreezeRotation | (RigidbodyConstraints2D.FreezePositionX)) ||
                rigidBody.constraints.Equals(RigidbodyConstraints2D.FreezeRotation | (RigidbodyConstraints2D.FreezePositionY)))
            {
                velocity = Vector3.zero;
            }
            else
            {
                velocity += gravity.normalized * (gravity.magnitude * (jumpWeight + enlargeReduce.Weight)) * Time.deltaTime;
                if (velocity.magnitude > (gravity.magnitude + jumpWeight))
                    velocity = velocity.normalized * gravity.magnitude;
            }
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
                    //grounded = Physics2D.Raycast(position, gravity.normalized * (0.1f + transform.localScale.magnitude) / 2, groundLayerMask);
                Debug.DrawRay(position, gravity.normalized * transform.localScale.magnitude / 2, grounded ? UnityEngine.Color.green : UnityEngine.Color.red);
            }
        }




        private void OnDrawGizmos()
        {
            CapsuleCollider2D collider = this.GetComponent<CapsuleCollider2D>();
            Vector3 size = (this.transform.localScale * collider.size);
            Vector3 origin = (Vector2)this.transform.position + collider.offset * size.magnitude;
            Vector3 direction = gravity.normalized * size.magnitude;
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
                velocity=Vector3.zero;
        }
    }
}
