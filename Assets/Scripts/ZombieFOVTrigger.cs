using UnityEngine;

public class ZombieFOVTrigger : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("test"))
        {
            Debug.Log("Trigger detected with an object on the 'test' layer.");

            if (other == other.GetComponent<ZombieFSM>().outerCollider)
            {
                Debug.Log("Trigger detected with the 'outerCollider'.");
                other.GetComponent<ZombieFSM>().m_isPreyInSight = true;

            }
            
            else if (other == other.GetComponent<ZombieFSM>().innerCollider)
            {
                Debug.Log("Trigger detected with the 'secondCollider'.");
                other.GetComponent<ZombieFSM>().m_isPreyInReach = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the outer collider exits the trigger
        if (other == other.GetComponent<ZombieFSM>().outerCollider)
        {
            Debug.Log("First collider exited.");
            other.GetComponent<ZombieFSM>().m_isPreyInSight = false;
        }

        // Check if the inner collider exits the trigger
        if (other == other.GetComponent<ZombieFSM>().innerCollider)
        {
            Debug.Log("Second collider exited.");
            other.GetComponent<ZombieFSM>().m_isPreyInReach = false;
        }
    }
}
