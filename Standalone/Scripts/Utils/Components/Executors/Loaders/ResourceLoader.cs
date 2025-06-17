using UnityEngine;

namespace Band.Utils.Execution
{
    public class ResourceLoader : Loader
    {
        [SerializeField]
        private GameObject resource;

        public GameObject Resource { get { return resource; } set { resource = value; } }

        public override object Load()
        {
            GameObject resourceLoader = GameObject.Instantiate<GameObject>(resource);
            return resourceLoader;
        }
    }
}
