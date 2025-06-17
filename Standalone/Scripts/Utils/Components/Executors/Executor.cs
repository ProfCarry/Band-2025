using UnityEngine;

namespace Band.Utils.Execution
{
    public abstract class Executor : MonoBehaviour
    {
        [SerializeField]
        private string id;

        public string Id { get { return id; } }

        public abstract void Execute();
    }
}
