using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SitCoM_ScenarioManager : SitCoM_Object //Class inherits from SitCoM_Object
{
    //Assets
    public GameObject trafficCone;

    //Attributes
    private List<GameObject> spawnedObjects;
    private bool scenariosStarted = false;
    private bool scenariosEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        //Call start up method of SitCoM_Object
        this.StartUp();

        //Initialize class
        spawnedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check, if scenarios have not been started and have not ended
        if (!scenariosStarted && !scenariosEnded)
        {
            if (AreImpactsCurrentlyHappening())
            {
                //Check for specific scenarios
                if (IsScenarioIncluded("Construction Site"))
                {
                    //Call method for specific scenario
                    ConstructionSite();
                }

                //Set bool value
                scenariosStarted = true;

                Debug.Log("Scenarios spawned");
            }
        //Check, if scenarios have been started and have not ended
        } else if (scenariosStarted && !scenariosEnded) {
            if (HaveImpactsEnded())
            {
                //Despawn all objects
                foreach (GameObject go in spawnedObjects)
                {
                    DestroyObject(go);
                }

                //Set bool value
                scenariosEnded = true;

                Debug.Log("Scenarios despawned");
            }
        }
    }

    //Method for construction site scenario
    private void ConstructionSite()
    {
        for (int i = 0; i <= 4; i++)
        {
            //Add traffic cones
            GameObject item = Instantiate(trafficCone);
            item.transform.position = new Vector3(-513.7347f, 164.0639f, 1296.262f + i * 2);
            spawnedObjects.Add(item);
        }
    }
}
