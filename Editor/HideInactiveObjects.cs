using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HideInactiveObjects : EditorWindow
{
    private static bool isHiddenObjects = false;
    private const string MenuPath = "Tools/Hide Inactive Objects";
    private GameObject draggedObject;
    
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
    
    [MenuItem(MenuPath + "/Enable")]
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
        Menu.SetChecked(MenuPath + "/Enable", isHiddenObjects);
    }
    
    [MenuItem(MenuPath + "/Enable", true)]
    static bool ValidateOption()
    {
        Menu.SetChecked(MenuPath + "/Enable", isHiddenObjects);
        return true;
    }

    [MenuItem(MenuPath + "/Options")]
    public static void ShowWindow()
    {
        GetWindow<HideInactiveObjects>("Hide Inactive Objects");
    }

    private void OnGUI()
    {   
        draggedObject = (GameObject)EditorGUILayout.ObjectField("Drag GameObject Here", draggedObject, typeof(GameObject), true);

        if (draggedObject != null)
        {
            
        }
    }
}
