using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HideInactiveObjects : EditorWindow
{
    private static bool isHiddenObjects = false;
    private static bool enableForWholeHierarchy = true;
    private static List<GameObject> targetObjects = new List<GameObject>();
    
    static GameObject[] GetInactiveObjects(GameObject[] allObjects)
    {
        GameObject[] inactiveObjects = Array.FindAll(allObjects, x => (!x.activeInHierarchy && !x.activeSelf && x.name != "InternalIdentityTransform"));
        return inactiveObjects;
    }

    static void HideObjects(GameObject[] targets)
    {
        foreach (var target in targets)
        {
            target.hideFlags = HideFlags.HideInHierarchy;
        }

        isHiddenObjects = true;
    }

    static void DisplayObjects(GameObject[] targets)
    {
        foreach (var target in targets)
        {
            target.hideFlags = HideFlags.None;
        }

        isHiddenObjects = false;
    }
    
    [MenuItem("Tools/Hide Inactive Objects/Enable")]
    static void ToggleOption()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        
        if (isHiddenObjects)
        {
            DisplayObjects(GetInactiveObjects(enableForWholeHierarchy ? allObjects : targetObjects.ToArray()));            
        }
        else
        {
            HideObjects(GetInactiveObjects(enableForWholeHierarchy ? allObjects : targetObjects.ToArray()));
        }

        Menu.SetChecked("Tools/Hide Inactive Objects/Enable", isHiddenObjects);
    }
    
    [MenuItem("Tools/Hide Inactive Objects/Enable", true)]
    static bool ValidateOption()
    {
        Menu.SetChecked("Tools/Hide Inactive Objects/Enable", isHiddenObjects);
        return true;
    }
    
    [MenuItem("Tools/Hide Inactive Objects/Options")]
    public static void ShowWindow()
    {
        GetWindow<HideInactiveObjects>("Hide Inactive Objects");
    }

    bool showError = false;
    
    private void OnGUI()
    {
        enableForWholeHierarchy = EditorGUILayout.Toggle("Enable for whole hierarchy", enableForWholeHierarchy);
        
        targetObjects = targetObjects.Select((obj, i) => 
                                             (GameObject)EditorGUILayout.ObjectField($"GameObject {i + 1}", obj, typeof(GameObject), true)).ToList();
        
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("+"))
        {
            targetObjects.Add(null);
        }

        if (GUILayout.Button("-"))
        {
            targetObjects.RemoveAt(targetObjects.Count - 1);
        }

        GUILayout.EndHorizontal();

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        
        if (GUILayout.Button(isHiddenObjects ? "Hide" : "Display"))
        {
            Debug.Log($"targetObjects.Count: {targetObjects.Count()}");
            
            if (targetObjects.Count() == 0 && !enableForWholeHierarchy) showError = true;
            
            else {
                Toggle();
                showError = false;
            }
        }

        if (showError) GUILayout.Label("Error: Object を指定するか Hierarchy 全体に設定を有効化してください", new GUIStyle() {normal = new GUIStyleState() { textColor = Color.red }});
    }
}
