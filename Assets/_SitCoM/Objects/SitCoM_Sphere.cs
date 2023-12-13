using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitCoM_Sphere : SitCoM_Object
{
    // Start is called before the first frame update
    void Start()
    {
        this.StartUp();

        DestroyObject(this);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    /*protected override void PerformAction()
    {
        
        if (AreImpactsCurrentlyHappening())
        {
            Debug.Log("Sphere cannot do stuff :-(");
        }
        else
        {
            Debug.Log("Sphere did some stuff :-)");
        }
        
    }*/
    
}
