using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPump : MonoBehaviour
{
    public bool ispumping;
    private Outline ol;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        ol = GetComponent<Outline>();
        y = transform.position.y - 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.tag == "pump"){
            ispumping = true;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            ol.OutlineColor = new Vector4(27, 255,0,255);
        }else{
            ispumping = false;
        }
    }
}
