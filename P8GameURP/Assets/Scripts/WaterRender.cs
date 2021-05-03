using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRender : MonoBehaviour
{

    int waterCounter;
    public GameObject middleWater;
    public GameObject topWater;
    public GameObject waterpump;
    MeshRenderer bottomRender;
    MeshRenderer middleRender;
    MeshRenderer topRender;
    WaterPump wp;
    // Start is called before the first frame update
    void Start()
    {
        bottomRender = GetComponent<MeshRenderer>();
        middleRender = middleWater.GetComponent<MeshRenderer>();
        topRender = topWater.GetComponent<MeshRenderer>();
        wp = waterpump.GetComponent<WaterPump>();
        bottomRender.enabled = false ;
        middleRender.enabled = false;
        topRender.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(waterCounter>0){
            bottomRender.enabled = true;
        }
        if(waterCounter>1){
            middleRender.enabled = true;
            topRender.enabled = true;
        }
        if(wp.ispumping){
            bottomRender.enabled = true;
            middleRender.enabled = true;
            topRender.enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="basket"){
            waterCounter++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "basket")
        {
            waterCounter--;
        }
    }

    public bool FountainComplete(){
        return bottomRender.enabled && middleRender.enabled && topRender.enabled;
    }
}
