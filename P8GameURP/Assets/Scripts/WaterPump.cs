using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPump : MonoBehaviour
{
    public bool ispumping;
    private Outline ol;
    float y;
    float starty;
    // Start is called before the first frame update
    void Start()
    {
        ol = GetComponent<Outline>();
        y = transform.position.y - 0.1f;
        starty = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "pump")
        {
            ispumping = true;
           
            
        }
       
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "pump")
        {
            ispumping = false;


        }
    }
}
