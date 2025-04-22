using UnityEngine;

public class PhoneTrigger : MonoBehaviour
{
    public FirstPersonController playerController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.SetNearPhone(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.SetNearPhone(false);
        }
    }
}