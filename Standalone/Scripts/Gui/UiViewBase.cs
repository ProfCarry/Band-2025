using Band.Utils.Execution;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Band.Gui
{

    [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class UiViewBase : MonoBehaviour
    {
        private UiViewBase previous;

        public UiViewBase Previous {  get { return previous; } set { previous = value; } }

        protected UIDocument uiDocument;

        protected VisualElement root;

        [SerializeField]
        protected AudioSource buttonSfx;

        [SerializeField]
        protected string buttonClass;

        [SerializeField]
        protected bool useExecutors;

        protected virtual void Start()
        {
            uiDocument = this.GetComponent<UIDocument>();
            root=uiDocument.rootVisualElement;
            if(!buttonClass.Equals(string.Empty))
            {
                var buttons = root.Query<Button>(className: buttonClass).ToList();
                foreach (var button in buttons)
                    button.clicked += OnButtonClicked;
            }
            if(useExecutors)
            {
                List<Button> buttons = root.Query<Button>().ToList();
                List<Executor> executors = this.GetComponents<Executor>().ToList<Executor>();
                foreach (Executor exe in executors)
                {
                    List<Button> btnSearch=buttons.Where<Button>(btn =>btn.name.Equals(exe.Id)).ToList<Button>();
                    if(btnSearch.Count > 0)
                    {
                        Button button=btnSearch.First();
                        button.clicked += () => exe.Execute();
                    }
                }
            }
            ControlView();
        }

        private void OnButtonClicked()
        {
            buttonSfx.Play();
        }

        public IEnumerator LoadUIDocument(UIDocument document)
        {
            yield return new WaitWhile(() => buttonSfx.isPlaying);
            UIDocument nextDocument=Instantiate(document);
            nextDocument.GetComponent<UiViewBase>().Previous = this;
            this.gameObject.SetActive(false);
        }

        public IEnumerator Dispose()
        {
            yield return new WaitWhile(() => buttonSfx.isPlaying);
            this.gameObject.SetActive(true);
            Destroy(this.gameObject);
        }

        protected Button GetFirstButton()
        {
            return root.Q<Button>();
        }

        public void ControlView()
        {
            Button button=GetFirstButton();
            button.Focus();
        }
    }
}
