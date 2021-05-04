using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2Controller : MonoBehaviour
{
    public GameObject basketGroup;
    public GameObject waterpump;
    public GameObject fountain;
    private WaterRender wr;
    // Start is called before the first frame update
    void Start()
    {
        wr = fountain.GetComponent<WaterRender>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startPuzzle2(bool start){
        wr.RenderWater(!start);
        if (!puzzle2Complete()) {
            basketGroup.SetActive(start);
            waterpump.SetActive(start);
        }
    }
    public bool puzzle2Complete(){
        Debug.LogError("PUZZLE 2 COMPLETE !!");
        return wr.FountainComplete();
    }
}
