using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerAniScript : MonoBehaviour
{
    Animator ani;
    basicmovement bm;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerlead;

    // needs to be shared to player movement script
    public CapsuleCollider runnerbody;
    public SphereCollider walkbody;
    //private Collider CurrentCollider;

    private AnimatorStateInfo animationInfo;
    private AnimatorClipInfo[] animationClip;

    string walk = "Walk";
    string run = "RunAnimation";
    string jump = "Jump";
    string defaultIdle = "IdleAnimation";
    string startCrouch = "Start_Crouch";
    string idleCrouch = "Idle_Crouch";
    string walkCrouch = "Crouch_Walk";
    string currentstate;

    private bool crouch = false;
    private bool onGround = false;
    private bool running = false;
    private bool jumping = false;
    private bool crouching = false;
    private bool walking = false;
    private bool IsWalking = false;
    private bool criticalAniDone = true;


    private float timedelay = 0.1f;
    private float AnimationTimeLength = 0;
    private const float JumpTime = 0.717f;
    private const float startCrouchTime = 0.264f;

    private int CrouchCounter = 0;
    private int FixedCounter = 0;
    private int JumpCounter = 0;
    private int CrouchStartCounter = 0;
    // Start is called before the first frame update
    void Awake()
    {
        ani = GetComponent<Animator>();
        bm = player.GetComponent<basicmovement>();
        runnerbody = runnerbody.GetComponent<CapsuleCollider>();
        walkbody = walkbody.GetComponent<SphereCollider>();
        currentstate = defaultIdle;

    }

    void FixedUpdate()
    {
        transform.position = playerlead.transform.position;
        ani.SetFloat("Vertical", Input.GetAxis("Vertical"));
        ani.SetFloat("Horizontal", Input.GetAxis("Vertical"));
        FixedCounter++;
        onGround = bm.onSurface();
        criticalAniDone = currentstate == idleCrouch || currentstate == defaultIdle || currentstate == walk || currentstate == walkCrouch || currentstate == run;
        // following try catch is to ensure that animations suchs as jumping completes before the next animation
        try
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName(jump) || ani.GetCurrentAnimatorStateInfo(0).IsName(startCrouch))
            {
                animationInfo = ani.GetCurrentAnimatorStateInfo(0);
                animationClip = ani.GetCurrentAnimatorClipInfo(0);

                AnimationTimeLength = animationClip[0].clip.length * animationInfo.normalizedTime;

                if (AnimationTimeLength >= JumpTime && currentstate == jump)
                {
                    currentstate = defaultIdle;
                    AnimationTimeLength = 0;
                    JumpCounter = 0;

                }
                else if (AnimationTimeLength >= startCrouchTime && currentstate == startCrouch)
                {

                    currentstate = idleCrouch;
                    AnimationTimeLength = 0;

                }
                else { currentstate = currentstate; }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("exception " + e);

            if (currentstate == jump)
            {
                currentstate = defaultIdle;
                JumpCounter = 0;

            }
            else if (currentstate == startCrouch)
            {
                currentstate = idleCrouch;
            }
        }

        running = Input.GetKey(KeyCode.LeftShift);
        crouching = Input.GetKey(KeyCode.LeftControl);
        jumping = Input.GetAxis("Jump") != 0;
        walking = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;

        AnimationController();

        crouch = CrouchCounter % 2 == 0 ? false : true;

        walkbody.enabled = IsWalking;
        runnerbody.enabled = !IsWalking;
        //ani.PlayInFixedTime(currentstate, 0);
        ani.Play(currentstate,0);
    }

    public bool GetCurrentCollider()
    {
        return IsWalking ? true : false;
    }
    void AnimationController()
    {

        // following if statement changes animation states.
        if (FixedCounter % Mathf.Round(timedelay / Time.fixedDeltaTime) == 0 && Input.anyKey && criticalAniDone)
        {

            currentstate = animationState();
            FixedCounter = 0;

        }
        //Following if else if, is to prevent jump/cruch animaion locks.
        if (!Input.anyKey && crouch && criticalAniDone)
        {

            timedelay = 0.1f;
            currentstate = idleCrouch;
            FixedCounter = 0;

        }
        else if (!Input.anyKey && !crouch && criticalAniDone)
        {

            timedelay = 0.1f;
            currentstate = defaultIdle;
            FixedCounter = 0;
        }
    }

    string animationState()
    {

        // non-crouch ani
        if (walking && !running && !jumping && !crouch && !crouching)
        {
            IsWalking = true;
            timedelay = 0.1f;
            return walk;

        }
        else if (walking && running && !jumping && !crouching)
        {
            IsWalking = false;
            if (crouch)
            {
                CrouchCounter++;
            }
            timedelay = 0.1f;
            return run;

        }
        else if (onGround && jumping && !crouching)
        {
            IsWalking = false;
            if (crouch)
            {
                CrouchCounter++;
            }
            // this might end up be redundant
            JumpCounter++;
            timedelay = 0.4f;
            return jump;

            //Crounch animation
        }
        else if (crouching && !crouch)
        {

            CrouchCounter++;
            timedelay = 0.3f;
            return startCrouch;

        }
        else if (crouch && !walking && !running && !jumping)
        {
            timedelay = 0.1f;
            return idleCrouch;

        }
        else if (crouch && walking && !running && !jumping)
        {
            timedelay = 0.1f;
            return walkCrouch;
        }
        else
        {
            timedelay = 0.1f;
            IsWalking = false;
            return defaultIdle;
        }
    }
}
