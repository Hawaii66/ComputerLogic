using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicked : MonoBehaviour
{
    public enum objectType { None, Switch, Lamp, Wire, Gate }
    public objectType type;
    public GameObject typeObj;
    public void OnClick()
    {
        Mouse.m.follow = true;
        if (typeObj == null)
        {
            Mouse.m.SetObject(gameObject,false);
        }
        else
        {
            Mouse.m.SetObject(typeObj,true);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject == gameObject) 
                { 
                    OnClick();
                }
            }
        }
    }

    public static bool ReleaseOnboard(objectType newType)
    {
        if(newType == objectType.Lamp)
        {
            return true;
        }
        if(newType == objectType.Switch)
        {
            return true;
        }
        if(newType == objectType.Gate)
        {
            return true;
        }

        return false;
    }
}
