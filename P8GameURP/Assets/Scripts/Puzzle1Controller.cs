using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Controller : MonoBehaviour
{
    public int collection=0;
    public bool isActive;
    public GameObject puzzleObjectsGroup;
// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        puzzleObjectsGroup.SetActive(isActive);
        if(puzzle1Complete()) { Debug.LogError("PUZZLE 1 COMPLETE !!! "); }
        Debug.LogError(collection);
    }

    public bool puzzle1Complete(){
        return collection >4;
    }

}
