using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Band.Utils;
using UnityEngine.InputSystem.XR;

namespace Band.Core
{
    public class GameController : MonoBehaviour, IController
    {
        private static GameController instance;

        public static GameController Instance { get; private set; }


        private List<IController> controllers;

        private Dictionary<string, object> data;


        private void Start()
        {
            data = new Dictionary<string, object>();
            DontDestroyOnLoad(this.gameObject);
            controllers = this.GetComponents<IController>().ToList<IController>();
        }

        private void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this.gameObject);
            else instance = this;
        }

        public T GetController<T>() where T : IController
        {
            IController controller = null;
            foreach (IController cont in controllers)
                if (cont is T)
                    controller = cont;
            if (controller == null)
                throw new NullReferenceException($"There is no object of type ${typeof(T)}");
            return (T)controller;
        }

        public object GetValue(string key)
        {
            return data[key];
        }

        public void SetValue(string key, object value)
        {
            data[key] = value;
        }

        public void Clear()
        {
            data.Clear();
            controllers.ForEach(controller => controller.Clear());
        }
    }
}