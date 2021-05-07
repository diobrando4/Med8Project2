using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // We need this for the NavMesh
using UnityEngine.SceneManagement; // We need this for the scene manager

public class AiCar : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agentCar;

    // Awake is called before Start()
    void Awake()
    {
        // Added these lines to automatically add components in the inspector when the script is activated
        rb = GetComponent<Rigidbody>();
        agentCar = GetComponent<NavMeshAgent>();

        // Checks if the NavMesh has been added to the agent/enemy
        if (agentCar == null)
        {
            Debug.LogError("The NavMeshAgent isn't attached to " + gameObject.name);
        }
        // Checks if the Rigidbody has been added to the agent/enemy
        if (rb == null)
        {
            Debug.LogError("The Rigidbody isn't attached to " + gameObject.name);
        }
    }

    // everytime you work with physics you want to use fixed update instead of update!
    void FixedUpdate()
    {
        Driving();
    }

    public Transform[] patrolPoints;
    private int currentPoint = 0;
    
    void Driving()
    {
        //Debug.Log("Distance to current node is " + agent.remainingDistance);

        // Agent goes to next patrol point after reaching its current node
        if (agentCar.remainingDistance < 0.5f)
        {
            // Destination for the agent
            agentCar.destination = patrolPoints[currentPoint++].position;
            //Debug.Log("currentPoint node: " + currentPoint);
        }
        // Restart the current patrol point back to the first node
        if (currentPoint >= patrolPoints.Length)
        {
            currentPoint = 0;
            //Debug.Log("currentPoint node reset to: " + currentPoint);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            //Debug.Log("YOU DIED");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}