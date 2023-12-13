using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using TMPro;

public class DevImpactMenu : MonoBehaviour
{
    //Attributes
    public Button bt_Create, bt_Cancel;
    private SitCoM world;
    private List<SitCoM_Impact> impacts;

    public TMP_InputField tbName, tbDescription;

    // Start is called before the first frame update
    void Start()
    {
        //Add Eventhandler to buttons
        if (bt_Create != null)
        {
            bt_Create.onClick.AddListener(TaskOnClickCreate);
        }
        else if (bt_Cancel != null)
        {
            bt_Cancel.onClick.AddListener(TaskOnClickCancel);
        }

        //Find SitCoM instance
        world = GameObject.Find("SitCoM").GetComponent<SitCoM>();

        //Set impacts from SitCoM
        impacts = world.Impacts;
    }

    //Method saves impact
    void TaskOnClickCreate()
    {
        //Get input values
        string name = tbName.text;
        string description = tbDescription.text;

        Debug.Log("Input " + name + " " + description);

        //Instantiate new impact
        SitCoM_Impact newimpact = new SitCoM_Impact(name,description);

        //Add impact to list
        impacts.Add(newimpact);

        //Write impacts to file
        world.WriteImpacts();

#if UNITY_EDITOR
        if (EditorUtility.DisplayDialog("Saved", "The impact has been created.", "OK"))
        {
            SceneManager.LoadScene("SitCoM_Menu");
        }
#else
        //Go back to main menu
        SceneManager.LoadScene("SitCoM_Menu");
#endif
    }

    //Cancel button clicked
    void TaskOnClickCancel()
    {
        Debug.Log("You have clicked the cancel button");

#if UNITY_EDITOR
        if(EditorUtility.DisplayDialog("?", "Do you really want to leave and discard all changes?", "Yes", "No"))
        {
            SceneManager.LoadScene("SitCoM_Menu");
        }
#else
        //Go back to main menu
        SceneManager.LoadScene("SitCoM_Menu");
#endif
    }
}
