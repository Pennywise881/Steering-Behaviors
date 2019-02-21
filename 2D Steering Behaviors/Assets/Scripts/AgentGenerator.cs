using UnityEngine;

public class AgentGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject agentPrefab;

    [Range(1, 200)]
    [SerializeField]
    int numberOfAgents;

    float maxX, maxY;

    void Start()
    {
        //get the width and the height of the main camera
        maxX = Camera.main.aspect * Camera.main.orthographicSize;
        maxY = Camera.main.orthographicSize;

        GenerateAgents();
    }

    private void GenerateAgents()
    {
        //Get the size of the camera
        //Place the agents randomly within the view of the camera

        for (int i = 0; i < numberOfAgents; i++)
        {
            GameObject agent = Instantiate(agentPrefab, Vector2.zero, Quaternion.identity);
            agent.transform.position = new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0);
            agent.transform.parent = this.transform;
        }
    }
}