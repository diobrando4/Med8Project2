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
    public int BasketCounter=0;
    public bool WaterIsPumping=false;
    public List<string> Player_Positions;
    
  public PuzzleData(double UniqueID, string Name, int Time, bool Emergent){
        this.UniqueID = UniqueID;
        this.Name = Name;
        this.Time = Time;
        this._Emergent = Emergent;
        this.Player_Positions = new List<string>();        
    }
}

public class CollectSavedData {
    public List<PuzzleData> CollectionList;
    public CollectSavedData() {
        
        this.CollectionList = new List<PuzzleData>();
    }
 
}

public class WriteJasonData : MonoBehaviour
{
    private static string path = "/Json/PuzzleStates_.json";

    public GameObject GUIMenu;
    private MenuGUI MGUI;
    public bool Emergent =false;
    private bool MyEmergent = false;
    public GameObject p1TimerObject;
    public GameObject p2TimerObject;
    public GameObject p3TimerObject;
    private GameObject GUI;
    private static MenuGUI mGUI;
    private PuzzleTimer p1timer;
    private PuzzleTimer p2timer;
    private PuzzleTimer p3timer;

    private  bool p1once =false;
    private  bool p2once = false;
    private  bool p3once = false;

    static CollectSavedData csd;
    List<string> p1pos;
    List<string> p2pos;
    List<string> p3pos;

    static PuzzleData P1Data = null;
    static PuzzleData P2Data = null;
    static PuzzleData P3Data = null;
    static PuzzleData P1Data_2 = null;
    static PuzzleData P2Data_2 = null;
    static PuzzleData P3Data_2 = null;

    static bool staticOnce = false;
    static bool staticWriteOnce = false;
    private bool JustOnce = false;
    static double MyUniqueID;
    void Start()
    {
       
        GUI = GameObject.FindGameObjectWithTag("GUI");      
        mGUI = GUI.GetComponent<MenuGUI>();

        p1pos = new List<string>();
        p2pos = new List<string>();
        p3pos = new List<string>();        
        p1timer = p1TimerObject.GetComponent<PuzzleTimer>();
        p2timer = p2TimerObject.GetComponent<PuzzleTimer>();
        p3timer = p3TimerObject.GetComponent<PuzzleTimer>();
       
        if (!staticOnce)
        {
            MyUniqueID = getUniqueID();
            csd = new CollectSavedData();
            staticOnce = true;
        }
    }
    static bool getGameType(){
        return mGUI.getGameType();
    }
    static double getUniqueID()
    {
        return mGUI.getUniqeID();
    }
    private void Update()
    {

            if(!JustOnce){
             
                JustOnce = true;
            }

        MyEmergent = getGameType();
        if (!p1timer.MyPuzzle && p1timer.NextPuzzle && !p1once){
                p1once = true;
                for (int i = 0; i < p1timer.playerPositionList.Count; i++)
                {
                    p1pos.Insert(0, p1timer.playerPositionList[i].ToString());
                }
                if (P1Data==null) {
                    P1Data = new PuzzleData(MyUniqueID, p1timer.MyName+p1timer.selfID, p1timer.PlayerTimeCounter, MyEmergent);
                    P1Data.Player_Positions = p1pos;
                    csd.CollectionList.Insert(0,P1Data);
                }
                else if(P1Data!=null){
                    P1Data_2 = new PuzzleData(MyUniqueID, p1timer.MyName + p1timer.selfID, p1timer.PlayerTimeCounter, MyEmergent);
                    P1Data_2.Player_Positions = p1pos;
                    csd.CollectionList.Insert(3, P1Data_2);
                }
               
               // Debug.LogError("Save Data P1 Emergent:" + MyEmergent);
                Debug.LogError("csd.CollectionList: " + csd.CollectionList.Count + " game is emergent " + MyEmergent);
            }

            if (!p2timer.MyPuzzle && p2timer.NextPuzzle && !p2once){
                p2once = true;
                for (int i = 0; i < p2timer.playerPositionList.Count; i++)
                {
                    p2pos.Insert(0, p2timer.playerPositionList[i].ToString());
                }
                if (P2Data==null) {
                    P2Data = new PuzzleData(MyUniqueID, p2timer.MyName + p2timer.selfID, p2timer.PlayerTimeCounter, MyEmergent);
                    P2Data.Player_Positions = p2pos;
                    P2Data.BasketCounter = p2timer.basketCounter;
                    P2Data.WaterIsPumping = p2timer.WaterPumpBool;
                    csd.CollectionList.Insert(1, P2Data);
                   // Debug.LogError("P2Data loaded");
                }
                else if(P2Data!=null){
                    
                    P2Data_2 = new PuzzleData(MyUniqueID, p2timer.MyName + p2timer.selfID, p2timer.PlayerTimeCounter, MyEmergent);
                    P2Data_2.Player_Positions = p2pos;
                    P2Data_2.BasketCounter = p2timer.basketCounter;
                    P2Data_2.WaterIsPumping = p2timer.WaterPumpBool;
                    csd.CollectionList.Insert(4, P2Data_2);
                   // Debug.LogError("P2Data_2 loaded");
                }
              //  Debug.LogError("Save Data P2 Emergent:" + MyEmergent);
                Debug.LogError("csd.CollectionList: " + csd.CollectionList.Count + " game is emergent " + MyEmergent);
            }

            if (!p3timer.MyPuzzle && p3timer.NextPuzzle && !p3once){
                p3once = true;
                for (int i = 0; i < p3timer.playerPositionList.Count; i++)
                {
                    p3pos.Insert(0, p3timer.playerPositionList[i].ToString());
                }
                if (P3Data==null) {
                    P3Data = new PuzzleData(MyUniqueID, p3timer.MyName + p3timer.selfID, p3timer.PlayerTimeCounter, MyEmergent);
                    P3Data.Player_Positions = p3pos;
                    csd.CollectionList.Insert(2, P3Data);

                }else if(P3Data!= null){
                    P3Data_2 = new PuzzleData(MyUniqueID, p3timer.MyName + p3timer.selfID, p3timer.PlayerTimeCounter, MyEmergent);
                    P3Data_2.Player_Positions = p3pos;
                    csd.CollectionList.Insert(5, P3Data_2);
                }
             //   Debug.LogError("Save Data P3 Emergent:" + MyEmergent);
                Debug.LogError("csd.CollectionList: " + csd.CollectionList.Count + " game is emergent " + MyEmergent);
            }
           
            if(csd.CollectionList.Count==6 && !staticWriteOnce) {
                writeJson(csd);
                staticWriteOnce = true;
            }
        
    }

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
