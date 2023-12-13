using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Timers;

public abstract class SitCoM_Object : MonoBehaviour
{
    //Attributes
    public SitCoM world;

    protected double intervalSeconds;
    private DateTime systemTimeStartUp;

    private List<SitCoM_Impact> impacts;
    private List<SitCoM_Scenario> scenarios;

    private DateTime impactStartTime;
    private int impactStartTimeSeconds;
    private DateTime impactEndTime;
    private int impactEndTimeSeconds;
    
    protected bool impactsRunning = false;
    private bool impactsEnded = false;

    //Method for initializing the class
    protected void StartUp()
    {
        //Connect the SitCoM class
        world = GameObject.Find("SitCoM").GetComponent<SitCoM>();

        //Set interval for scenario and impact checks
        this.intervalSeconds = 0.1;

        //Initialize list
        impacts = new List<SitCoM_Impact>();
        
        //Request information from SitCoM
        this.RetrieveImpactInformation();

        //Set up the timer for scenario and impact checks
        this.SetUpTimer(intervalSeconds);
    }

    //Eventhandler
    private void TimeEvent(object source, ElapsedEventArgs e)
    {
        //The code prevents impacts from happening over and over again
        //Have impacts ended?
        if (impactEndTime <= DateTime.Now && impactsRunning && !impactsEnded)
        {
            impactsEnded = true;
        }
        //Are impacts already running?
        else if (impactStartTime <= DateTime.Now && !impactsRunning && !impactsEnded)
        {
            impactsRunning = true;
        }
    }

    //Preparing the timer for scenario and impact checks
    private void SetUpTimer(double intervalSeconds)
    {
        Timer myTimer = new Timer(); //Initialize timer
        myTimer.Interval = intervalSeconds * 1000; //1000 ms is one second
        myTimer.Elapsed += new ElapsedEventHandler(TimeEvent); //Add eventhandler
        myTimer.Start(); //Start timer

        systemTimeStartUp = DateTime.Now; //Store start time
        impactStartTime = systemTimeStartUp.AddSeconds(impactStartTimeSeconds); //Calculate start time of impacts
        impactEndTime = systemTimeStartUp.AddSeconds(impactEndTimeSeconds); //Calculate end time of impacts
    }

    //Method requests information from SitCoM
    private void RetrieveImpactInformation()
    {
        //Typecasting is required, as the list consists of ínstances of the object-class
        List<object> retrieve = world.SendInformationPackage();
        impacts = (List<SitCoM_Impact>)retrieve[0];
        scenarios = (List<SitCoM_Scenario>)retrieve[1];

        impactStartTimeSeconds = int.Parse(((string)retrieve[2]).Split(':')[1]);
        impactEndTimeSeconds = int.Parse(((string)retrieve[3]).Split(':')[1]);
    }

    //Search for impact in list (by instance)
    protected bool IsImpactIncluded(SitCoM_Impact impact)
    {
        return impacts.Contains(impact);
    }

    //Search for impact in list (by name)
    protected bool IsImpactIncluded(string name)
    {
        foreach(SitCoM_Impact i in impacts)
        {
            if (i.Name.Equals(name))
            {
                return true;
            }
        }

        return false;
    }

    //Search for scenario in list (by instance)
    protected bool IsScenarioIncluded(SitCoM_Scenario scenario)
    {
        return scenarios.Contains(scenario);
    }

    //Search for scenario in list (by name)
    protected bool IsScenarioIncluded(string name)
    {
        foreach(SitCoM_Scenario s in scenarios)
        {
            if (s.Name.Equals(name))
            {
                return true;
            }
        }

        return false;
    }

    //Get scenario instance by name
    protected SitCoM_Scenario GetScenario(string name)
    {
        foreach (SitCoM_Scenario s in scenarios)
        {
            if (s.Name.Equals(name))
            {
                return s;
            }
        }

        return null;
    }

    protected bool AreImpactsCurrentlyHappening()
    {
        return impactsRunning && !impactsEnded;
    }

    protected bool HaveImpactsEnded()
    {
        return impactsRunning && impactsEnded;
    }
}
