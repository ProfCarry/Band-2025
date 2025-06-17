
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Band.Utils.Execution
{
    public class SceneLoader : Loader
    {
        [SerializeField]
        private bool reloadCurrentScene;

        [SerializeField]
        private string scene;

        public string ResourceScene { get { return scene; } set { scene = value; } }

        public override object Load()
        {
            SceneManager.LoadScene(scene);
            return null;
        }

        private void Start()
        {
            if(reloadCurrentScene)
                scene=SceneManager.GetActiveScene().name;
        }
    }
}
