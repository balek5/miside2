using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AICharacterFollow : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Inspector
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public float walkSpeedThreshold = 0.8f;  // Speed threshold for walking
    public float runSpeedThreshold = 1.1f;   // Speed threshold for running
    public float minimumDistance = 2.0f;     // Minimum distance to keep from the player
    public float distanceCheckInterval = 0.5f; // Interval to check distance (seconds)
    
    public List<Transform> patrolPoints; // List of patrol points
    private int currentPatrolIndex = 0; // Current patrol point index
    private bool isPatrolling = true; // Flag to check if the AI is patrolling

    private List<Vector3> playerPath = new List<Vector3>(); // List to store the player's path
    private float timeSinceLastCheck = 0f; // Timer for checking distance

    private float stopTime = 0f; // Timer to stop AI for a random time
    private float maxStopTime = 3f; // Maximum time to stop (in seconds)
    private bool isStopped = false; // Flag to check if AI is stopped

    private float followDistanceBuffer = 1.5f; // Buffer distance to maintain from the player

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Set the NavMeshAgent speed (this can be adjusted based on your game)
        navMeshAgent.speed = 3.0f; // Set the walking speed (this can be adjusted)
        navMeshAgent.stoppingDistance = minimumDistance + followDistanceBuffer; // Set stopping distance to maintain buffer
    }

    void Update()
    {
        timeSinceLastCheck += Time.deltaTime;

        if (timeSinceLastCheck >= distanceCheckInterval)
        {
            timeSinceLastCheck = 0f;

            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // If the AI is too close to the player, stop and maintain distance
                if (distanceToPlayer < minimumDistance + followDistanceBuffer)
                {
                    isPatrolling = false; // Stop patrolling
                    MaintainDistance();
                }
                else
                {
                    if (!isPatrolling)
                    {
                        isPatrolling = true; // Resume patrolling
                    }
                    Patrol();
                }
            }
        }

        // If the AI is stopped, count down the stop time
        if (isStopped)
        {
            stopTime -= Time.deltaTime;
            if (stopTime <= 0f)
            {
                isStopped = false; // Resume movement after stopping
            }
        }
    }

    void Patrol()
    {
        // If AI is stopped, do not move
        if (isStopped)
        {
            animator.SetFloat("Speed", 0); // Set to idle
            return;
        }

        // Move to the current patrol point
        if (patrolPoints.Count > 0)
        {
            navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            // Check if the AI has reached the current patrol point
            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 1.0f)
            {
                // Move to the next patrol point
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            }
        }
    }

    void MaintainDistance()
    {
        // Calculate the distance between AI and player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the AI is too close, stop moving and maintain a safe distance
        if (distanceToPlayer < minimumDistance + followDistanceBuffer)
        {
            // Move back to maintain the buffer distance
            Vector3 directionToPlayer = transform.position - player.position;
            Vector3 newPosition = transform.position + directionToPlayer.normalized * followDistanceBuffer;

            navMeshAgent.SetDestination(newPosition);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }

        // Update animation based on speed
        float speed = navMeshAgent.velocity.magnitude;
        if (speed >= runSpeedThreshold)
        {
            animator.SetInteger("Locomotion", 2); // Run
        }
        else if (speed >= walkSpeedThreshold)
        {
            animator.SetInteger("Locomotion", 1); // Walk
        }
        else
        {
            animator.SetInteger("Locomotion", 0); // Idle
        }
    }

    void FollowPlayer()
    {
        // If AI is stopped, do not move
        if (isStopped)
        {
            animator.SetFloat("Speed", 0); // Set to idle
            return;
        }

        // Store the player's last position in the path
        if (playerPath.Count == 0 || Vector3.Distance(player.position, playerPath[playerPath.Count - 1]) > 0.5f)
        {
            playerPath.Add(player.position);
        }

        // Follow the last steps the player took
        if (playerPath.Count > 0)
        {
            navMeshAgent.SetDestination(playerPath[playerPath.Count - 1]);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

            // If AI reaches the last step, remove it from the path
            if (Vector3.Distance(transform.position, playerPath[playerPath.Count - 1]) < 1.0f)
            {
                playerPath.RemoveAt(playerPath.Count - 1);
            }
        }

        // Update animation based on speed
        float speed = navMeshAgent.velocity.magnitude;
        if (speed >= runSpeedThreshold)
        {
            animator.SetInteger("Locomotion", 2); // Run
        }
        else if (speed >= walkSpeedThreshold)
        {
            animator.SetInteger("Locomotion", 1); // Walk
        }
        else
        {
            animator.SetInteger("Locomotion", 0); // Idle
        }

        // Randomly stop the AI for a brief period during the chase
        if (Random.Range(0f, 1f) < 0.05f && !isStopped) // 5% chance to stop each frame
        {
            isStopped = true;
            stopTime = Random.Range(1f, maxStopTime); // Random stop time between 1 and maxStopTime
        }
    }
}
