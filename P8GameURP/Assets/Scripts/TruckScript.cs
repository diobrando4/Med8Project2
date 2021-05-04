using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour
{
    public Transform p3Pos;
    public bool start;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start){
            transform.position = Vector3.Lerp(transform.position, p3Pos.position, Time.deltaTime * 0.25f);
        }
    }
}
