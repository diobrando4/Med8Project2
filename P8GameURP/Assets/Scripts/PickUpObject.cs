using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("abc!!!" + collider.tag);
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("abc!!!" + collider.tag);
            print("Item picked up");
            transform.position = new Vector3(Random.Range(-54, 66), 0, Random.Range(22, 192));
        }
    }
    void OnTriggerStay(Collider collider)
    {
        Debug.Log("abc!!!" + collider.tag);
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("abc!!!" + collider.tag);
            print("Item picked up");
            transform.position = new Vector3(Random.Range(-54, 66), 0, Random.Range(22, 192));
        }
    }
}
