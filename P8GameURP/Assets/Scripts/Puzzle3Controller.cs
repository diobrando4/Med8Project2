using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] puzzleObjects;
   // public GameObject truck1;
   // public GameObject truck2;
    TruckScript t1;
    TruckScript t2;
    PressurePlate pp;
    public bool s = false;
    void Start()
    {
        t1 = puzzleObjects[0].GetComponent<TruckScript>();
        t2 = puzzleObjects[1].GetComponent<TruckScript>();
        pp = puzzleObjects[5].GetComponent<PressurePlate>();
    }

    // Update is called once per frame
    void Update()
    {
       // startPuzzle3(s);
    }
    public void startPuzzle3(bool start){

        
        t1.start = start;
        t2.start = start;
        if (start) {
            for (int i = 2; i < 5; i++) {
                puzzleObjects[i].SetActive(Puzzle3Complete());
            }
        }else{
            for (int i = 2; i < 5; i++)
            {
                puzzleObjects[i].SetActive(true);
            }
        }
        pp.startPuzzle(start);
        if(Puzzle3Complete()){
            Debug.LogError("PUZZLE 3 COMPLETE !!! ");
        }

    }
    public bool Puzzle3Complete(){
        return pp.Puzzle3Complete();
    }
}
