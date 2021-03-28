using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicmovement : MonoBehaviour
{
    public PlayerAniScript pas;
    [SerializeField] private Rigidbody rb;
    [Range(0.2f, 5)] public float jumpSpeed = 0.5f;
    [Header("Speed")]
    [Tooltip("start Speed is the start value, max speed is x2 of the start speed, crouch speed is start speed/2")]
    [Range(0.5f, 5)] public float speed = 5;


    private Vector3 playerPos;
    private Vector3 downDirection;
    private float downDisRange;
    private float downDis = 0.1f;
    private float jump = 0;
    private float vertical=0;
    private float horizontal=0;
    private float startSpeed = 5;
    private float maxSpeed = 10;
    private CapsuleCollider BodyCollider;
    
    private bool crouching;
    private float crouchSpeed = 1f;

   
    // Start is called before the first frame update
    void Start()
    {
        rb = rb.GetComponent<Rigidbody>();
        
        downDirection = Vector3.down;
        startSpeed = speed;
        maxSpeed = startSpeed * 2;
        BodyCollider = GetComponent<CapsuleCollider>();
        downDisRange = BodyCollider.GetComponent<Collider>().GetComponent<Collider>().bounds.extents.y + downDis;
        Debug.LogError(downDisRange);
        crouchSpeed = startSpeed / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        crouching = pas.crouch;
        playerPos = rb.position;
        move();
    }
    
    void move()
    {
        Vector3 jumping = Vector3.up * rb.velocity.y * jumpSpeed;
        Vector3 j = Vector3.zero;
        vertical = Input.GetAxis("Vertical") * runSpeed();
        
        horizontal = Input.GetAxis("Horizontal") * runSpeed();
        jump = Input.GetAxis("Jump");
       
       // rb.velocity = (transform.right * vertical + transform.forward * horizontal) * runSpeed();
        //rb.velocity = (transform.forward * horizontal + transform.right * vertical) ;
         rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed()*Time.fixedDeltaTime);
         rb.MovePosition(rb.position + (transform.forward * horizontal) * runSpeed() * -1* Time.fixedDeltaTime);
        //rb.MovePosition(transform.position + (transform.forward * vertical) * Time.deltaTime);
        /*Debug.Log("velocity x = "+rb.velocity.x);
        Debug.Log("velocity y = "+rb.velocity.y);
        Debug.Log("velocity z = "+rb.velocity.z);*/
        // jump if player is on a collider
        if (jump > 0 && onSurface())
        {
            
            rb.velocity = new Vector3(0, jumpSpeed, 0);
        }


    }
   
    public bool onSurface()
    {
        //                      origin point,   direction   maxDis
        return Physics.Raycast(playerPos, downDirection, downDisRange);

    }
    private float runSpeed(){

        if (!crouching) {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed++;
            } else {
                speed--;
            }
            speed = Mathf.Clamp(speed, startSpeed, maxSpeed);
        }else{
            speed = crouchSpeed;
        }
        return speed;
    }
}
