using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.SceneManagement;
public class PuzzleData
{
    
    public double UniqueID;
    public string Name;
    public int Time;
    public bool _Emergent;
    public int EventCapsuleCounter = 0;
    public int BasketCounter = 0;
    public bool WaterIsPumping = false;
    public List<string> Player_Positions;

    public PuzzleData(double UniqueID, string Name, int Time, bool Emergent, int EventCapsuleCounter)
    {
        this.UniqueID = UniqueID;
        this.Name = Name;
        this.Time = Time;
        this._Emergent = Emergent;
        this.Player_Positions = new List<string>();
        this.EventCapsuleCounter = EventCapsuleCounter;
    }
}

public class CollectSavedData
{
    public List<PuzzleData> CollectionList;
    public CollectSavedData()
    {

        this.CollectionList = new List<PuzzleData>();
    }

}

public class WriteJasonData : MonoBehaviour
{
    private static string path = "/Json/PuzzleStates_.json";

    public GameObject GUIMenu;
    private MenuGUI MGUI;
    public bool Emergent = false;
    private bool MyEmergent = false;
    public GameObject p1TimerObject;
    public GameObject p2TimerObject;
    public GameObject p3TimerObject;
    private GameObject GUI;
    private static MenuGUI mGUI;
    private PuzzleTimer p1timer;
    private PuzzleTimer p2timer;
    private PuzzleTimer p3timer;

    private bool p1once = false;
    private bool p2once = false;
    private bool p3once = false;

    static CollectSavedData csd;
    List<string> MyList;
    List<string> p2pos;
    List<string> p3pos;

    public static PuzzleData P1Data = null;
    public static PuzzleData P2Data = null;
    public static PuzzleData P3Data = null;
    public static PuzzleData P1Data_2 = null;
    public static PuzzleData P2Data_2 = null;
    public static PuzzleData P3Data_2 = null;

    static bool staticOnce = false;
    static bool staticWriteOnce = false;
    private bool JustOnce = false;
    public bool writeJason1 = false;
    public bool writeJason2 = false;
    public bool writeJason3 = false;
    static int indexCounter = 0;
    static double MyUniqueID;
    void Start()
    {

        GUI = GameObject.FindGameObjectWithTag("GUI");
        mGUI = GUI.GetComponent<MenuGUI>();

        MyList = new List<string>();
        p2pos = new List<string>();
        p3pos = new List<string>();
        p1timer = p1TimerObject.GetComponent<PuzzleTimer>();
        p2timer = p2TimerObject.GetComponent<PuzzleTimer>();
        p3timer = p3TimerObject.GetComponent<PuzzleTimer>();
        p1once = false;
        p2once = false;
        p3once = false;


    }

    static bool getGameType()
    {
        return mGUI.getGameType();
    }
    static double getUniqueID()
    {
        return mGUI.getUniqeID();
    }
    private void Update()
    {



        if (!staticOnce)
        {
            MyUniqueID = getUniqueID();
            csd = new CollectSavedData();
            staticOnce = true;
        }
        else
        {
            Debug.LogError(" staticOnce is true  getCount : " + csd.CollectionList.Count + " static : " + staticOnce);
        }
        if (!JustOnce)
        {

            JustOnce = true;
        }
        
        // WritePuzzleDataBool(0);
        MyEmergent = getGameType();
        Debug.LogError("j1:" + writeJason1 + " j2: " + writeJason2 + " j3 " + writeJason3);

        if (!p1once && writeJason1){
            if (P1Data == null && !p1once){
                P1Data = SetPuzzleData(p1timer, P1Data, P1Data_2, MyEmergent);
                csd.CollectionList.Insert(indexCounter, P1Data);
                indexCounter++;
            }else if (P1Data != null && !p1once && P1Data_2==null){
                P1Data_2 = SetPuzzleData(p1timer, P1Data, P1Data_2, MyEmergent);
                csd.CollectionList.Insert(indexCounter, P1Data_2);
                indexCounter++;
            }
        }
        if (!p2once && writeJason2){
            if (P2Data == null && !p2once){
                P2Data = SetPuzzleData(p2timer, P2Data, P2Data_2, MyEmergent);
                csd.CollectionList.Insert(indexCounter, P2Data);
                indexCounter++;
            }else if (P2Data != null && !p2once && P2Data_2 == null){
                P2Data_2 = SetPuzzleData(p2timer, P2Data, P2Data_2, MyEmergent);
                csd.CollectionList.Insert(indexCounter, P2Data_2);
                indexCounter++;
            }
        }
        if (!p3once && writeJason3){
            if (P3Data == null && !p3once){
                P3Data = SetPuzzleData(p3timer, P3Data, P3Data_2, MyEmergent);
                csd.CollectionList.Insert(indexCounter, P3Data);
                indexCounter++;
            }else if (P3Data != null && !p3once && P3Data_2 == null){
                P3Data_2 = SetPuzzleData(p3timer, P3Data, P3Data_2, MyEmergent);
                csd.CollectionList.Insert(indexCounter, P3Data_2);
                indexCounter++;
            }

        }

        Debug.LogError("insertIndex " + indexCounter + " current count: " + csd.CollectionList.Count);
        Debug.LogError("csd.CollectionList: " + csd.CollectionList.Count + " game is emergent " + MyEmergent + " has written= " + staticWriteOnce);
        if (csd.CollectionList.Count == 6 && !staticWriteOnce)
        {
            writeJson(csd);
            staticWriteOnce = true;
        }

    }
    public PuzzleData SetPuzzleData(PuzzleTimer MyPuzzleData, PuzzleData SetMyPuzzleData, PuzzleData SetMyPuzzleData_2, bool emergent)
    {
        //if (!MyPuzzleData.MyPuzzle && !MyPuzzleData.NextPuzzle && !doThisOnce)
        if (MyPuzzleData.MyPuzzle != emergent){
            if (MyPuzzleData.selfID == 1){
                p1once = true;

            }else if (MyPuzzleData.selfID == 2){
                p2once = true;

            }else if (MyPuzzleData.selfID == 3){
                p3once = true;
            }

            Debug.LogError("setpuzzle!!! ");
            for (int i = 0; i < MyPuzzleData.playerPositionList.Count; i++){
                MyList.Insert(indexCounter, MyPuzzleData.playerPositionList[i].ToString());
            }
            SetMyPuzzleData = new PuzzleData(MyUniqueID, MyPuzzleData.MyName + MyPuzzleData.selfID, MyPuzzleData.PlayerTimeCounter, MyEmergent, (MyPuzzleData.ECC_1+ MyPuzzleData.ECC_2));
            SetMyPuzzleData.Player_Positions = MyList;
            Debug.LogError("csd.CollectionList: " + csd.CollectionList.Count + " game is emergent " + MyEmergent);
            return SetMyPuzzleData;
        }
        return new PuzzleData(MyUniqueID, MyPuzzleData.MyName + MyPuzzleData.selfID, MyPuzzleData.PlayerTimeCounter, MyEmergent, (MyPuzzleData.ECC_1 + MyPuzzleData.ECC_2));
    }

    /*static void SetSave(CollectSavedData data)
    {
        csd = data;
    }*/


    static void writeJson(CollectSavedData ClassData)
    {
        Debug.LogError("Writing Jason");
        JsonData newData = new JsonData();

        newData = JsonMapper.ToJson(ClassData);

        string data = newData.ToString();

        File.WriteAllText(Application.dataPath + path, data);
        Debug.LogError(" Json is done ");
    }
}
