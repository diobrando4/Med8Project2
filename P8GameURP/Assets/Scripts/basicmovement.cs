using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class basicmovement : MonoBehaviour
{
    public PlayerAniScript pas;
    private Rigidbody rb;
    [SerializeField] private GameObject GrabPos;

    [Range(0.2f, 10)] public float jumpSpeed = 0.5f;
    [Header("Speed")]
    [Tooltip("start Speed is the start value, max speed is x2 of the start speed, crouch speed is start speed/2")]
    [Range(0.5f, 5)] public float speed = 5;

    [Header("added jump force to player")]
    [Range(0.0f, 1.5f)] public float jumpForceIncrements = 0.2f;

    [Header("Jump speed incremential time mili sec")]
    [Range(0.0f, 1)] public float jumpIncrements = 0.1f;

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
    private float vertical = 0;
    private float horizontal = 0;
    private float startSpeed = 5;
    private float maxSpeed = 10;
    private float crouchSpeed = 1f;
    private float grabY;
    private float startWeight;
    private float weight;
    private float startJumpSpeed = 0f;
    float rigNormalX;
    float rigNormalZ;

    private bool forward;
    private bool backward;
    private bool crouching;
    private bool flying;
    private bool startJumpCounter;
    [HideInInspector] public bool grabbing;
    [HideInInspector] public bool playJumpAnimation;
    private bool canJump = false;
    private bool fall = false;
    private bool jumpCountTest;
    private bool jumpIsMaxed;
    float velY = 1;

    [Header("max increments depended on jumpincrements see tooltip")]
    [Tooltip("if jumpincrements is 0.1 and maxincrements max jump height will be achived after 1 sec")]
    public int maxIncrements = 10;
    private int increments = 0;
    private int counter;
    private int testCoutner = 0;
    int multiplyierX = 1;
    int multiplyierZ = 1;


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
        startJumpSpeed = jumpSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter++;
        forward = Input.GetAxis("Vertical") > 0;
        backward = Input.GetAxis("Vertical") < 0;

        raypos = BodyCollider.transform.position;
        raypos.y = raypos.y + 0.3f;// 927692
        crouching = pas.crouch;
        playerPos = rb.position;
        move();
        grab();
        jumping(jumpIncrements); Debug.Log("canJump" + canJump);

        //  if (!onSurface() && grabbing&& vertical!=0 && !canJump|| !onSurface() && grabbing && horizontal != 0 && !canJump) { preventFlying(1.5f); };
        test();
        preventFlying();
        if (!flying && !onSurface() && grabbing && vertical != 0 || !flying && !onSurface() && grabbing && horizontal != 0) { };
        // Debug.LogError(counter % Mathf.Round(1.5f / Time.fixedDeltaTime));
        //preventFlying(0.0f);
        Debug.LogError("first part " + (!flying && !onSurface() && grabbing && vertical != 0));
        // Debug.LogError("test =  "+test());
        //Debug.LogError("total  =  "+(!flying && !onSurface() && grabbing && vertical != 0 && test()));

        /*if (!flying && !onSurface() && grabbing && vertical != 0 && test() ||test() && !flying && !onSurface() && grabbing && horizontal != 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, Physics.gravity.y + rb.velocity.y, rb.velocity.z);
        }*/
        if(transform.position.y<0 && !onSurface()){
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);

        }
    }

    void move()
    {
        Vector3 jumping = Vector3.up * rb.velocity.y * jumpSpeed;
        Vector3 j = Vector3.zero;
        vertical = Input.GetAxis("Vertical") * runSpeed();

        horizontal = Input.GetAxis("Horizontal") * runSpeed();
        jump = Input.GetAxis("Jump");
        if (!rayHitStop())
        {
            if (!rayHit())
            {
                // rb.velocity = (transform.right * vertical + transform.forward * horizontal) * runSpeed();
                //rb.velocity = (transform.forward * horizontal + transform.right * vertical) ;
                if(forward || backward) { rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed() * Time.fixedDeltaTime); }
                else if(vertical!=0) { rb.MovePosition(rb.position + (transform.forward * horizontal) * runSpeed() * -1 * Time.fixedDeltaTime); }
               
                //rb.MovePosition(transform.position + (transform.forward * vertical) * Time.deltaTime);
                /*Debug.Log("velocity x = "+rb.velocity.x);
                Debug.Log("velocity y = "+rb.velocity.y);
                Debug.Log("velocity z = "+rb.velocity.z);*/
            }
            else if (rayHit() && !forward)
            {
                if (backward) { rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed() * Time.fixedDeltaTime); }
                else if (vertical != 0) { rb.MovePosition(rb.position + (transform.forward * horizontal) * runSpeed() * -1 * Time.fixedDeltaTime); }


            }
            else if(rayHit() && !forward)
            {
                if (backward) { rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed() * Time.fixedDeltaTime); }
                else if (vertical != 0) { rb.MovePosition(rb.position + (transform.forward * horizontal) * runSpeed() * -1 * Time.fixedDeltaTime); }
                
            }
            else{
                if (speed>0) { speed--; }
            }

        }else if(rayHitStop() && !forward)
        {
            rb.MovePosition(rb.position + (transform.right * vertical) * 3* Time.fixedDeltaTime);

            rb.MovePosition(rb.position + (transform.forward * horizontal) * 1.1f * -1 * Time.fixedDeltaTime);
        }
        else { rb.velocity = Vector3.zero; Debug.LogWarning(hit.collider.name); speed--;  }
        // jump if player is on a collider




    }
    bool degrease = false; int count = 0;
    void preventFlying()
    {

        if (fall && vertical != 0 || horizontal != 0 && fall)
        {
            rb.velocity = new Vector3(rb.velocity.x, Physics.gravity.y + rb.velocity.y, rb.velocity.z);
        }



    }

    void test()
    {
        if (startJumpCounter && !onSurface())
        {
            testCoutner++;
        }
       /* if (testCoutner % Mathf.Round(1f / Time.fixedDeltaTime) == 0 && startJumpCounter && !onSurface() && !grabbing)
        {
            flying = false;
            startJumpCounter = false;
            testCoutner = 0;
            fall = true;



        }*/
        if (jump>0){
            jumpCountTest = true;
        }
        if(jumpCountTest && jump==0){
            if (counter % Mathf.Round(1.5f / Time.fixedDeltaTime) == 0 && startJumpCounter && !onSurface() && grabbing)
            {
                flying = false;
                startJumpCounter = false;
                counter = -10;
                fall = true;
                jumpCountTest = false;
            }
        }/*else{
            if (testCoutner % Mathf.Round(1.5f / Time.fixedDeltaTime) == 0 && startJumpCounter && !onSurface() && grabbing&&!jumpCountTest)
            {
                flying = false;
                startJumpCounter = false;
                testCoutner = 0;
                fall = true;
                
            }
        }*/
        
        if (fall && onSurface())
        {
            fall = false;
        }

    }


    void jumping(float increaseForcePrMiliSec)
    {


        if (jump > 0 && onSurface())
        {
            canJump = true;
            flying = true;
        }

        if (canJump)
        {

            if (counter % Mathf.Round(increaseForcePrMiliSec / Time.fixedDeltaTime) == 0 && jump > 0 && canJump && increments <= maxIncrements && !Input.GetKey(KeyCode.LeftShift))
            {

                jumpSpeed += jumpForceIncrements;
                increments++;

            }
        }
        if (jump == 0 && canJump)
        {
            if(grabbing){
                jumpSpeed = jumpSpeed+1f;
            }
            playJumpAnimation = true;
            rb.velocity = new Vector3(0, jumpSpeed, 0);
            jumpSpeed = startJumpSpeed;
            increments = 0;
            canJump = false;
            counter = 0;
            startJumpCounter = true;

        }





    }



    void grab()
    {
        
        float y =  Physics.gravity.y + rb.velocity.y;
        if ( Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f) && Input.GetKey(KeyCode.E) && hit.collider.attachedRigidbody && grabbedObject==null)
        {



            
            
            grabbedObject = hit.collider.gameObject.GetComponent<Rigidbody>();
            Debug.LogError(hit.normal);
            if (grabbedObject.mass < rb.mass + 110f)
            {
                grabbedObject.freezeRotation = true;
                //hit.collider.gameObject.layer = 2;
                grabDirection = transform.position - grabbedObject.transform.position;
                if (onSurface())
                {
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
        
        if (vertical != 0 && grabbing || horizontal != 0 && grabbing)
        {

            multiplyierX = hit.normal.x > 0 ? 1 : -1;
            multiplyierZ = hit.normal.z > 0 ? 1 : -1;
            if(!flying)
                rb.velocity = new Vector3(rb.velocity.x*multiplyierX, y, rb.velocity.z*multiplyierZ);
           // jumping(jumpIncrements);
            grabbedObject.velocity = rb.velocity;
           // grabbedObject.MovePosition(GrabPos.transform.position - grabbedObject.transform.position);
           // y = grabbedObject.position.y;
           
            //grabbedObject.transform.position = Vector3.Lerp(GrabPos.transform.position, grabbedObject.transform.position, Time.fixedDeltaTime * 100);
            //grabbedObject.transform.position = GrabPos.transform.position;
        }
         if (grabbing)
        {
            multiplyierX = hit.normal.x > 0 ? 1 : -1;
            multiplyierZ = hit.normal.z > 0 ? 1 : -1;
            if (!flying)
                rb.velocity = new Vector3(rb.velocity.x * multiplyierX, y, rb.velocity.z * multiplyierZ);
            if (hit.normal.x > 0) { hit.normal.Equals(0); }
            rb.useGravity = true;
            grabbedObject.velocity = Vector3.zero;
            //jumping(jumpIncrements);
            grabbedObject.useGravity = false;
            
            // grabbedObject.MovePosition(GrabPos.transform.position - grabbedObject.transform.position);
            //grabbedObject.transform.position = GrabPos.transform.position - grabbedObject.transform.position;
            //grabbedObject.velocity = rb.velocity * -1;

            // grabbedObject.MovePosition(GrabPos.transform.position);
            //Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            //  grabbedObject.transform.position = GrabPos.transform.position;
            grabbedObject.transform.position = GrabPos.transform.position;
           // grabbedObject.transform.position = Vector3.Lerp(GrabPos.transform.position, grabbedObject.transform.position, Time.fixedDeltaTime*100);
            Debug.Log("grabbedObject" + grabbedObject.transform.position + " goal position " + GrabPos.transform.position);
        }

        if (grabbedObject != null && !Input.GetKey(KeyCode.E))
        {
            grabbedObject.useGravity = true;
            //grabbedObject.transform.parent = null;
            grabbedObject.freezeRotation = false;
            grabbing = false;
           // grabbedObject.gameObject.layer = 0;
            Debug.LogWarning("Dropped the object");
            grabbedObject = null;
            rb.mass = startWeight;
        }


    }
    float grabForce(float mass, float accelation)
    {
        float a = 1 / mass;
        float force = mass * a;

        return force;
    }

    public bool rayHit()
    {
        Debug.DrawRay(raypos, Camera.main.transform.forward);
        try
        {
            if (Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f) && hit.collider.attachedRigidbody)
            {
                return false;
            }
            else
            {
                return Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f);
            }
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
            if (Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f) && hit.collider.attachedRigidbody)
            {
                return false;
            }
            else
            {
                return Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f);
            }
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
    private float runSpeed()
    {

        if (!crouching)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !grabbing)
            {
                speed++;
            }
            else
            {
                speed--;
            }
            if(grabbing ){
                speed =  startSpeed+0.25f;
            }
            
        }
        else
        {
            speed = crouchSpeed;
        }
        speed = Mathf.Clamp(speed, startSpeed, maxSpeed);
        return speed;
    }
}
