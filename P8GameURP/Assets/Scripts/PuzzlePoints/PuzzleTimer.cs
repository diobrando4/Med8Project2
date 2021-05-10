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

    public int basketCounter = 0;
    public bool WaterPumpBool =false;
    public int PlayerTimeCounter = 0;
    private int fixedCounter = 0;
    public int selfID;

    public string MyName;
    private string selfIDString = string.Empty;
    
    public bool MyPuzzle = false;
    public bool NextPuzzle = false;
    private bool isInside = false;
    private bool once = false;

    private MethodInfo MyPuzzleInfo;
    private MethodInfo NextPuzzleInfo;
    
    void Start()
    {
        

        playerPositionList = new List<Vector3>();
        mpc = MainPuzlleControllerObject.GetComponent<MainPuzzleController>();
        for (int i =0; i< this.gameObject.name.Length; i++){
            if(Char.IsDigit(this.gameObject.name[i])){
                selfIDString += this.gameObject.name[i];
                MyName = this.gameObject.name.Split((char)selfIDString[0])[0];
                selfID = int.Parse(selfIDString);
            }            
        }
        int nextID = selfID + 1;

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
        WaterPumpBool = mpc.WaterPumpIsPumping;
        basketCounter = mpc.basketCollection;

        NextPuzzle = GetPuzzleActiveInfo(NextPuzzleInfo, NextPuzzle);      
        MyPuzzle = GetPuzzleActiveInfo(MyPuzzleInfo, MyPuzzle) && !NextPuzzle;

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
