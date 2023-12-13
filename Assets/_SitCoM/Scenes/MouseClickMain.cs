using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MouseClickMain : MonoBehaviour
{
    //Attributes
    public Button bt_Dev, bt_Res, bt_DevImpact;

    // Start is called before the first frame update
    void Start()
    {
        //Add Eventhandler to buttons
        if (bt_Dev != null)
        {
            bt_Dev.onClick.AddListener(TaskOnClickDev);
        }
        else if(bt_Res != null)
        {
            bt_Res.onClick.AddListener(TaskOnClickRes);
        }
        else if(bt_DevImpact != null)
        {
            bt_DevImpact.onClick.AddListener(TaskOnClickDevImpact);
        }
    }

    //Go to scenario development menu
    void TaskOnClickDev()
    {
        SceneManager.LoadScene("SitCoM_DevMenu");
    }

    //Go to impact development menu
    void TaskOnClickDevImpact()
    {
        SceneManager.LoadScene("SitCoM_DevImpactMenu");
    }

    //Go to research menu
    void TaskOnClickRes()
    {
        SceneManager.LoadScene("SitCoM_ResMenu");
    }
}
