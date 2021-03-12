using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicmovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public float speed = 5;
    public float jumpSpeed = 0.5f;

    private Vector3 playerPos;
    private Vector3 downDirection;
   private float downDisRange;
    public float downDis = 0.2f;
    private float jump = 0;
    private float vertical=0;
    private float horizontal=0;
    private float startSpeed = 5;
    private float maxSpeed = 10;
    [SerializeField] private Collider BodyCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = rb.GetComponent<Rigidbody>();
        downDisRange = BodyCollider.GetComponent<Collider>().GetComponent<Collider>().bounds.extents.y + downDis;
        downDirection = Vector3.down;
        startSpeed = speed;
        maxSpeed = startSpeed * 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = rb.position;
        move();
    }

    void move()
    {

        vertical = Input.GetAxis("Vertical") * runSpeed();
        
        horizontal = Input.GetAxis("Horizontal") * runSpeed();
        jump = Input.GetAxis("Jump");
       
        rb.MovePosition(rb.position + (transform.forward * vertical) * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + (transform.right * horizontal) * Time.fixedDeltaTime);

        // jump if player is on a collider
       
        if (jump > 0 && onSurface())
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }

    }

    public bool onSurface()
    {
        //                      origin point,   direction   maxDis
        return Physics.Raycast(playerPos, downDirection, downDisRange);

    }
    private float runSpeed(){

        
        if(Input.GetKey(KeyCode.LeftShift))
        {           
            speed++;          
        }else{
            speed--;
        }
        speed = Mathf.Clamp(speed, startSpeed, maxSpeed);
        return speed;
    }
}
