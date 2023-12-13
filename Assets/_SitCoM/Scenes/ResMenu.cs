using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

using System.Text.RegularExpressions;

using TMPro;

public class ResMenu : MonoBehaviour
{
    //Attributes
    private SitCoM world;
    private List<SitCoM_Scenario> scenarios;
    private List<SitCoM_Impact> impacts;

    private List<SitCoM_Scenario> selectedScenarios;
    private List<SitCoM_Impact> selectedImpacts;
    private List<Button> buttonsScenarios;
    private List<Button> buttonsImpacts;

    public Transform contentContainerScenario, contentContainerImpact;
    public GameObject buttonPrefab;
    public Button bt_Back, bt_Run, bt_DelScenarios, bt_DelImpacts;
    public TMP_InputField input_StartTime, input_EndTime, input_SceneName;

    private int startSeconds, endSeconds;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate button lists
        buttonsScenarios = new List<Button>();
        buttonsImpacts = new List<Button>();

        //Instantiate scenario and impact lists for selection
        selectedScenarios = new List<SitCoM_Scenario>();
        selectedImpacts = new List<SitCoM_Impact>();

        //Find SitCoM instance
        world = GameObject.Find("SitCoM").GetComponent<SitCoM>();

        //Set scenarios and impacts from SitCoM
        scenarios = world.Scenarios;
        impacts = world.Impacts;

        Debug.Log("Res Menu Loaded " + scenarios.Count + " scenarios " + impacts.Count + " impacts");

        //Add Eventhandlers to buttons
        bt_Back.onClick.AddListener(BackButtonClicked);
        bt_Run.onClick.AddListener(RunButtonClicked);

        input_StartTime.onValueChanged.AddListener(delegate { OnlyNumericalInput(input_StartTime); });
        input_EndTime.onValueChanged.AddListener(delegate { OnlyNumericalInput(input_EndTime); });

        bt_DelScenarios.onClick.AddListener(DelScenariosButtonClicked);
        bt_DelImpacts.onClick.AddListener(DelImpactsButttonClicked);

        //Display scenarios and impacts
        AddScenariosToScrollView();
        AddImpactsToScrollView();
    }

    //Back button clicked
    private void BackButtonClicked()
    {
#if UNITY_EDITOR
        if (EditorUtility.DisplayDialog("Back?", "Do you want to go back to the main menu?", "Yes","No"))
        {
            SceneManager.LoadScene("SitCoM_Menu");
        }
#else
        //Go back to main menu
        SceneManager.LoadScene("SitCoM_Menu");
#endif
    }

    //Run button clicked
    private void RunButtonClicked()
    {
        string sceneName = input_SceneName.text;

        try
        {
            //Parse start time
            startSeconds = int.Parse(input_StartTime.text);
        }
        catch (System.Exception ex)
        {
            //If error, set start time to 0s
            startSeconds = 0;
        }

        try
        {
            //Parse end time
            endSeconds = int.Parse(input_EndTime.text);
        }
        catch (System.Exception ex)
        {
            //If error, set end time to 0s
            endSeconds = int.MaxValue;
        }

        //Swap start and end time, if end time is before start time
        if (endSeconds < startSeconds)
        {
            int tmp = startSeconds;
            startSeconds = endSeconds;
            endSeconds = tmp;
        }

        //Pass selected impacts and scenarios to SitCoM
        world.SelectedScenarios = selectedScenarios;
        world.SelectedImpacts = selectedImpacts;

        //Pass start and end time to SitCoM
        world.StartSeconds = startSeconds;
        world.EndSeconds = endSeconds;

        //Load scene
        SceneManager.LoadScene(sceneName);        
        input_SceneName.text = "Scene not found";       

    }

    //Method adds scenarios to the respective scroll viewer
    private void AddScenariosToScrollView()
    {
        //Iterate over all scenarios
        foreach (SitCoM_Scenario s in scenarios)
        {
            //Create button for scenario
            GameObject item = Instantiate(buttonPrefab);
            //Add button to scroll viewer
            item.transform.SetParent(contentContainerScenario);

            //Find label and description in child components
            foreach (TMP_Text text in item.GetComponentsInChildren<TMP_Text>())
            {
                if (text.gameObject.name == "Label")
                {
                    //Set label text to name
                    text.text = s.Name;
                    text.enableAutoSizing = true;
                }
                else
                {
                    //Set tooltip text to description
                    text.text = s.Description;
                }
            }

            //Add button to list
            buttonsScenarios.Add(item.GetComponent<Button>());

            //Add Eventhandler to button
            item.GetComponent<Button>().onClick.AddListener(delegate { ScenarioButtonClicked(item.GetComponent<Button>()); });
        }

        //Scroll to top
        contentContainerScenario.parent.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
    }

    //Method adds impacts to the respective scroll viewer
    private void AddImpactsToScrollView()
    {
        //Iterate over all imapcts
        foreach (SitCoM_Impact i in impacts)
        {
            //Create button for impact
            GameObject item = Instantiate(buttonPrefab);

            //Find label and description in child components
            foreach(TMP_Text text in item.GetComponentsInChildren<TMP_Text>())
            {
                if(text.gameObject.name == "Label")
                {
                    //Set label text to name
                    text.text = i.Name;
                    text.enableAutoSizing = true;
                }
                else
                {
                    //Set tooltip text to description
                    text.text = i.Description;
                }
            }
            
            //Add button to scroll viewer
            item.transform.SetParent(contentContainerImpact);

            //Add button to list
            buttonsImpacts.Add(item.GetComponent<Button>());

            //Add Eventhandler to button
            item.GetComponent<Button>().onClick.AddListener(delegate { ImpactButtonClicked(item.GetComponent<Button>()); });
        }

        contentContainerImpact.parent.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
    }

    //Clear scenario viewer
    private void ClearScenarioScrollView()
    {
        //Delete all scenarios from scroll viewer
        foreach (Transform child in contentContainerScenario)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    //Clear impact viewer
    private void ClearImpactScrollView()
    {
        //Delete all impacts from impact viewer
        foreach(Transform child in contentContainerImpact)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    //Delete scenarios button clicked
    private void DelScenariosButtonClicked()
    {
        //Is at least 1 scenario selected?
        if (selectedScenarios.Count > 0)
        {
#if UNITY_EDITOR
            string s = "Are you sure, you want to delete the scenarios ";

            foreach(SitCoM_Scenario sc in selectedScenarios)
            {
                s += "\"" + sc.Name + "\", ";
            }

            s = s.Substring(0, s.Length - 2);
            s += "?";


            if (EditorUtility.DisplayDialog("Attention", s, "Yes", "No"))
            {
#endif
                //Remove selected scenarios from list
                foreach(SitCoM_Scenario sc in selectedScenarios)
                {
                    scenarios.Remove(sc);
                }

                //Clear lists
                selectedScenarios.Clear();
                buttonsScenarios.Clear();

                //Write and load scenarios
                world.WriteScenarios();
                scenarios = world.LoadScenarios();

                //Add scenarios again
                ClearScenarioScrollView();
                AddScenariosToScrollView();
#if UNITY_EDITOR
            }
#endif
        }
        else
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog("Info", "You have no scenario(s) selected.", "OK"))
            {
                return;
            }
#else
            return;
#endif
        }
    }
    
    //Delete impacts button clicked
    private void DelImpactsButttonClicked()
    {
#if UNITY_EDITOR
        string s = "Are you sure, you want to delete the impacts ";

        foreach(SitCoM_Impact im in selectedImpacts)
        {
            s += "\"" + im.Name + "\", ";
        }

        s = s.Substring(0, s.Length - 2);
        s += "?";

        if(EditorUtility.DisplayDialog("Attention", s, "Yes", "No"))
        {
#endif
            //Remove selected impacts from list
            foreach(SitCoM_Impact im in selectedImpacts)
            {
                impacts.Remove(im);
            }

            //Clear lists
            selectedImpacts.Clear();
            buttonsImpacts.Clear();

            //Write and load impacts
            world.WriteImpacts();
            impacts = world.LoadImpacts();
            
            //Add impacts again
            ClearImpactScrollView();
            AddImpactsToScrollView();
#if UNITY_EDITOR
        }
#endif


    }

    //Eventhandler for scenario buttons
    private void ScenarioButtonClicked(Button trigger)
    { 
        //Find button in list and get corresponding scenario
        int indexOfScenario = buttonsScenarios.IndexOf(trigger);
        SitCoM_Scenario scenario = scenarios[indexOfScenario];

        Debug.Log("Scenario clicked " + indexOfScenario);

        //Is scenario not selected?
        if (!selectedScenarios.Contains(scenario))
        {
            //Add scenario to selection
            selectedScenarios.Add(scenario);

            //Change color of button
            Color c = Color.magenta;
            c.a = 0.3f;
            trigger.GetComponent<Image>().color = c;
        }
        else
        {
            //Remove scenario from selection
            selectedScenarios.Remove(scenario);

            //Change color of button
            trigger.GetComponent<Image>().color = Color.white;
        }

    }

    //Eventhandler for impact buttons
    private void ImpactButtonClicked(Button trigger)
    {
        //Find button in list and get corresponding impact
        int indexOfImpact = buttonsImpacts.IndexOf(trigger);
        SitCoM_Impact impact = impacts[indexOfImpact];

        Debug.Log("Impact clicked " + indexOfImpact);

        //Is impact not selected?
        if (!selectedImpacts.Contains(impact))
        {
            //Add impact to selection
            selectedImpacts.Add(impact);

            //Change color of button
            Color c = Color.magenta;
            c.a = 0.3f;
            trigger.GetComponent<Image>().color = c;
        }
        else
        {
            //Remove impact from selection
            selectedImpacts.Remove(impact);

            //Change color of button
            trigger.GetComponent<Image>().color = Color.white;
        }

    }

    //Helper method to prevent non-numerical input (in time fields)
    private void OnlyNumericalInput(TMP_InputField field)
    {
        //Debug.Log("Value changed");

        //Get input
        string input = field.text;

        //Define possible characters
        Regex rgx = new Regex("[^0-9]");

        //Delete characters that are not allowed
        string output = rgx.Replace(input, "");

        if (output.Length > 0)
        {
            try
            {
                //Parse output to integer
                int.Parse(output);
            }
            catch (System.OverflowException ex)
            {
                //If max value of int is exceeded, set output to max value
                output = "" + int.MaxValue;
            }
        }

        //Set text
        field.text = output;
    }
}
