using UnityEngine;
using UnityEngine.Events;

public class LevelRoom : MonoBehaviour
{
    [SerializeField]
    private int id;

    public int Id { get { return id; } }

    [SerializeField]
    private UnityEvent<int> onEnter;

    public UnityEvent<int> OnEnter { get { return onEnter; } }

    [SerializeField]
    private UnityEvent<int> onExit;

    public UnityEvent<int> OnExit { get { return onExit; } }
}
