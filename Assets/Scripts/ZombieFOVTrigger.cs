using UnityEngine;
using UnityEngine.UIElements;

public class ZombieFOVTrigger : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        if (transform.name == "OuterFOV" || transform.name == "FrontFOV")
        {
            //Debug.Log("Trigger detected with the 'outerCollider'.");
            GetComponentInParent<ZombieFSM>().m_isPreyInSight = true;
            GetComponentInParent<ZombieFSM>().m_preyPosition = other.transform.position;
        }

        else if (transform.name == "InnerFOV")
        {
            //Debug.Log("Trigger detected with the 'secondCollider'.");
            GetComponentInParent<ZombieFSM>().m_isPreyInReach = true;
            GetComponentInParent<ZombieFSM>().m_preyPosition = other.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        // Check if the outer collider exits the trigger
        if (transform.name == "OuterFOV" || transform.name == "FrontFOV")
        {
            Debug.Log("Far FOV exited.");
            GetComponentInParent<ZombieFSM>().m_isPreyInSight = false;
        }

        // Check if the inner collider exits the trigger
        if (transform.name == "InnerFOV")
        {
            Debug.Log("Near FOV exited.");
            GetComponentInParent<ZombieFSM>().m_isPreyInReach = false;
        }
    }
}
