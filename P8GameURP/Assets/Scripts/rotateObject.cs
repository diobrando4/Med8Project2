using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    MeshRenderer mr;
    MeshCollider mc;
    public int ECC = 0;
    private bool isTrigged;
    public GameObject MainPuzzleController;
    MainPuzzleController mpc;
    AudioSource audio;
    public float vol = 0.5f;
    public int myID = 0;
    public bool canPlay;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        mpc = MainPuzzleController.GetComponent<MainPuzzleController>();
        mr = GetComponent<MeshRenderer>();
        mc = GetComponent<MeshCollider>();

        audio.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(mpc.isEmergentBool())
        {
            if (!isTrigged) {
                Appear();
                transform.Rotate(Vector3.up * 60 * Time.deltaTime, Space.Self);
            }
        }else{
            Disapear();
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"){
            if (canPlay) {
                isTrigged = true;
                ECC = 1;
                Disapear();
                audio.volume = vol;
                audio.enabled = true;
            }
        }
    }
    
    void Disapear(){
        mr.enabled = false;
        mc.enabled = false;
    }
    void Appear(){
        mr.enabled = true;
        mc.enabled = true;
    }
    public bool IamTriggered(){
        return isTrigged;
    }
}
