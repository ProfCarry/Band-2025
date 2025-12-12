using UnityEngine;
using UnityEngine.Events;

enum PassageDirectionType
{ 
    Up,
    Down,
    Left,
    Right
}

public class LevelPassage : MonoBehaviour
{
    [SerializeField]
    private PassageDirectionType passageDirectionType;

    [SerializeField]
    private string characterTag="MainCharacter";

    [SerializeField]
    private int startRoomId;

    public int StartRoomId {  get { return startRoomId; } }

    [SerializeField]
    private int endRoomId;

    public int EndRoomId { get { return endRoomId; } }

    [SerializeField]
    private UnityEvent<int, int> onPass;

    public UnityEvent<int, int> OnPass { get { return onPass; } }

    private Vector3 startTriggerPosition;
    private Vector3 endTriggerPosition;

    public Vector3 GetPassageDirection()
    {
        switch(passageDirectionType)
        {
            case PassageDirectionType.Up:
                return Vector3.up;
            case PassageDirectionType.Down:
                return Vector3.down;
            case PassageDirectionType.Left:
                return Vector3.left;
            case PassageDirectionType.Right:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }

    public void InvertPassageDirection()
    {
        switch (passageDirectionType)
        {
            case PassageDirectionType.Up:
                passageDirectionType = PassageDirectionType.Down;
                break;
            case PassageDirectionType.Down:
                passageDirectionType = PassageDirectionType.Up;
                break;
            case PassageDirectionType.Left:
                passageDirectionType=PassageDirectionType.Right;
                break;
            case PassageDirectionType.Right:
                passageDirectionType = PassageDirectionType.Left;
                break;
        }
        int tmp = startRoomId;
        startRoomId = endRoomId;
        endRoomId = tmp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(characterTag))
            startTriggerPosition=collision.transform.position;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(characterTag))
            endTriggerPosition = collision.transform.position;
        Vector3 characterDirection = (endTriggerPosition - startTriggerPosition).normalized;
        Vector3 passageDirection=this.GetPassageDirection();
        if(Vector3.Dot(characterDirection,passageDirection)>0)
        {
            OnPass.Invoke(startRoomId,endRoomId);
            InvertPassageDirection();
        }
    }
}
