using Band.Extensions;
using System;
using UnityEngine;

namespace Band.Platform2D.Actions
{
    public class CharacterMovement : CharacterAction
    {
        [SerializeField]
        [Range(0f,500f)]
        private float speed;

        [SerializeField]
        [Range(0f, 500f)]
        private float acceleration;

        private Vector3 velocity;

        private Rigidbody2D rigidBody;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            rigidBody = this.GetComponent<Rigidbody2D>();
            velocity = rigidBody.linearVelocity;
        }

        // Update is called once per frame
        protected override void Update()
        {
            Vector3 inputVector = GetInput<Vector3>();
            if (Vector3.Dot(inputVector.normalized,this.transform.right) != 0)
                StartMovement(inputVector);
            else StopMovement(inputVector);
        }

        private void StopMovement(Vector3 inputVector)
        {
            velocity=inputVector * Mathf.MoveTowards(velocity.magnitude,0, acceleration*Time.fixedDeltaTime);

        }

        private void StartMovement(Vector3 vector)
        {
            velocity = vector.normalized * Mathf.MoveTowards(vector.magnitude, speed * vector.magnitude, acceleration * Time.fixedDeltaTime);
        }

        private void FixedUpdate()
        {
            Vector3 vector = rigidBody.linearVelocity;
            rigidBody.linearVelocity = new Vector2(velocity.x, rigidBody.linearVelocityY);

        }
    }

    
}

