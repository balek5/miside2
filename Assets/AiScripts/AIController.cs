using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform player; // Assign the player transform in the Inspector
    public float followDistance = 5f; // Distance to maintain from the player
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > followDistance)
        {
            // Move towards the player
            agent.SetDestination(player.position);
        }
        else
        {
            // Stop moving when close enough
            agent.ResetPath();
        }
    }
}
