using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
public class PuzzleTimer : MonoBehaviour
{
    public List<Vector3> playerPositionList;
    private Vector3 playerPosition;

    public GameObject MainPuzlleControllerObject;
    private MainPuzzleController mpc;

    public GameObject writeJsonObj;
    private WriteJson writeJason;

    public int basketCounter = 0;
    public bool WaterPumpBool = false;
    public int PlayerTimeCounter = 0;
    private int fixedCounter = 0;
    public int selfID;
   // PuzzleTimer myTimer;
    public string MyName;
    private string selfIDString = "";

    public bool MyPuzzle = false;
    public bool NextPuzzle = false;
    private bool isInside = false;
    private bool isEmergent = false;
    private bool writeOnce = false;
    private MethodInfo MyPuzzleInfo;
    private MethodInfo NextPuzzleInfo;

    public int ECC_1 = 0;
    public int ECC_2 = 0;
    public GameObject eventCapsule1;
    public GameObject eventCapsule2;
    private rotateObject eventCap1;
    private rotateObject eventCap2;
    private bool hasPlayed = false;
    private bool GetValuesOnce = false;
    private int nextID;
    private bool checkEventOnce1 = false;
    private bool checkEventOnce2 = false;

    AudioSource audioSource;
    public AudioClip emergentStart;
    //public AudioClip emergentSolution;

    void Start(){
        audioSource = GetComponent<AudioSource>();
        eventCap1 = eventCapsule1.GetComponent<rotateObject>();
        eventCap2 = eventCapsule2.GetComponent<rotateObject>();
       // myTimer = new PuzzleTimer();
        writeJason = writeJsonObj.GetComponent<WriteJson>();
        playerPositionList = new List<Vector3>();
        mpc = MainPuzlleControllerObject.GetComponent<MainPuzzleController>();
        for (int i = 0; i < this.gameObject.name.Length; i++){
            if (Char.IsDigit(this.gameObject.name[i])){
                selfIDString += this.gameObject.name[i];
                MyName = this.gameObject.name.Split((char)selfIDString[0])[0];
                selfID = int.Parse(selfIDString);
            }
        }
        nextID = selfID + 1;

        MyPuzzleInfo = mpc.GetType().GetMethod(MyName + selfID + "_Boolean");
        NextPuzzleInfo = mpc.GetType().GetMethod(MyName + nextID + "_Boolean");
    }
    private bool GetPuzzleActiveInfo(MethodInfo Method, bool PuzzleBool){
        var getMyPuzzleBool = Method.Invoke(mpc, null);
        PuzzleBool = Convert.ToBoolean(getMyPuzzleBool);
        return PuzzleBool;
    }
    // Update is called once per frame
    private void FixedUpdate() {
        isEmergent = mpc.isEmergentBool();
        WaterPumpBool = mpc.WaterPumpIsPumping;
        basketCounter = mpc.basketCollection;
        if(eventCap1.IamTriggered() && !checkEventOnce1){
            ECC_1 = eventCap1.ECC;
        }
        if (eventCap2.IamTriggered() && !checkEventOnce2)
        {
            ECC_2 = eventCap2.ECC;
        }
        if (!GetValuesOnce){
            if (isEmergent){
                nextID = 4;
            }

            MyPuzzleInfo = mpc.GetType().GetMethod(MyName + selfID + "_Boolean");
            NextPuzzleInfo = mpc.GetType().GetMethod(MyName + nextID + "_Boolean");
            GetValuesOnce = true;
        }
        if (!isEmergent){
            NextPuzzle = GetPuzzleActiveInfo(NextPuzzleInfo, NextPuzzle);
            MyPuzzle = GetPuzzleActiveInfo(MyPuzzleInfo, MyPuzzle) && !NextPuzzle;
        }else{
            NextPuzzle = GetPuzzleActiveInfo(NextPuzzleInfo, NextPuzzle);
            MyPuzzle = GetPuzzleActiveInfo(MyPuzzleInfo, MyPuzzle);
            // Debug.LogError("p1 is true? :"+MyPuzzle + " next id: " + nextID + " nextIDBool " + NextPuzzle);
        }
        if(selfID==3){
            Debug.LogError("MyPuzzle: " + MyPuzzle);
            Debug.LogError("NextPuzzle: " + NextPuzzle);
           
        }
    //    Debug.Log("mypuzzle " + MyPuzzle + " emergent " +isEmergent + " id " + selfID);
        if (!MyPuzzle && !writeOnce && playerPositionList.Count > 0){
          
            if (selfID == 1){
                writeJason.writeJason1 = true;
            }
            else if (selfID == 2){
                writeJason.writeJason2 = true;
            }
            else if (selfID == 3){
                Debug.LogError("Write json 3 id: " + selfID);
                writeJason.writeJason3 = true;
            }
           
            writeOnce = true;
        }
        if (MyPuzzle && isInside){            
            fixedCounter++;          
        }
    }

    private void OnTriggerStay(Collider other){
        if (MyPuzzle && other.tag == "Player"){
            isInside = true;
            playerPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            PlayerTimer();
           
            if (!audioSource.isPlaying && !hasPlayed){
                audioSource.PlayOneShot(emergentStart);
                hasPlayed = true;
            }          
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player"){
            isInside = false;
            hasPlayed = false;
        }
    }
    private void PlayerTimer(){
        if (fixedCounter % Mathf.Round(1f / Time.fixedDeltaTime) == 0){
            PlayerTimeCounter++;
            playerPositionList.Insert(0, playerPosition);
        }
    }
}
