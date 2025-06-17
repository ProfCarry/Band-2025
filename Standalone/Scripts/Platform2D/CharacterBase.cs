using Band.Platform2D.Actions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Band.Extensions;
using UnityEngine.Events;
using System;
using System.Collections;

namespace Band.Platform2D.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterBase : MonoBehaviour
    {
        protected List<CharacterAction> actions;

        protected Rigidbody2D rigidBody;

        protected UnityEvent<Collider2D> triggerEvent;
        protected UnityEvent<Collision2D> collisionEvent;

        private Vector3 velocity;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            triggerEvent = new UnityEvent<Collider2D>();
            collisionEvent = new UnityEvent<Collision2D>();
            rigidBody = this.GetComponent<Rigidbody2D>();
            actions = this.GetComponents<CharacterAction>().ToList<CharacterAction>();
        }


        // Update is called once per frame
        protected virtual void Update()
        {
            
        }

        private void FixedUpdate()
        {
            velocity = actions.Select(a => a.GetOutput())
                .Aggregate((acc, a) => acc + a);
            //Vector3 newPos = this.transform.position + velocity * Time.fixedDeltaTime;
            //rigidBody.MovePosition(newPos);
            rigidBody.linearVelocity= velocity;
            
        }

        public void DoAction<TAction>(object input) where TAction : CharacterAction
        {
            // input = null;
            TAction action = (TAction) actions.First((comp) => comp is TAction);
            action.SetInput(input);
        }

        public void StopActions()
        {
            foreach(var action in actions)
                action.EraseInput();
        }

        public void AddTriggerEvent(UnityAction<Collider2D> action)
        {
            triggerEvent.AddListener(action);
        }

        public void AddCollisionEvent(UnityAction<Collision2D> action)
        {
            collisionEvent.AddListener(action);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            triggerEvent.Invoke(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collisionEvent.Invoke(collision);
        }
    }
}
