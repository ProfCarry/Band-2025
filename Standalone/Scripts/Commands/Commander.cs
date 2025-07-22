using Band.Character;
using Band.Utils;
using UnityEngine;

namespace Band.Commands
{
    public abstract class Commander : MonoBehaviour, IController, ICharactable
    {
        [SerializeField]
        protected CharacterBase2D character;

        [SerializeField]
        private bool useAttachedCharacter;

        CharacterBase2D ICharactable.Character { get { return character; } set { character = value; } }

        public abstract void Clear();

        protected virtual void Start()
        {
            if (useAttachedCharacter)
                character = this.transform.GetComponent<CharacterBase2D>();
        }
    }

}