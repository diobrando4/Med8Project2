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
    Outline BR;
    Outline MR;
    Outline TR;
    AudioSource AS;
    WaterPump wp;
    // Start is called before the first frame update
    void Start()
    {
        bottomRender = GetComponent<MeshRenderer>();
        BR = GetComponent<Outline>();
        AS = GetComponent<AudioSource>();
        AS.enabled = false;
        middleRender = middleWater.GetComponent<MeshRenderer>();
        MR = middleWater.GetComponent<Outline>();
        topRender = topWater.GetComponent<MeshRenderer>();
        TR = topWater.GetComponent<Outline>();
        wp = waterpump.GetComponent<WaterPump>();
        bottomRender.enabled = false;
        middleRender.enabled = false;
        topRender.enabled = false;
        BR.enabled = false;
        MR.enabled = false;
        TR.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (waterCounter > 0)
        {
            bottomRender.enabled = true;
            BR.enabled = true;
        }
        if (waterCounter > 1)
        {
            middleRender.enabled = true;
            topRender.enabled = true;
            MR.enabled = true;
            TR.enabled = true;
        }
        if (wp.ispumping)
        {
            bottomRender.enabled = true;
            middleRender.enabled = true;
            topRender.enabled = true;
            BR.enabled = true;
            MR.enabled = true;
            TR.enabled = true;

        }
        if(waterCounter<1 && !wp.ispumping)
        {
            bottomRender.enabled = false;
            middleRender.enabled = false;
            topRender.enabled = false;
            BR.enabled = false;
            MR.enabled = false;
            TR.enabled = false;
        }
        AS.enabled = FountainComplete();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "basket")
        {
            if (other.gameObject.GetComponent<BascketScript>().IHaveWater()) {
                waterCounter++;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "basket")
        {
            waterCounter--;
        }
    }

    public bool FountainComplete()
    {
        return bottomRender.enabled && middleRender.enabled && topRender.enabled;
    }
}
