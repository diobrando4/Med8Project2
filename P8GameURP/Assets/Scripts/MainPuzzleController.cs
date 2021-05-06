using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainPuzzleController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Puzzle1Controller;
    public GameObject Puzzle2Controller;
    public GameObject Puzzle3Controller;
    private Puzzle1Controller p1c;
    private Puzzle2Controller p2c;
    private Puzzle3Controller p3c;
    bool startP1, startP2, startP3, gameFinish;
    public InputField command;
    private bool executeCommand;
    public string textCommand;
    int enterCounter= 2;
    private bool closeInputField = false;
    public Transform cheatP2Pos;
    public Transform cheatP3Pos;
    public Transform player;
    bool p1Once = false;
    bool p2Once = false;
    bool p3Once = false;
    private string lastCommand="";
    private int fixedCounter=0;
    private bool canContinue = false;
    void Start()
    {
        p1c = Puzzle1Controller.GetComponent<Puzzle1Controller>();
        p2c = Puzzle2Controller.GetComponent<Puzzle2Controller>();
        p3c = Puzzle3Controller.GetComponent<Puzzle3Controller>();
        startP1 = true;
        command = command.GetComponent<InputField>();

    }
    private void FixedUpdate()
    {
        fixedCounter++;
    }
    // Update is called once per frame
    void Update()
    {
        Commands();
        if (p1c.collection<4 && !p1Once){
            startP1 = true;
            p1Once = true;
            Debug.LogError("1___ " + startP1);
        }
    
        if (p1c.collection > 4 && !p2Once)
        {
            p2Once = true;
            startP2 = true;
            Debug.LogError("2___ " + startP2);
            if (executeCommand) {
                activateP2(startP2);
            }
        }

        if (p2c.puzzle2Complete())
        {
            p3Once = true;
            startP3 = true;
            Debug.LogError("3___ " + startP3);
            fixedCounter = 0;
            activateP3(startP3);
        }

        if (!executeCommand&&Delay()) {
            activateP1(startP1);
            activateP2(startP2);
            activateP3(startP3);
        }else{
            activateP1(startP1);
            activateP2(startP2);
            activateP3(startP3);
        }
    }
    bool Delay(){
        return fixedCounter % Mathf.Round(0.2f / Time.fixedDeltaTime) == 0;
    }
    public void Commands(){
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            enterCounter++;
            
        }
        if (enterCounter%2!=0){
            Cursor.lockState = CursorLockMode.None;
            command.enabled = true;
            command.image.enabled = true;
            command.textComponent.enabled = true;

            ExecuteCommandFunction();
            if (closeInputField) {              
                closeInputField = false;
                enterCounter++;          
            }         
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            command.text = "";
            command.enabled = false;
            command.image.enabled = false;
            command.textComponent.enabled = false;
            
        }
       
        
       
    }
    public void activateP1(bool start){
        if (start) {
            p1c.startPuzzle1(start);
        }
    }
    public void activateP2(bool start){
        if (start) {

            p2c.startPuzzle2(start);
        } 
    }
    public void activateP3(bool start){
        if (start)
        {
            p3c.startPuzzle3(start);
        }
    }
    public void skipP1(){
        if(p1c.collection!=5){
            p1c.collection = 5;
        }        
    }

   

    void ExecuteCommandFunction(string optional="_optional_"){
        if(optional != "_optional_"){
            command.text = optional;
        }
        lastCommand = command.text;
        switch (command.text)
        {
            case "p1":
                p1c.resetP1(true);
                activateP1(true);
                p2c.basketCounter = 0;
                p2c.isPumping = false;
                startP3 = false;
                activateP3(false);
                p3c.resetP3(false);
                executeCommand = true;
                closeInputField = true;
                
               
               


                break;

            case "p2":
                p1c.resetP1(false);
                startP3 = false;
                skipP1();
                p2c.resetP2(true);
                p2c.basketCounter = 0;
                p2c.isPumping = false;
                p3Once = false;
                activateP3(false);
                p3c.resetP3(false);
                executeCommand = true;
                closeInputField = true;
                break;

            case "p3":

                p2c.basketCounter = 2;
                p3c.resetP3(true);
                startP3 = true;
                p3c.stop = false;
                activateP3(true);

                executeCommand = true;
                closeInputField = true;
                break;

            case "reset":
                closeInputField = true;
                SceneManager.LoadScene(1);
                break;

            case "status_p1":
                Debug.LogError("Puzzle 1 is active: "+startP1 + "  collection: " +  p1c.collection + " out of 5");
                executeCommand = true;
                closeInputField = true;
                break;

            case "status_p2":
                Debug.LogError("Puzzle 2 is active: "+startP2 + "  condition water is pumping: "+ p2c.isPumping + " condition basket counter >1 ? :" + p2c.basketCounter);
                executeCommand = true;
                closeInputField = true;
                break;

            case "status_p3":
                Debug.LogError("Puzzle 3 is active: " + startP3+"  is the objective in place? " + p3c.Puzzle3Complete());
                executeCommand = true;
                closeInputField = true;
                break;

            case "status_all":
                Debug.LogError("Puzzle 1 is active: " + startP1 + "  collection: " + p1c.collection + " out of 5");
                Debug.LogError("Puzzle 2 is active: " + startP2 + "  condition water is pumping: " + p2c.isPumping + " condition basket counter >1 ? :" + p2c.basketCounter);
                Debug.LogError("Puzzle 3 is active: " + startP3 + "  is the objective in place? " + p3c.Puzzle3Complete());
                executeCommand = true;
                closeInputField = true;
                break;

            case "cheat_p2":
                player.position = cheatP2Pos.position;
                executeCommand = true;
                closeInputField = true;
                break;

            case "cheat_p3":
                player.position = cheatP3Pos.position;
                executeCommand = true;
                closeInputField = true;
                break;
         
            default:
               
                break;

        }
       
    }


}
