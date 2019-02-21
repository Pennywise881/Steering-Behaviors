using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WallAvoidance))]
public class WireArc : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.white;

        WallAvoidance agent = (WallAvoidance)target;

        Handles.DrawWireArc(agent.transform.position, agent.transform.forward, agent.transform.up, 360, agent.radius);
    }
}
