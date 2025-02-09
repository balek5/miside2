using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class AICONTROLLER : MonoBehaviour
{
    public Transform player; // The player's transform to follow
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public float walkSpeedThreshold = 0.8f; // Speed threshold for walking
    public float runSpeedThreshold = 1.1f;  // Speed threshold for running

    // Normal and initial stopping distances
    public float normalMinimumDistance = 2.0f;       // Normal minimum distance to keep from the player
    public float initialMinimumDistance = 5.0f;      // Initial larger distance from the player
    public float initialDelay = 6.5f;                  // Time in seconds to keep the initial distance

    public float fieldOfViewAngle = 120f; // Field of view angle
    public float viewDistance = 10f;      // Maximum distance to see the player
    public LayerMask obstructionMask;     // Layer mask for obstacles

    public float patrolPointRecordInterval = 2f; // Time interval to record player's position
    public int maxPatrolPoints = 5;              // Maximum number of patrol points to remember

    private List<Vector3> patrolPoints = new List<Vector3>(); // List of patrol points
    private bool playerInSight;
    private float timeOutOfSight = 0f;
    public float sightLostDelay = 1f; // Time before AI switches to patrol mode

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            Debug.LogError("Player transform is not assigned! Assign the Player in the Inspector.");
        }

        // Set the stopping distance to the initial larger distance
        navMeshAgent.stoppingDistance = initialMinimumDistance;
        navMeshAgent.angularSpeed = 120f;
        navMeshAgent.acceleration = 8f;

        // Start recording patrol points
        StartCoroutine(RecordPatrolPoints());

        // Start the coroutine to reset the distance after the initial delay
        StartCoroutine(ResetStoppingDistanceAfterDelay());
    }

    private IEnumerator ResetStoppingDistanceAfterDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        navMeshAgent.stoppingDistance = normalMinimumDistance;
    }

    private void OnDrawGizmos()
    {
        if (navMeshAgent != null && navMeshAgent.hasPath)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < navMeshAgent.path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
            }
        }
    }

    private void Update()
    {
        if (player != null)
        {
            playerInSight = IsPlayerInSight();

            if (playerInSight)
            {
                Debug.Log("Player in sight, following...");
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer > navMeshAgent.stoppingDistance)
                {
                    navMeshAgent.SetDestination(player.position);
                }
                else
                {
                    navMeshAgent.ResetPath();
                }
            }
            else
            {
                Debug.Log("Player out of sight, patrolling...");
                Patrol();
            }

            // Update speed and animation based on movement
            float speed = navMeshAgent.velocity.magnitude;
            animator.SetFloat("Speed", speed);

            if (speed >= runSpeedThreshold)
            {
                animator.SetInteger("Locomotion", 2); // Run animation
            }
            else if (speed >= walkSpeedThreshold)
            {
                animator.SetInteger("Locomotion", 1); // Walk animation
            }
            else
            {
                animator.SetInteger("Locomotion", 0); // Idle animation
            }
        }
    }

    private bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= viewDistance)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, obstructionMask))
                {
                    timeOutOfSight = 0f; // Reset the timer when the player is visible
                    return true;
                }
            }
        }

        // Increment the timer if the player is not visible
        timeOutOfSight += Time.deltaTime;
        return timeOutOfSight < sightLostDelay;
    }

    private void Patrol()
    {
        if (patrolPoints.Count > 0)
        {
            // Move to the first patrol point
            if (!navMeshAgent.hasPath)
            {
                navMeshAgent.SetDestination(patrolPoints[0]);
            }

            // Check if the AI has reached the patrol point
            if (Vector3.Distance(transform.position, patrolPoints[0]) < navMeshAgent.stoppingDistance)
            {
                // Remove the reached patrol point
                patrolPoints.RemoveAt(0);
            }
        }
    }

    private IEnumerator RecordPatrolPoints()
    {
        while (true)
        {
            if (playerInSight)
            {
                // Ensure the new patrol point is far enough from the last one
                if (patrolPoints.Count == 0 || Vector3.Distance(patrolPoints[patrolPoints.Count - 1], player.position) > 2f)
                {
                    patrolPoints.Add(player.position);

                    // Limit the number of patrol points
                    if (patrolPoints.Count > maxPatrolPoints)
                    {
                        patrolPoints.RemoveAt(0); // Remove the oldest point
                    }
                }
            }
            yield return new WaitForSeconds(patrolPointRecordInterval);
        }
    }
}
