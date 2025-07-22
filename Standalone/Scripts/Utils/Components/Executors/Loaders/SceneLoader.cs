
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Band.Utils.Loading
{
    public class SceneLoader : Loader
    {
        [SerializeField]
        private bool reloadCurrentScene;

        [SerializeField]
        private string scene;

        [SerializeField]
        private LoadSceneMode loadMode;

        public string ResourceScene { get { return scene; } set { scene = value; } }

        protected override IEnumerator AsyncLoad()
        {
            AsyncOperation operation=SceneManager.LoadSceneAsync(ResourceScene, loadMode);
            yield return new WaitUntil(()=>operation.isDone);

        }

        protected override void SyncLoad()
        {
            SceneManager.LoadScene(ResourceScene, loadMode);
        }

        private void Start()
        {
            if(reloadCurrentScene)
                scene=SceneManager.GetActiveScene().name;
        }
    }
}
