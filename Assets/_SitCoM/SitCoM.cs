using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SitCoM : MonoBehaviour
{
    //Attributes
    private List<SitCoM_Impact> impacts, selectedImpacts;
    private List<SitCoM_Scenario> scenarios, selectedScenarios;

    private int startSeconds, endSeconds;

    //Properties
    public List<SitCoM_Impact> Impacts { get => impacts; set => impacts = value; }
    public List<SitCoM_Scenario> Scenarios { get => scenarios; set => scenarios = value; }

    public List<SitCoM_Impact> SelectedImpacts { get => selectedImpacts; set => selectedImpacts = value; }
    public List<SitCoM_Scenario> SelectedScenarios { get => selectedScenarios; set => selectedScenarios = value; }

    public int StartSeconds { get => startSeconds; set => startSeconds = value; }
    public int EndSeconds { get => endSeconds; set => endSeconds = value; }

    // Start is called before the first frame update
    void Start()
    {
        //If SitCoM object already exists -> destroy yourself
        if(GameObject.Find("SitCoM").GetComponent<SitCoM>() != this)
        {
            DestroyObject(this);
            Debug.Log("Second SitCoM has been destroyed");
            return;
        }

        Debug.Log("This is SitCoM");

        //Prevent destruction during scene change
        DontDestroyOnLoad(transform.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        //Retrieve scenarios and impacts from files (or initialize lists)
        LoadScenarios();
        LoadImpacts();

        Debug.Log("SitCoM Loaded " + scenarios.Count + " scenarios " + impacts.Count + " impacts");
    }

    public List<SitCoM_Scenario> LoadScenarios()
    {
        try
        {
            //Load scenario list from file
            using (FileStream fs = File.Open("scenarios.sitcom", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                scenarios = (List<SitCoM_Scenario>)bf.Deserialize(fs);

                fs.Close();
            }
        }
        //Error occurred?
        catch (System.Exception ex)
        {
            //Initialize list
            scenarios = new List<SitCoM_Scenario>();

            //Write list to file
            using (FileStream fs = File.Create("scenarios.sitcom"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, scenarios);
                fs.Close();

                Debug.Log("Empty scenario file created");
            }
        }

        //Sort list by scenario name
        scenarios = scenarios.OrderBy(scenario => scenario.Name).ToList();

        return scenarios;
    }

    public List<SitCoM_Impact> LoadImpacts()
    {
        try
        {
            //Load impact list from file
            using (FileStream fs = File.Open("impacts.sitcom", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                impacts = (List<SitCoM_Impact>)bf.Deserialize(fs);

                fs.Close();
            }
        }
        //Error occurred?
        catch (System.Exception ex)
        {
            //Initialize list
            impacts = new List<SitCoM_Impact>();

            //Write list to file
            using (FileStream fs = File.Create("impacts.sitcom"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, impacts);
                fs.Close();

                Debug.Log("Empty impact file created");
            }
        }

        //Sort list by impact name
        impacts = impacts.OrderBy(impact => impact.Name).ToList();

        return impacts;
    }

    public void WriteScenarios()
    {
        //Sort list by scenario name
        scenarios = scenarios.OrderBy(scenario => scenario.Name).ToList();

        //Write list to file
        using (FileStream fs = File.Create("scenarios.sitcom"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, scenarios);

            fs.Close();
        }
    }

    public void WriteImpacts()
    {
        //Sort list by impact name
        impacts = impacts.OrderBy(impact => impact.Name).ToList();

        //Write list to file
        using (FileStream fs = File.Create("impacts.sitcom"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, impacts);

            fs.Close();
        }
    }



    public List<object> SendInformationPackage()
    {
        //Initialize list
        List<object> output = new List<object>();

        //Add information to list
        output.Add(selectedImpacts);
        output.Add(selectedScenarios); 
        output.Add($"StartTime:{startSeconds}:seconds");
        output.Add($"EndTime:{endSeconds}:seconds");
        
        //Return list
        return output;
    }

    //Eventhandler for scene change
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SitCoM has changed the scene °o°");

        //Position SitCoM outside the field of view
        this.transform.position = new Vector3(50000, 50000, 50000);
    }
}
