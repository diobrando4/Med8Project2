using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class basicmovement : MonoBehaviour
{
    public PlayerAniScript pas;
    private Rigidbody rb;
    [SerializeField] private GameObject GrabPos;

    [Range(0.2f, 5)] public float jumpSpeed = 0.5f;
    [Header("Speed")]
    [Tooltip("start Speed is the start value, max speed is x2 of the start speed, crouch speed is start speed/2")]
    [Range(0.5f, 5)] public float speed = 5;

    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public Rigidbody grabbedObject;

    private CapsuleCollider BodyCollider;

    private Vector3 playerPos;
    private Vector3 downDirection;
    private Vector3 grabDirection;
    private Vector3 raypos;

    private float downDisRange;
    private float downDis = 0.1f;
    private float jump = 0;
    private float vertical=0;
    private float horizontal=0;
    private float startSpeed = 5;
    private float maxSpeed = 10;
    private float crouchSpeed = 1f;
    private float grabY;
    private float startWeight;
    private float weight;

    private bool forward;
    private bool backward;
    private bool crouching;
    public bool grabbing;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        downDirection = Vector3.down;
        startSpeed = speed;
        maxSpeed = startSpeed * 2;
        BodyCollider = GetComponent<CapsuleCollider>();
        downDisRange = BodyCollider.GetComponent<Collider>().GetComponent<Collider>().bounds.extents.y + downDis;
        Debug.LogError(downDisRange);
        crouchSpeed = startSpeed / 2;
        startWeight = rb.mass;
        weight = startWeight;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forward = Input.GetAxis("Vertical") > 0;
        backward = Input.GetAxis("Vertical") < 0;

        raypos = BodyCollider.transform.position;
        raypos.y = raypos.y + 0.3f;// 927692
        crouching = pas.crouch;
        playerPos = rb.position;
        move();
        grab();
    }
    
    void move()
    {
        Vector3 jumping = Vector3.up * rb.velocity.y * jumpSpeed;
        Vector3 j = Vector3.zero;
        vertical = Input.GetAxis("Vertical") * runSpeed();
        
        horizontal = Input.GetAxis("Horizontal") * runSpeed();
        jump = Input.GetAxis("Jump");
        if (!rayHitStop()) {
            if (!rayHit()) {
                // rb.velocity = (transform.right * vertical + transform.forward * horizontal) * runSpeed();
                //rb.velocity = (transform.forward * horizontal + transform.right * vertical) ;
                rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed() * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + (transform.forward * horizontal) * runSpeed() * -1 * Time.fixedDeltaTime);
                //rb.MovePosition(transform.position + (transform.forward * vertical) * Time.deltaTime);
                /*Debug.Log("velocity x = "+rb.velocity.x);
                Debug.Log("velocity y = "+rb.velocity.y);
                Debug.Log("velocity z = "+rb.velocity.z);*/
            } else if (rayHit() && backward)
            {
                rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed() * Time.fixedDeltaTime);

            }else{
                speed--;
            }
            
        }
        else { rb.velocity = Vector3.zero; Debug.LogWarning(hit.collider.name); speed--; }
        // jump if player is on a collider
        if (jump > 0 && onSurface())
        {
            
            rb.velocity = new Vector3(0, jumpSpeed, 0);
        }



    }
    void grab(){
        if (rayHit() && Input.GetKey(KeyCode.E) && hit.collider.attachedRigidbody)
        {
            
            
            
            grabbedObject = hit.collider.gameObject.GetComponent<Rigidbody>();

            if (grabbedObject.mass < rb.mass+1000) {
                
                hit.collider.gameObject.layer = 2;
                grabDirection = transform.position - grabbedObject.transform.position;
                if (onSurface()) {
                    grabY = rb.position.y;
                }
                rb.position = new Vector3(rb.position.x, grabY, rb.position.z);
                Debug.LogWarning("Grabbing");
                grabbing = true;

                
                //  grabbedObject.position= transform.position- grabDirection ;
                /*weight = grabbedObject.mass;
                rb.mass = weight;*/
                // grabbedObject.transform.SetParent(rb.transform);
                /*if (forward) {
                    grabbedObject.AddRelativeForce(grabDirection * (runSpeed()/weight));
                    //grabbedObject.AddForce(Vector3.forward * grabForce(grabbedObject.mass,0.2f));
                }else if(backward){
                    grabbedObject.AddForce(grabDirection * speed*-1);
                }*/
            }
        }

        if (vertical != 0  && grabbing || horizontal != 0 && grabbing)
        {

            grabbedObject.velocity = rb.velocity*-1;
            
            grabbedObject.useGravity = true;

        }
        else if ( grabbing)
        {

            grabbedObject.useGravity = false;
            grabbedObject.transform.position = GrabPos.transform.position;
            Debug.Log("grabbedObject" + grabbedObject.transform.position + " goal position " + GrabPos.transform.position);
        }

        if (grabbedObject!=null && !Input.GetKey(KeyCode.E))
        {
            grabbedObject.transform.parent = null;
            grabbing = false;
            grabbedObject.gameObject.layer = 0;
            Debug.LogWarning("Dropped the object");
            grabbedObject = null;
            rb.mass = startWeight;
        }


    }
    float grabForce(float mass, float accelation){
        float a = 1 / mass;
        float force= mass * a;

        return force;
    }

    public bool rayHit()
    {
        Debug.DrawRay(raypos, Camera.main.transform.forward);
        try
        {
            return Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f);
        }
        catch (Exception e)
        {
            return false;
        }
    }
    public bool rayHitStop()
    {
        Debug.DrawRay(raypos, Camera.main.transform.forward);
        try
        {
            return Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.1f);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public bool onSurface()
    {
        //                      origin point,   direction   maxDis
        return Physics.Raycast(playerPos, downDirection, downDisRange);

    }
    public bool objectInfront()
    {
        //                      origin point,   direction   maxDis
        return Physics.Raycast(playerPos, Vector3.forward, 0.1f);

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
