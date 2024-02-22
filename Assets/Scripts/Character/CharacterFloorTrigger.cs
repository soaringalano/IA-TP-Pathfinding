using UnityEngine;

public class CharacterFloorTrigger : MonoBehaviour
{

    private void Awake()
    {
    }

    public bool IsOnFloor { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        if (!IsOnFloor)
        {
        }
        IsOnFloor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsOnFloor = false;
    }
}
