using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Band.Utils
{
    public static class UIDocumentExtensions
    {
        public static void Show(this UIDocument ui)
        {
            ui.gameObject.SetActive(true);
        }

        public static void Hide(this UIDocument ui)
        {
            ui.gameObject.SetActive(false);
        }
    }
}
