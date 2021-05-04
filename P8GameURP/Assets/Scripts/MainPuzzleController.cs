using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
   
    void Start()
    {
        p1c = Puzzle1Controller.GetComponent<Puzzle1Controller>();
        p2c = Puzzle2Controller.GetComponent<Puzzle2Controller>();
        p3c = Puzzle3Controller.GetComponent<Puzzle3Controller>();
        startP1 = true;
    }

    // Update is called once per frame
    void Update()
    {
        p1c.isActive = startP1 && !p1c.puzzle1Complete();
        startP2 = p1c.puzzle1Complete() && !p2c.puzzle2Complete();
        startP3 = p2c.puzzle2Complete();

        p2c.startPuzzle2(startP2);
        p3c.startPuzzle3(startP3);
       
        Debug.LogError("puzzle 1 is active " + p1c.isActive + " puzzle 2 is active " + startP2 + " puzzle 3 is active " + startP3);
    }
}
