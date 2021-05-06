using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuGUI : MonoBehaviour
{

    public GameObject PauseScreenGUI;
    public GameObject SettingsScreenGUI;
    public GameObject MenuScreenGUI;
    public GameObject EmergentGUI;
    public GameObject LinierGUI;
    public bool PauseBool = false;
    private bool MenuBool = false;
    private string StartSceneName;
    private int escCounter = 0;
    public static bool EmergentGame;
    private static bool setGameOnce = false;
    static float gameType;
    static int GameCompletionCounter = 0;
    GameObject MainPuzzleControllerObject;
    MainPuzzleController mpc;
    bool runOnce;
    bool p1Active;
    bool p2Active;
    bool p3Active;
    bool gameIsCompleted;
    // Start is called before the first frame update
    void Start() {

        MenuBool = true;
        StartSceneName = "menu";

        if (SceneManager.GetActiveScene().name == StartSceneName){
            if(runOnce){ runOnce = false; }

            if (!setGameOnce){

                setGameType();
                setGameOnce = true;
                Debug.LogError("game type is set: " + setGameOnce);

            }else{

                setGameType();
                Debug.LogError("ELSE game type is set: " + setGameOnce);
            }
        }
        MenuScreen();
    }
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
    public static void setGameType() {
        if (!setGameOnce) {
            gameType = Random.value;
            Debug.LogError("value below 0.5 is Linear: " + gameType);
            EmergentGame = gameType > 0.5f ? true : false;
        } else {
            EmergentGame = !EmergentGame;
        }
    }

    public static bool getGameType() {
        return EmergentGame;
    }

    // Update is called once per frame
    void Update() {

        if (SceneManager.GetActiveScene().name != StartSceneName && Input.GetKeyDown(KeyCode.Escape)){
            pauseBtn();
            
            p1Active = mpc.startP1;
            p2Active = mpc.startP2;
            p3Active = mpc.startP3;

        }else if(SceneManager.GetActiveScene().name != StartSceneName ) {
            if (!runOnce){

                MainPuzzleControllerObject = GameObject.FindGameObjectWithTag("MainPuzzleController");
                mpc = MainPuzzleControllerObject.GetComponent<MainPuzzleController>();
                runOnce = true;
            }
            gameIsCompleted = mpc.gameFinish;

            if (gameIsCompleted){
                GameIsCompleted();
            }
        }

    }
    public void loadGame() {

        SceneManager.LoadSceneAsync(1);
        resumeBtn();
    }
    public void backBtn() {

        if (PauseBool) {
            PauseScreen();
        } else {
            MenuScreen();
        }
    }

    public void pauseBtn() {

        escCounter++;
        if (escCounter % 2 != 0) {
            PauseScreen();
        } else {
            resumeBtn();
        }
    }
    public void resetPuzzle(){
        if(p1Active && !p2Active && !p3Active){
            mpc.ExecuteCommandFunction("p1");

        }else if (p1Active && p2Active && !p3Active){
            mpc.ExecuteCommandFunction("p2");

        }else if (p1Active && p2Active && p3Active){
            mpc.ExecuteCommandFunction("p3");
        }
        resumeBtn();

    }
    void GameIsCompleted(){
        GameCompletionCounter++;
        SceneManager.LoadSceneAsync(0);
    }
    public void resumeBtn(){

        MenuScreenGUI.SetActive(false);
        SettingsScreenGUI.SetActive(false);
        PauseScreenGUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        if(Time.timeScale != 1){
            Time.timeScale = 1;
        }
        if (PauseBool) {
            PauseBool = false;
        }       
    }

    public void MenuScreen(){

        Cursor.lockState = CursorLockMode.None;
        MenuScreenGUI.SetActive(true);        
        SettingsScreenGUI.SetActive(false);
        PauseScreenGUI.SetActive(false);

        if(EmergentGame){
            EmergentGUI.SetActive(true);
            LinierGUI.SetActive(false);
        }
        if(!EmergentGame){ 
            EmergentGUI.SetActive(false);
            LinierGUI.SetActive(true);
            
        }
    }

    public void SettingsScreen(){

        SettingsScreenGUI.SetActive(true);
        MenuScreenGUI.SetActive(false);
        PauseScreenGUI.SetActive(false);
    }

    public void PauseScreen(){
    
        Cursor.lockState = CursorLockMode.None;
        PauseScreenGUI.SetActive(true);
        SettingsScreenGUI.SetActive(false);
        MenuScreenGUI.SetActive(false);
        if (Time.timeScale != 0){
            Time.timeScale = 0;           
        }
        if(!PauseBool){
            PauseBool = true;
        }
    }

    public void exitGame(){

        Debug.LogError("Game is exiting_ only works in a Build");
        Application.Quit();
        
    }
}
