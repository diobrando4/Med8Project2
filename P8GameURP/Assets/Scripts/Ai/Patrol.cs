using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // We need this for the NavMesh

public class Patrol : MonoBehaviour
{
    #region Variables

    [Header("Patrol points")]
    [Tooltip("Array indicating patrol route. Gameobject is the target destination")]
    public Transform[] patrolPoints;
    // Refer to the array index of patrolPoints
    private int currentPoint = 0;

    [Header("Stuff...")]
    public Transform player;
    public Rigidbody rb;
    public NavMeshAgent agent;
    public float movementSpeed;

    #endregion

    // Awake is called before Start()
    void Awake()
    {
        // Added these lines to automatically get added in the inspector when the script is activated
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
        // I would like to have an if-statement checking if the player tag was found, but how do I do that? :D
    }

    // Start is called before the first frame update
    void Start()
    {
        // Changes the agent's speed to whatever this script sets it to
        // Made an error when moved to Awake()
        agent.speed = movementSpeed;
        // To see if the value was infinity or not
        //Debug.Log(agent.remainingDistance);
        // To see if the first node is set at 0
        //Debug.Log("currentPoint starts at node: " + currentPoint);
    }

    // Update is called once per frame
    void Update()
    {
        Patrolling();
    }

    void Patrolling()
    {
        // 
        if (agent.remainingDistance < 0.5f)
        {
            agent.destination = patrolPoints[currentPoint++].position;
            //Debug.Log("currentPoint goes to node: " + currentPoint);
        }
        // Restart the current patrol point back to the first node
        if (currentPoint >= patrolPoints.Length)
        {
            currentPoint = 0;
            //Debug.Log("currentPoint reset to node: " + currentPoint);
        }
    }
}
