using Band.Platform2D.Actions;
using Band.Platform2D.Character;
using Band.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Band.Commands
{
    public abstract class InputCommander : Commander, IInputable
    {
        [SerializeField]
        protected InputActionAsset inputAsset;

        //protected IInputActionCollection2 inputAsset;

        protected virtual void OnEnable()
        {
            inputAsset.Enable();
        }

        protected virtual void OnDisable()
        {
            inputAsset.Disable();
        }

        public void Enable()
        {
            OnEnable();
        }

        public void Disable()
        {
            OnDisable();
        }

        public void SetupCommands()
        {
            if (inputAsset != null)
            {
                inputAsset.Enable();
                foreach(InputActionMap map in inputAsset.actionMaps)
                    foreach(InputAction action in map.actions)
                    {
                        action.performed += (context) => this.gameObject.SendMessage("On" + map.name+action.name+"Performed", context);
                        if(!action.type.Equals(InputActionType.PassThrough))
                        {
                            action.started += (context) => this.gameObject.SendMessage("On" + map.name + action.name + "Started", context);
                            action.canceled += (context) => this.gameObject.SendMessage("On" + map.name + action.name + "Canceled", context);
                        }
                        //action.canceled += (context) => this.gameObject.SendMessage("On" + action.name, context);

                    }
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            SetupCommands();

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static List<InputCommander> FindActiveCommanders()
        {
            List<InputCommander> commanders = GameObject.FindObjectsByType<InputCommander>(FindObjectsSortMode.InstanceID).ToList<InputCommander>();
            commanders = commanders.Where((commander)=>commander.enabled).ToList();
            return commanders;
        }

    }
}
