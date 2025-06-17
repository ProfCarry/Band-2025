using UnityEngine;

namespace Band.Utils.Execution
{
    public abstract class Loader : Executor
    {


        public abstract object Load();

        public override void Execute()
        {
            Load();
        }
    }
}
