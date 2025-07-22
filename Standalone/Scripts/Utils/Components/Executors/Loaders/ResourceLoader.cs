using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Band.Utils.Loading
{
    public class ResourceLoader : Loader
    {
        [SerializeField]
        private bool loadFromPath;

        [SerializeField]
        private bool instanceAfterLoading;

        [SerializeField]
        private Object resource;

        [SerializeField]
        private string resourcePath;

        protected override IEnumerator AsyncLoad()
        {
            if(loadFromPath)
            {
                ResourceRequest request = Resources.LoadAsync(resourcePath);
                yield return new WaitUntil(() => request.GetAwaiter().IsCompleted);
                resource = request.asset;
                if(instanceAfterLoading)
                    Instantiate(request.asset);
            }
            else if(instanceAfterLoading)
                Instantiate(resource);
        }

        protected override void SyncLoad()
        {
            if(loadFromPath)
            {
                resource = Resources.Load(resourcePath);
                if(instanceAfterLoading)
                    Instantiate(resource);
            }
            else if(instanceAfterLoading)
                Instantiate(resource);
        }

        public void SpawnInstance(Vector3 position,Vector3 rotation,Transform parent=null)
        {
            Instantiate(resource,position,Quaternion.Euler(rotation),parent);
        }
    }
}
