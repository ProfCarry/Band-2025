using Band.Character;
using UnityEngine;

namespace Band.Utils
{
    public interface ICharactable
    {
        public CharacterBase2D Character { get; set; }
    }
}
