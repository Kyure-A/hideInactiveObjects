using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HideInactiveObjects
{
    private static bool isHiddenObjects = false;
    private const string MenuPath = "Tools/Hide Inactive Objects";

    static GameObject[] GetInactiveObjects()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        GameObject[] inactiveObjects = Array.FindAll(allObjects, x => (!x.activeInHierarchy && !x.activeSelf));
        return inactiveObjects;
    }

    static void HideObjects(GameObject[] targets)
    {
        foreach (var target in targets)
        {
            target.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    static void DisplayObjects(GameObject[] targets)
    {
        foreach (var target in targets)
        {
            target.hideFlags = HideFlags.None;
        }
    }
    
    [MenuItem(MenuPath)]
    static void ToggleOption()
    {
        if (isHiddenObjects)
        {
            DisplayObjects(GetInactiveObjects());            
        }
        else
        {
            HideObjects(GetInactiveObjects());
        }
        
        isHiddenObjects = !isHiddenObjects;
        Menu.SetChecked(MenuPath, isHiddenObjects);
    }
    
    [MenuItem(MenuPath, true)]
    static bool ValidateOption()
    {
        Menu.SetChecked(MenuPath, isHiddenObjects);
        return true;
    }
}
