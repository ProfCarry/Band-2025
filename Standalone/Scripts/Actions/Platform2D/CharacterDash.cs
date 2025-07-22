using Band.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Band.Platform2D.Actions
{
    public class CharacterDash : CharacterAction
    {
        private bool canDash;

        private bool isDashing;

        [SerializeField]
        private bool cannotDashVertically;

        [SerializeField]
        private bool noPhysicsWhenDashing;

        public bool CanDash { get { return canDash; } }
        public bool IsDashing { get { return isDashing; } }

        [SerializeField]
        private float dashPower;

        public float DashPower { get { return dashPower; } }

        [SerializeField]
        private float dashTime;

        [SerializeField]
        [Range(0f, 10f)]
        private float cooldown;

       // private ConstantForce2D constantForce;

        private Rigidbody2D rigidbody;

        [SerializeField]
        protected UnityEvent onDashTimeElapsed;

        [SerializeField]
        private bool isTopDown;

        protected override void Start()
        {
            base.Start();
            canDash = true;
            isDashing = false;
            rigidbody = this.GetComponent<Rigidbody2D>();
        }

        protected override void Update()
        {
            Vector3 input=GetInput<Vector3>();
            if(canDash && !isDashing && input!=Vector3.zero)
            {
                StartCoroutine(DashCoroutine(input));
            }
            base.Update();
        }

        private IEnumerator DashCoroutine(Vector3 direction)
        {
            Vector3 vector = this.transform.right *this.transform.localScale.x;
            if (cannotDashVertically)
                vector = Vector3.Project(vector, this.transform.right).normalized;

            if(vector.magnitude!=0)
            {
                canDash = false;
                isDashing = true;
                output = vector * dashPower;
                if (noPhysicsWhenDashing)
                {
                    rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | (Vector3.Dot(vector,Vector3.right)!=0 ? RigidbodyConstraints2D.FreezePositionY : RigidbodyConstraints2D.FreezePositionX);
                }
                onActionStarted.Invoke();
                yield return new WaitForSecondsRealtime(dashTime);
                output = Vector3.zero;
                isDashing = false;
                if (noPhysicsWhenDashing)
                    rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                onActionFinished.Invoke();
                yield return new WaitForSecondsRealtime(cooldown);
                onDashTimeElapsed.Invoke();
                canDash = true;
            }
        }

        public void AddOnDashTimeElapsedEvent(UnityAction action)
        {
            onDashTimeElapsed.AddListener(action);
        }

        private void FixedUpdate()
        {
            if(output.magnitude!=0)
            {
                Vector3 vector = rigidbody.linearVelocity;
                if (!isTopDown)
                    rigidbody.linearVelocity = new Vector2(output.x, rigidbody.linearVelocityY);
                else
                    rigidbody.linearVelocity = output;
            }
        }

    }
}
