using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GAgentVisual))]
[CanEditMultipleObjects]
public class GAgentVisualEditor : Editor
{


  void OnEnable()
  {

  }

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();
    serializedObject.Update();
    GAgentVisual agent = (GAgentVisual)target;
    GUILayout.Label("Name: " + agent.name);
    GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GAgent>().currentAction);
    GUILayout.Label("Actions: ");
    foreach (GAction a in agent.gameObject.GetComponent<GAgent>().actions)
    {
      string pre = "";
      string eff = "";

      foreach (KeyValuePair<string, int> p in a.preConditions)
        pre += p.Key + ", ";
      foreach (KeyValuePair<string, int> e in a.effects)
        eff += e.Key + ", ";

      GUILayout.Label("====  " + a.actionName + "(" + pre + ")(" + eff + ")");
    }
    GUILayout.Label("Goals: ");
    foreach (KeyValuePair<SubGoal, int> g in agent.gameObject.GetComponent<GAgent>().goals)
    {
      GUILayout.Label("---: ");
      foreach (KeyValuePair<string, int> sg in g.Key.sgoals)
        GUILayout.Label("=====  " + sg.Key);
    }
    GUILayout.Label("Beliefs: ");
    Dictionary<string, int> beliefs = agent.gameObject.GetComponent<GAgent>().beliefs.GetStates();
    GUILayout.Label("---: ");
    foreach (KeyValuePair<string, int> b in beliefs)
      GUILayout.Label("=====  " + b.Key);
    GUILayout.Label("Inventory: ");
    List<GameObject> inventory = agent.gameObject.GetComponent<GAgent>().inventory.GetItems();
    GUILayout.Label("---: ");
    foreach (GameObject i in inventory)
      GUILayout.Label("=====  " + i);
    serializedObject.ApplyModifiedProperties();
  }
}