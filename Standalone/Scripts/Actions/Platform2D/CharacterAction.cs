using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Band.Platform2D.Actions
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public abstract class CharacterAction : MonoBehaviour
    {
        protected Dictionary<string,object> inputs;

        protected string inputKey;

        protected Vector3 output;

        [SerializeField]
        private UnityEvent<Collider2D> triggerEvent;

        [SerializeField]
        protected UnityEvent onActionStarted;

        [SerializeField]
        protected UnityEvent onActionFinished;

        protected float weight;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            inputs = new Dictionary<string,object>();
            inputKey=this.GetType().Name;
            triggerEvent = new UnityEvent<Collider2D>();
            weight = 1;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
           // inputs.Clear();
        }

        public void SetInput(object value)
        {
            inputs[inputKey] = value;
        }

        public Vector3 GetOutput()
        {
            return output;
        }

        public TInput GetInput<TInput>()
        {
            TInput value=default;
            try
            {
                value =(TInput) inputs[inputKey];
                if (value == null)
                    value = default(TInput);
            }
            catch(Exception e)
            {
                print(e.Message);
                value = default(TInput);
            }
            return value;
        }

        public void SetWeight(float weight)
        {
            this.weight = weight;
        }

        public void AddTriggerEvent(UnityAction<Collider2D> action)
        {
            triggerEvent.AddListener(action);
        }

        public void EraseInput()
        {
            SetInput(new object());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            triggerEvent.Invoke(collision);
        }

        public void AddOnActionStartedEvent(UnityAction action)
        {
            onActionStarted.AddListener(action);
        }

        public void AddOnActionFinishedEvent(UnityAction action)
        {
            onActionFinished.AddListener(action);
        }
    }

}