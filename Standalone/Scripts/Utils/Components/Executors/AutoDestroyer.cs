using UnityEngine;

namespace Band.Utils.Execution
{
    public class AutoDestroyer : Executor
    {
        public override void Execute()
        {
            Destroy(this.gameObject);
        }
    }
}
