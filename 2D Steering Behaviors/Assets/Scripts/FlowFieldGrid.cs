using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldGrid : MonoBehaviour
{
    [SerializeField]
    FlowFieldFollowing flowFieldFollowing;

    [SerializeField]
    GameObject arrowPrefab;

    int rows, columns;

    [HideInInspector]
    public bool fieldIsSet;

    Vector2[,] arrowDirections/*, arrowPositions */;

    private void Start()
    {
        rows = ((int)Camera.main.orthographicSize / 10) * 2;
        float temp = ((Camera.main.orthographicSize * Camera.main.aspect) / 10) * 2;
        columns = (int)temp + 1;

        Camera.main.transform.position = new Vector3(0, 0, -10);
        CreateField();
        Camera.main.transform.position = new Vector3((columns / 2) * 10, Camera.main.orthographicSize, -10);
    }

    //place the arrows on the screen
    private void CreateField()
    {
        float x = 5;
        float y = 5;

        flowFieldFollowing.arrowDirections = new Vector2[rows, columns];
        flowFieldFollowing.arrowPositions = new Vector2[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject arrow = Instantiate(arrowPrefab, new Vector2(x, y),
                    Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(-180, 1))), transform);

                if (i == 0 && j == 0) arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));

                flowFieldFollowing.arrowDirections[i, j] = arrow.transform.up;
                flowFieldFollowing.arrowPositions[i, j] = arrow.transform.position;

                x += 10;
            }
            x = 5;
            y += 10;
        }

        flowFieldFollowing.gameObject.transform.position = flowFieldFollowing.arrowPositions[UnityEngine.Random.Range(0, flowFieldFollowing.arrowPositions.GetLength(0)), UnityEngine.Random.Range(0, flowFieldFollowing.arrowPositions.GetLength(1))];
        fieldIsSet = true;
    }
}