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
    // ".Count" doesn't seem to work with array, but it does with list
    public List<Waypoint> pp;
    // Refer to the array index of patrolPoints
    private int currentPoint = 0;

    [Header("AI stuff...")]
    public Transform player;
    public Rigidbody rb;
    public NavMeshAgent agent;
    public float movementSpeed;
    public float detectionRange;

    [Header("FoV stuff...")]
    [Range(0,180)] public float maxAngle;
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

    
    int IncreaseCurrentState(int counter, float timeInSec, int value, bool playerInRange)
    {
        if (counter % Mathf.Round(timeInSec / Time.fixedDeltaTime) == 0 && playerInRange == false)
        {
            currentState = 100;
            Debug.Log("reset currentStatev to: " + currentState);
        }
        currentState = IntRange(currentState, 0, 100); // Does this need to be here?
        return currentState;
    }
    
    int DecreaseCurrentState(int counter, float timeInSec, int value, bool playerInRange)
    {
        if (counter % Mathf.Round(timeInSec / Time.fixedDeltaTime) == 0 && playerInRange == false)
        {
            currentState--;
            Debug.Log("decreasing currentState by: " + currentState);
        }
        currentState = IntRange(currentState, 0, 100); // Does this need to be here?
        return currentState;
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
            value =  max;
        }
		return value;
    }


    // Awake is called before Start()
    void Awake()
    {
        // Added these lines to automatically add components in the inspector when the script is activated
        player = GameObject.Find("Player").transform;
        rb = this.GetComponent<Rigidbody>();
        agent = this.GetComponent<NavMeshAgent>();

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
        // Changes the agent's speed to whatever this script sets it to
        // Made an error when moved to Awake()
        agent.speed = movementSpeed;
        // To see if the value is infinity or not
        //Debug.Log(agent.remainingDistance);
        // To see if the first node is set at 0 or not
        //Debug.Log("currentPoint starts at node: " + currentPoint);

        // Agent needs to have at least 2 nodes before patrolling starts
        /*
        if (2 > pp.Count)
        {
            behaviour = "patrol";
        }
        else
        {
            behaviour = "idle";
        }
        */
    }

    // Maybe using FixedUpdate is better?
    // Update is called once per frame
    void Update()
    {
        /*
        switch (behaviour)
        {
            case "idle":
                Debug.Log("Agent is idling");
            break;
            case "patrol":
                Debug.Log("Agent is patrolling");
            break;
            case "chase":
                Debug.Log("Agent is chasing");
            break;
        }
        */

        // The result of inFov is applied for this var
        playerInRange = inFov(transform, player, maxAngle, maxRadius);
        
        if (playerInRange == true)
        {
            Chasing();
        }
        // fyi at some point i could get this to work with list.count
        // but for whatever reason i can't get array.length to work
        if (2 > patrolPoints.Length && playerInRange == false) // this gives me an error
        {
            Patrolling();
        }
        else (playerInRange == false)
        {
            Debug.Log("Agent is idle");
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

    void Investigating()
    {
        // doesn't exist yet :D
    }
}
