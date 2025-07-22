using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Band.Components
{
    public class BandTimer : MonoBehaviour
    {
        [SerializeField]
        private float time=1;

        public float TotalTime {  get { return time; } set { time = value; } }

        [SerializeField]
        private bool autostart;

        [SerializeField]
        private UnityEvent timeoutHandler;

        public float CurrentTime { get; private set; }

        private enum State
        { 
            Running,
            Pause,
            Stop
        }

        private State state;

        public bool IsRunning { get { return state == State.Running; } }
        public bool IsPaused { get { return state == State.Pause; } }
        public bool IsStopped { get { return state == State.Stop; } }

        public void Run()
        {
            state= State.Running;
            Resume();
        }

        public void Pause()
        {
            state = State.Pause;
        }

        public void Stop()
        { 
            state = State.Stop;
            CurrentTime=TotalTime;
            
        }

        public void Resume()
        {
            CurrentTime = 0;
        }

        public void AddTimeoutListener(UnityAction listener)
        {
            timeoutHandler.AddListener(listener);
        }

        private void Awake()
        {
            timeoutHandler = new UnityEvent();
        }

        // Start
        // is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            Stop();
            if (autostart)
                Run();
        }

        // Update is called once per frame
        void Update()
        {
            if (state == State.Running)
            { 
                CurrentTime += Time.deltaTime;
                if (CurrentTime >= time)
                {
                    Stop();
                    timeoutHandler.Invoke();
                }
            }
            
        }
    }

}