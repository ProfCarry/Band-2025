using System.Collections;
using UnityEngine;

namespace Band.Utils.Loading
{
    public abstract class Loader : MonoBehaviour
    {
        [SerializeField]
        private bool useAsyncLoad;

        public void Load()
        {
            if (useAsyncLoad)
                StartCoroutine(AsyncLoad());
            else SyncLoad();
        }

        protected abstract void SyncLoad();

        protected abstract IEnumerator AsyncLoad();

    }
}
