using UnityEngine;

public class DisableRigidbodyDuringAnimation : MonoBehaviour
{
    public Rigidbody rb; // Reference to the Rigidbody
    public Animator animator; // Reference to the Animator
    private bool isWakingUp = false;

    void Start()
    {
        // Ensure Rigidbody is assigned
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the waking-up animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
        {
            if (!isWakingUp)
            {
                // Disable the Rigidbody when the animation starts
                rb.isKinematic = true;
                isWakingUp = true;
            }
        }
        else
        {
            if (isWakingUp)
            {
                // Re-enable Rigidbody after animation ends
                rb.isKinematic = false;
                isWakingUp = false;
            }
        }
    }
}