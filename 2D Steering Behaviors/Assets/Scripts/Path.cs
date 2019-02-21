using UnityEngine;

public class Path : MonoBehaviour
{
    [HideInInspector]
    public int numberOfPointsOnPath;

    [SerializeField]
    float width;

    LineRenderer lineRenderer;

    [HideInInspector]
    public bool pathIsSet;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        SetupPath();
    }

    private void SetupPath()
    {
        numberOfPointsOnPath = transform.childCount;
        lineRenderer.positionCount = transform.childCount;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        for (int i = 0; i < numberOfPointsOnPath; i++) lineRenderer.SetPosition(i, transform.GetChild(i).position);

        pathIsSet = true;
    }

    public Vector3 GetLineSegmentPoint(int i)
    {
        return transform.GetChild(i).position;
    }
}
