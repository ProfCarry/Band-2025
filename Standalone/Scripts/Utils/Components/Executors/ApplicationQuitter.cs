using UnityEngine;

namespace Band.Utils.Execution
{
    public class ApplicationQuitter : Executor
    {
        public override void Execute()
        {
            Application.Quit();
        }
    }
}
