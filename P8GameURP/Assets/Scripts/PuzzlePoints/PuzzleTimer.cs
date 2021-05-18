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

    public GameObject NeighboorPuzzle_1;
    public GameObject NeighboorPuzzle_2;

    private PuzzleTimer NP_1;
    private PuzzleTimer NP_2;

    public GameObject bgmusic;
    private BGMusic bg;

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

    private MethodInfo IsAPuzzlePlaying_1;
    private MethodInfo IsAPuzzlePlaying_2;

    private MethodInfo IsPlayerInside_1;
    private MethodInfo IsPlayerInside_2;

    private MethodInfo GetNeighboorID_1;
    private MethodInfo GetNeighboorID_2;

    bool PlayerInsideBefore=false;
    bool PlayerInsideAfter=false;

    int NeighboorBeforeID = 0;
    int NeighboorAfterID = 0;

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
    public bool CanIPlayMusic = false;
    AudioSource audioSource;
    public AudioClip emergentStart;
    private int before = 0;
    private int after = 0;
    
    public Transform Dgroub;
    public Transform Pgroub;
    public Transform Fgroub;
     List<Transform> Doughnuts_EC_SpacePositions;
     List<Transform> Park_EC_SpacePositions;
     List<Transform> Factory_EC_SpacePositions;
    //public AudioClip emergentSolution;

    void Start(){
        Doughnuts_EC_SpacePositions = new List<Transform>();
        Park_EC_SpacePositions = new List<Transform>();
        Factory_EC_SpacePositions = new List<Transform>();
        for (int i =0; i< Dgroub.childCount; i++){
            Doughnuts_EC_SpacePositions.Insert(i, Dgroub.GetChild(i).transform);
        }
        for (int i = 0; i < Pgroub.childCount; i++){
            Park_EC_SpacePositions.Insert(i, Pgroub.GetChild(i).transform);
        }
        for (int i = 0; i < Fgroub.childCount; i++){
            Factory_EC_SpacePositions.Insert(i, Fgroub.GetChild(i).transform);
        }
        audioSource = GetComponent<AudioSource>();
        eventCap1 = eventCapsule1.GetComponent<rotateObject>();
        eventCap2 = eventCapsule2.GetComponent<rotateObject>();
        NP_1 = NeighboorPuzzle_1.GetComponent<PuzzleTimer>();
        NP_2 = NeighboorPuzzle_2.GetComponent<PuzzleTimer>();
        bg = bgmusic.GetComponent<BGMusic>();
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
        before = selfID - 1>= 1 ? selfID - 1 : 3;
        after = selfID + 1 <= 3 ? selfID + 1 : 1;
        nextID = selfID + 1;

        MyPuzzleInfo = mpc.GetType().GetMethod(MyName + selfID + "_Boolean");
        NextPuzzleInfo = mpc.GetType().GetMethod(MyName + nextID + "_Boolean");

  

    }
    private bool GetPuzzleActiveInfo(MethodInfo Method, bool PuzzleBool){
        var getMyPuzzleBool = Method.Invoke(mpc, null);
        PuzzleBool = Convert.ToBoolean(getMyPuzzleBool);
        return PuzzleBool;
    }
    private bool DoesMyNeighboorPlayMusic(MethodInfo Method,PuzzleTimer Neighboor,bool PuzzleBool){
        var getMyPuzzleBool = Method.Invoke(Neighboor, null);
        PuzzleBool = Convert.ToBoolean(getMyPuzzleBool);
        return PuzzleBool;
    }
    private bool IsPlayerInsideMe(MethodInfo Method, PuzzleTimer Neighboor, bool PuzzleBool)
    {
        var getMyPuzzleBool = Method.Invoke(Neighboor, null);
        PuzzleBool = Convert.ToBoolean(getMyPuzzleBool);
        return PuzzleBool;
    }

    private int GetMyNeighboorID(MethodInfo Method, PuzzleTimer Neighboor, int NeighboorID)
    {
        var getMyNeighboorID = Method.Invoke(Neighboor, null);
        NeighboorID = Convert.ToInt32(getMyNeighboorID);
        return NeighboorID;
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


            IsAPuzzlePlaying_1 = NP_1.GetType().GetMethod(MyName + before + "_isPlaying");
            IsPlayerInside_1 = NP_1.GetType().GetMethod(MyName + before + "_isInside");
            GetNeighboorID_1 = NP_1.GetType().GetMethod(MyName + before + "_MyID");

            IsAPuzzlePlaying_2 = NP_2.GetType().GetMethod(MyName + after + "_isPlaying");
            IsPlayerInside_2 = NP_2.GetType().GetMethod(MyName + after + "_isInside");
            GetNeighboorID_2 = NP_2.GetType().GetMethod(MyName + after + "_MyID");

            Debug.Log("myname after" +MyName+after);
            Debug.Log("myname before "+MyName+before);
            GetValuesOnce = true;
        }
        if (!isEmergent){
            NextPuzzle = GetPuzzleActiveInfo(NextPuzzleInfo, NextPuzzle);
            MyPuzzle = GetPuzzleActiveInfo(MyPuzzleInfo, MyPuzzle) && !NextPuzzle;
        }else{
            NextPuzzle = GetPuzzleActiveInfo(NextPuzzleInfo, NextPuzzle);
            MyPuzzle = GetPuzzleActiveInfo(MyPuzzleInfo, MyPuzzle);

            PlayerInsideBefore = IsPlayerInsideMe(IsPlayerInside_1,NP_1,PlayerInsideBefore);
            PlayerInsideAfter = IsPlayerInsideMe(IsPlayerInside_2,NP_2,PlayerInsideAfter);

            NeighboorBeforeID = GetMyNeighboorID(GetNeighboorID_1,NP_1,NeighboorBeforeID);
            NeighboorAfterID = GetMyNeighboorID(GetNeighboorID_2,NP_2,NeighboorAfterID);
            // Debug.LogError("p1 is true? :"+MyPuzzle + " next id: " + nextID + " nextIDBool " + NextPuzzle);
        }
      
        CanIPlayMusic = !DoesMyNeighboorPlayMusic(IsAPuzzlePlaying_1, NP_1, CanIPlayMusic) && !DoesMyNeighboorPlayMusic(IsAPuzzlePlaying_2, NP_2, CanIPlayMusic);
        eventCap1.canPlay = CanIPlayMusic && !audioSource.isPlaying && bg.IamPlayingTheThemeSong();
        eventCap2.canPlay = CanIPlayMusic && !audioSource.isPlaying && bg.IamPlayingTheThemeSong();
    //    Debug.Log("mypuzzle " + MyPuzzle + " emergent " +isEmergent + " id " + selfID);
        if (!MyPuzzle && !writeOnce && playerPositionList.Count > 0){
          
            if (selfID == 1){
                writeJason.writeJason1 = true;
            }
            else if (selfID == 2){
                writeJason.writeJason2 = true;
            }
            else if (selfID == 3){
             //   Debug.LogError("Write json 3 id: " + selfID);
                writeJason.writeJason3 = true;
            }
           
            writeOnce = true;
        }
        if (MyPuzzle && isInside){            
            fixedCounter++;          
        }
        Debug.Log("PlayerInsideBefore: " + PlayerInsideBefore + " my ID: " + selfID);
        if (selfID == 1)
        {
            Debug.Log(" this.PlayerInsideBefore: " + PlayerInsideBefore + " before id " + before);
            Debug.Log(" this.PlayerInsideAfter " + PlayerInsideAfter + " after id " + after);
        }
        if (!MyPuzzle && !isInside && isEmergent){
            Debug.Log("reposition eventCapsules");
            
            if (PlayerInsideBefore)
            {
                Debug.Log("reposition eventCapsules BEFORE SelfID: " + selfID);
                if (NeighboorBeforeID == 1)
                { // before 2 = 1 
                    
                    // this is event cap 2 this works
                    if (!eventCap1.IamTriggered())
                    {
                        eventCapsule1.transform.position = Doughnuts_EC_SpacePositions[selfID - 2].position;
                    }
                    if (!eventCap2.IamTriggered())
                    {
                        eventCapsule2.transform.position = Doughnuts_EC_SpacePositions[selfID].position;
                    }
                }
                if (NeighboorBeforeID == 2)
                { // before 3 = 2 
                    Debug.Log("MY NAME:  " + this.gameObject.name + " L246 ");
                    // this is event cap 3
                    if (!eventCap1.IamTriggered())
                        eventCapsule1.transform.position = Park_EC_SpacePositions[selfID + 1].position;

                    if (!eventCap2.IamTriggered())
                        eventCapsule2.transform.position = Park_EC_SpacePositions[selfID].position;
                }
                if (NeighboorBeforeID == 3)//this works
                { // before 3 = 1 
     
                    if (!eventCap1.IamTriggered())
                        eventCapsule1.transform.position = Factory_EC_SpacePositions[selfID - 1].position;

                    if (!eventCap2.IamTriggered())
                        eventCapsule2.transform.position = Factory_EC_SpacePositions[selfID + 1].position;

                }
            }
            if (PlayerInsideAfter)
            {// After 1 = 2 After 2 = 3 After 3 = 1
                Debug.Log("reposition eventCapsules AFTER SelfID: " + selfID);
                if (NeighboorAfterID == 1)// this works
                {
                    // event cap 3
                    if (!eventCap1.IamTriggered())
                    {
                        eventCapsule1.transform.position = Doughnuts_EC_SpacePositions[selfID - 2].position;
                    }
                    if (!eventCap2.IamTriggered())
                    {
                        eventCapsule2.transform.position = Doughnuts_EC_SpacePositions[selfID].position;
                    }
                }
                if (NeighboorAfterID == 2)
                {
                    if (selfID == 1)
                    {
                        Debug.Log(" before 2: event 1: " + Park_EC_SpacePositions[selfID - 1].position);
                        Debug.Log(" before 2: event 2: " + Park_EC_SpacePositions[selfID + 1].position);
                    }
                    // Event cap 1
                    Debug.Log("MY NAME:  "+this.gameObject.name + " Line 288");
                    if (!eventCap1.IamTriggered())
                        eventCapsule1.transform.position = Park_EC_SpacePositions[0].position;

                    if (!eventCap2.IamTriggered())
                        eventCapsule2.transform.position = Park_EC_SpacePositions[2].position;
                }
                if (NeighboorAfterID == 3)
                { // before 3 = 2 after 3 = 1
                    // Event cap 2 this works
                    if (!eventCap1.IamTriggered())
                        eventCapsule1.transform.position = Factory_EC_SpacePositions[selfID - 1].position;

                    if (!eventCap2.IamTriggered())
                        eventCapsule2.transform.position = Factory_EC_SpacePositions[selfID + 1].position;

                }
            }

            /*if (PlayerInsideAfter){

                if (NeighboorAfterID == 1){ // after 1 =3

                }
                if (NeighboorAfterID == 2){ // after 2 = 3

                }
                if (NeighboorAfterID == 3){ // after 3 = 1

                }

            }


            if (PlayerInsideBefore && NeighboorBeforeID==1)
            {
                if (!eventCap1.IamTriggered()){
                    eventCapsule1.transform.position = Doughnuts_EC_SpacePositions[selfID - 2].position;
                }
                if (!eventCap2.IamTriggered()) {
                    eventCapsule2.transform.position = Doughnuts_EC_SpacePositions[selfID].position;
                }
            }
            if (PlayerInsideBefore && NeighboorBeforeID == 2){

                if (!eventCap1.IamTriggered())
                    eventCapsule1.transform.position = selfID == 1  ? Park_EC_SpacePositions[selfID - 1].position : Doughnuts_EC_SpacePositions[selfID - 2].position;
               
                if (!eventCap2.IamTriggered())
                    eventCapsule2.transform.position = selfID == 1  ? Park_EC_SpacePositions[selfID + 1].position : Doughnuts_EC_SpacePositions[selfID].position;                                             
            }
            if (PlayerInsideBefore && NeighboorBeforeID == 3){

                if (!eventCap1.IamTriggered())
                    eventCapsule1.transform.position = selfID == 1  ? Park_EC_SpacePositions[selfID - 1].position : Doughnuts_EC_SpacePositions[selfID - 2].position;
                
                if (!eventCap2.IamTriggered())
                    eventCapsule2.transform.position = selfID == 1  ? Park_EC_SpacePositions[selfID + 1].position : Doughnuts_EC_SpacePositions[selfID].position;

            }
            if (!eventCap1.IamTriggered() && PlayerInsideBefore){ // counts for position index 0,1,2
                              
            }*/

        }
    }

    private void OnTriggerStay(Collider other){
        if (MyPuzzle && other.tag == "Player"){
            isInside = true;
            playerPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            PlayerTimer();
           
            if (!audioSource.isPlaying && !hasPlayed && CanIPlayMusic && bg.IamPlayingTheThemeSong() && isEmergent)
            {
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

    public bool startP1_isPlaying(){
        return audioSource.isPlaying;
    }
    public bool startP2_isPlaying()
    {
        return audioSource.isPlaying;
    }
    public bool startP3_isPlaying()
    {
        return audioSource.isPlaying;
    }
    public bool startP1_isInside()
    {
        return isInside && MyPuzzle;
    }
    public bool startP2_isInside()
    {
        return isInside && MyPuzzle;
    }
    public bool startP3_isInside()
    {
        return isInside && MyPuzzle;
    }

    public int startP1_MyID(){
        return selfID;
    }
    public int startP2_MyID()
    {
        return selfID;
    }
    public int startP3_MyID()
    {
        return selfID;
    }

}
