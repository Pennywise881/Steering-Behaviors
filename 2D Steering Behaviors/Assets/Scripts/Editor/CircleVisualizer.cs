using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Wander))]
public class CircleVisualizer : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.green;

        Wander agent = (Wander)target;

        Vector3 circleCenter = agent.transform.position + (agent.transform.up * agent.maxSeeAhead);

        Handles.DrawSolidDisc(circleCenter, agent.transform.forward, 0.5f);
        Handles.DrawWireArc(circleCenter, agent.transform.forward, agent.transform.up, 360, agent.radius);
        Handles.DrawLine(agent.transform.position, circleCenter);
    }
}
