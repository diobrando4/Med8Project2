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
    private WriteJasonData writeJason;

    public int basketCounter = 0;
    public bool WaterPumpBool = false;
    public int PlayerTimeCounter = 0;
    private int fixedCounter = 0;
    public int selfID;
    PuzzleTimer myTimer;
    public string MyName;
    private string selfIDString = string.Empty;
    
    public bool MyPuzzle = false; // if false then the puzzle is completed and the sound should be disabled?
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

    private bool GetValuesOnce = false;
    private int nextID;

    // not finished yet :D
    AudioSource audioSource;
    public AudioClip emergentStart;
    public AudioClip emergentSolution;

    // needs something that says "puzzle is completed: ture or false" so it doesn't repeat after being finished
    // if (MyPuzzleData.MyPuzzle != emergent){} then the sound should no longer play?

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        myTimer = new PuzzleTimer();
        writeJason = writeJsonObj.GetComponent<WriteJasonData>();
        playerPositionList = new List<Vector3>();
        mpc = MainPuzlleControllerObject.GetComponent<MainPuzzleController>();
        for (int i =0; i< this.gameObject.name.Length; i++){
            if(Char.IsDigit(this.gameObject.name[i])){
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
    private void FixedUpdate(){
        isEmergent = mpc.isEmergentBool();
        WaterPumpBool = mpc.WaterPumpIsPumping;
        basketCounter = mpc.basketCollection;

        if(!GetValuesOnce){
            if(isEmergent){
                nextID = 4;
            }

            MyPuzzleInfo = mpc.GetType().GetMethod(MyName + selfID + "_Boolean");
            NextPuzzleInfo = mpc.GetType().GetMethod(MyName + nextID + "_Boolean");
            GetValuesOnce = true;
        }
        if (!isEmergent) {
            NextPuzzle = GetPuzzleActiveInfo(NextPuzzleInfo, NextPuzzle);
            MyPuzzle = GetPuzzleActiveInfo(MyPuzzleInfo, MyPuzzle) && !NextPuzzle;
        }else{
            NextPuzzle = GetPuzzleActiveInfo(NextPuzzleInfo, NextPuzzle);
            MyPuzzle = GetPuzzleActiveInfo(MyPuzzleInfo, MyPuzzle);
           // Debug.LogError("p1 is true? :"+MyPuzzle + " next id: " + nextID + " nextIDBool " + NextPuzzle);
        }
        //Debug.LogError("mypuzzle " + MyPuzzle);
        if(!MyPuzzle && !writeOnce && playerPositionList.Count>0)
        {
            if(selfID==1){
                writeJason.writeJason1 = true;
            }else if(selfID==2){
                writeJason.writeJason2 = true;
            }else if(selfID==3){
                writeJason.writeJason3 = true;
            }
            Debug.LogError("Write json id: " + selfID);  
            writeOnce = true;
        }
        if (MyPuzzle && isInside) {
            fixedCounter++;
        }
    }

    private void OnTriggerStay(Collider other){
        if(MyPuzzle && other.tag=="Player"){
            isInside = true;
            playerPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            PlayerTimer();           
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            isInside = false;
        }
    }
    private void PlayerTimer(){
        if (fixedCounter % Mathf.Round(1f / Time.fixedDeltaTime) == 0){
            PlayerTimeCounter++;
            playerPositionList.Insert(0, playerPosition);
        }
    }
}
