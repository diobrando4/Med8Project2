using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    MeshRenderer mr;
    MeshCollider mc;
    public int ECC = 0;
    private bool isTrigged;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mc = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 60 * Time.deltaTime, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"){
            isTrigged = true;
            ECC = 1;
            mr.enabled = false;
            mc.enabled = false;
        }
    }
    public bool IamTriggered(){
        return isTrigged;
    }
}
