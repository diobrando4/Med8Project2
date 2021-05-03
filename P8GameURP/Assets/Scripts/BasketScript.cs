using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketScript : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer meshrender;
    void Start()
    {
        meshrender = GetComponent<Transform>().GetChild(0).GetComponent<MeshRenderer>();
        meshrender.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError(other.tag);
        if (other.tag == "water")
        {
            meshrender.enabled = true;
        }
    }
    public bool IHaveWater(){
        return meshrender.enabled;
    }

}
