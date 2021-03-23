using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniScript : MonoBehaviour
{
    Animator ani;
    basicmovement bm;
    [SerializeField] private GameObject player;

    string walk = "Walk";
    string run = "RunAnimation";
    string jump = "Jump";
    string defaultIdle = "IdleAnimation";
    string startCrouch = "Start_Crouch";
    string idleCrouch = "Idle_Crouch";
    string walkCrouch = "Crouch_Walk";

    bool crouch = false;
    bool onGround = false;
    bool running = false;  
    bool jumping = false; 
    bool crouching = false;  
    bool walking = false; 

    int CrouchCounter = 0;
    int FixedCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        bm = player.GetComponent<basicmovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      FixedCounter++;
      onGround = bm.onSurface();
        //Debug.Log(ani.GetCurrentAnimatorStateInfo(0).IsName(jump)); 0=Idle 1=Walk 2=Run 3=StartCruch 4=Jump
        // 5=IdleCruch 6=CruchWalk
        //Debug.Log(ani.runtimeAnimatorController.animationClips[7].name);
       // Debug.Log(ani.runtimeAnimatorController.animationClips[0].length - ani.runtimeAnimatorController.animationClips[0].time);
       
      if (FixedCounter > Mathf.Round(0.05f / Time.fixedDeltaTime) )
      {
            crouch = CrouchCounter % 2 == 0 ? false : true;
            running = Input.GetKey(KeyCode.LeftShift);
            jumping = Input.GetAxis("Jump") != 0;
            crouching = Input.GetKeyDown("v");
            walking = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
            FixedCounter = 0;
      }
        ani.PlayInFixedTime(animationState(), 0);
    }

    string animationState(){

        // non-crouch ani
        if(walking && !running && !jumping && !crouch){
            return walk;

        }else if(walking && running && !jumping){

            if (crouch){
                CrouchCounter++;
            }
            return run;

        }else if (onGround && jumping){

            if(crouch){
                CrouchCounter++;
            }
            return jump;

            //Crounch animation
        }else if(crouching && !walking && !running && !jumping){
            CrouchCounter++;
            return startCrouch;

        }else if(crouch && !walking && !running && !jumping){
            return idleCrouch;

        }else if(crouch && walking && !running && !jumping){
            return walkCrouch;
        }

        return defaultIdle;
    }
}
