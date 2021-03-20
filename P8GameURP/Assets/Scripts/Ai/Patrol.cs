using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // We need this for the NavMesh

public class Patrol : MonoBehaviour
{
    #region Variables

    [Header("Patrol stuff...")]
    [Tooltip("Array indicating patrol route. Gameobject is the target destination")]
    public Transform[] patrolPoints;
    public List<Waypoint> pp;
    // Refer to the array index of patrolPoints
    private int currentPoint = 0;

    [Header("AI stuff...")]
    public Transform player;
    private Rigidbody rb;
    private NavMeshAgent agent;
    public float movementSpeed;
    public float detectionRange;

    [Header("FoV stuff...")]
    [Range(0, 180)] public float maxAngle;
    public float maxRadius;
    private bool playerInRange = false;

    // For counting down between changing states
    private int currentState = 0;

    // I'm experimenting here
    private string behaviour;

    #endregion

    // This is only to visualize the agent's FoV
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        // The reason why we use transform.up is because work with the horizontal axis (green rotation)
        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (playerInRange == true)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        //Gizmos.DrawLine(transform.position, player.position);
        Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }

    // Agent's Field of View (FoV)
    bool inFov(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        // Basically this is used to see if the player is within the agent's FoV
        for (int i = 0; i < count + 1; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == target)
                {

                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    // Just to make sure the y direction is always 0, so that height is not a factor
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            if (hit.transform == target)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        // If none of our checks are true (that the player isn't within agent's FoV) 
        // then it returns as false
        return false;
    }

    bool lookingAtPlayer = false;
    int IncreaseCurrentState(int counter, float timeInSec, int value, bool playerInRange)
    {

        if (counter % Mathf.Round(timeInSec / Time.fixedDeltaTime) == 0 && playerInRange == true)
        {
            value += 1;
            // Debug.Log("reset currentStatev to: " + currentState);
        }
        // currentState = IntRange(currentState, 0, 100); // Does this need to be here?
        return value;
    }

    int DecreaseCurrentState(int counter, float timeInSec, int value, bool playerInRange)
    {
        if (counter % Mathf.Round(timeInSec / Time.fixedDeltaTime) == 0 && playerInRange == false)
        {
            value -= 1;
            Debug.Log("decreasing currentState by: " + currentState);
        }
        //currentState = IntRange(currentState, 0, 100); // Does this need to be here?
        return value;
    }

    // What does this do?
    int IntRange(int value, int min, int max)
    {
        if (value < min)
        {
            value = min;
        }
        else if (value > max)
        {
            value = max;
        }
        return value;
    }


    // Awake is called before Start()
    void Awake()
    {
        // Added these lines to automatically add components in the inspector when the script is activated
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        // Checks if the NavMesh has been added to the agent/enemy
        if (agent == null)
        {
            Debug.LogError("The NavMeshAgent isn't attached to " + gameObject.name);
        }
        // Checks if the Rigidbody has been added to the agent/enemy
        if (rb == null)
        {
            Debug.LogError("The Rigidbody isn't attached to " + gameObject.name);
        }
        // I would like to have an if-statement checking if the player tag was found, but how to do that? :D
    }

    // Start is called before the first frame update
    void Start()
    {

        agent.speed = movementSpeed;

    }

    // Maybe using FixedUpdate is better?
    // Update is called once per frame
    int FixedCounter = 0;
    void FixedUpdate()
    {
        playerInRange = inFov(transform, player, maxAngle, maxRadius);
        Debug.LogWarning("%¤%%¤ " + playerInRange);
        currentState = playerInRange ? IncreaseCurrentState(FixedCounter, 0.2f, currentState, playerInRange) : DecreaseCurrentState(FixedCounter, 0.2f, currentState, playerInRange);

        currentState = IntRange(currentState, 0, 100);
        Debug.LogWarning(currentState);
        
        FixedCounter++;

        if(playerInRange)
        {
            transform.LookAt(player, Vector3.left);
        }


        switch (agentStateIndex(currentState))
        {

            case 0:
                // Debug.LogWarning("Patrol");
                Patrolling();
                break;
            case 1:
                Debug.LogWarning("Alert");
                break;
            case 2:
                Debug.LogWarning("Disengage");
                break;
            case 3:
                Debug.LogWarning("Investigate");
                break;
            case 4:
                Chasing();
                Debug.LogWarning("Chase");
                break;

        }


    }

    void Patrolling()
    {
        //Debug.Log("Distance to current node is " + agent.remainingDistance);

        // Agent goes to next patrol point after reaching its current node
        if (agent.remainingDistance < 0.5f)
        {
            // Destination for the agent
            agent.destination = patrolPoints[currentPoint++].position;
            Debug.Log("currentPoint node: " + currentPoint);
        }
        // Restart the current patrol point back to the first node
        if (currentPoint >= patrolPoints.Length)
        {
            currentPoint = 0;
            Debug.Log("currentPoint reset node to: " + currentPoint);
        }
    }

    void Chasing()
    {
        agent.SetDestination(player.position);
        Debug.Log("Agent is chasing the player");
    }
    bool BoolRange(int value, int min, int max)
    {
        return value >= min && value <= max ? true : false;
    }
    int agentStateIndex(int index)
    {
        if (index < 2)// =0.4 secs
        {
            return 0;
        }
        /* else if (BoolRange(index, 10, 19))
         {
             return 1;
         }
         else if (BoolRange(index, 20, 29))
         {
             return 2;
         }
         else if (BoolRange(index, 30, 39))
         {
             return 3;
         }*/
        else
            return 4;
    }
    void Investigating()
    {
        // doesn't exist yet :D
    }
    bool timer(int counter, float timeInSec)
    {

        if (counter % Mathf.Round(timeInSec / Time.fixedDeltaTime) == 0)
        {
            return true;
        }
        return false;
    }
}
