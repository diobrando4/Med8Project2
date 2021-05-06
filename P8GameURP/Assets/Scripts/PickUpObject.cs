using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    
    private Puzzle1Controller p1c;
    public GameObject controller;
    private Outline ol;
    private bool onlyOnce=false;
    private void Start()
    {
        p1c = controller.GetComponent<Puzzle1Controller>();
        ol = GetComponent<Outline>();
    }
    void OnTriggerEnter(Collider collider)
    {
        
        
        if (collider.gameObject.tag == "Player" && !onlyOnce)
        {
            p1c.collection = p1c.collection + 1;
            onlyOnce = true;
            transform.position = new Vector3(0, -100, 0);
            
            ol.OutlineMode= (Outline.Mode)1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(transform.position == new Vector3(0, -100, 0)){ onlyOnce = false; }
    }
    void OnTriggerStay(Collider collider)
    {
        /*Debug.Log("abc!!!" + collider.tag);
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("abc!!!" + collider.tag);
            print("Item picked up");
            transform.position = new Vector3(Random.Range(-54, 66), 0, Random.Range(22, 192));
        }*/
    }
}
