using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject objective;
    public GameObject objectivePOS;
    bool p3Complete;
    Outline ol;
    Outline ol_Objective;
    // Start is called before the first frame update
    void Start()
    {
        ol = GetComponent<Outline>();
        ol_Objective = objectivePOS.GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startPuzzle(bool start){
        if(start&&!Puzzle3Complete())
        {
            objective.SetActive(true);
            objectivePOS.SetActive(false);
            ol.enabled = true;
            ol_Objective.enabled = true;
        }
        else if(!start){
            objective.SetActive(false);
            objectivePOS.SetActive(true);
            ol.enabled = false;
            ol_Objective.enabled = false;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag== "Electric_Box_Objective")
        {
            objectivePOS.SetActive(true);
            objective.SetActive(false);
            p3Complete = true;
        }
    }

    public bool Puzzle3Complete(){
        return p3Complete;
    }


}
