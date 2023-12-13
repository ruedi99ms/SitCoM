using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Attribute
    public GameObject gmo_toolTip;

    void Start()
    {
        //Deactive tooltip object during startup
        if (gmo_toolTip != null)
        {
            gmo_toolTip.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //When mouse enters, make object visible
        if (gmo_toolTip != null)
        {
            gmo_toolTip.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //When mouse leaves, make object disappear
        if (gmo_toolTip != null)
        {
            gmo_toolTip.SetActive(false);
        }
    }
}