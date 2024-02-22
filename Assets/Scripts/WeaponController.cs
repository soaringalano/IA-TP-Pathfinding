using UnityEngine;

public class WeaponController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Zombie")) return;

        other.gameObject.GetComponentInParent<ZombieFSM>().m_health -= 10f;
    }
}