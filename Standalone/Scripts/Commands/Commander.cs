using Band.Platform2D.Character;
using Band.Utils;
using UnityEngine;

namespace Band.Commands
{
    public abstract class Commander : MonoBehaviour, IController, ICharactable
    {
        [SerializeField]
        protected CharacterBase character;

        [SerializeField]
        private bool useAttachedCharacter;

        CharacterBase ICharactable.Character { get { return character; } set { character = value; } }

        public abstract void Clear();

        protected virtual void Start()
        {
            if (useAttachedCharacter)
                character = this.transform.GetComponent<CharacterBase>();
        }
    }

}