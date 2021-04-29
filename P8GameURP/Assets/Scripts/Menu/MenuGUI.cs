using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuGUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadGame(){
        SceneManager.LoadSceneAsync(1);
    }

    public void exitGame(){
        Debug.LogError("Game is exiting_ only works in a Build");
        Application.Quit();
        
    }
}
