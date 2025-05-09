using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class AIPhysicsController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        // Ensure Rigidbody settings are correct
        rb.isKinematic = true; // NavMeshAgent controls movement
    }

    void FixedUpdate()
    {
        // Synchronize Rigidbody with NavMeshAgent
        if (navMeshAgent != null)
        {
            rb.MovePosition(navMeshAgent.nextPosition);
        }
    }
}
