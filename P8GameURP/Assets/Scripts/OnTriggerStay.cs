using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerStay : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(MyPuzzle && other.tag == "Player")
        {
            isInside = true;
            playerPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            PlayerTimer();
        }
    }
}
