using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //Class can be written to file
public class SitCoM_Impact
{
    //Attributes
    private string name;
    private string description;

    //These attributes are already included for possible future versions
    private int startTime;
    private int endTime;
    private string category;
    private bool started;
    private bool ended;

    //Properties
    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }

    //Constructor
    public SitCoM_Impact(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}
