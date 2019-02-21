using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalarProjections : MonoBehaviour
{
    [SerializeField] Transform pathCube, vectorCube, sphere;

    void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, vectorCube.position - transform.position, Color.blue);
        Debug.DrawRay(transform.position, pathCube.position - transform.position, Color.red);
    }

    void Start()
    {

    }

    void Update()
    {
        Vector3 toPathCube = pathCube.position - transform.position;
        toPathCube.Normalize();

        Vector3 toVectoCube = vectorCube.position - transform.position;

        toPathCube *= Vector3.Dot(toVectoCube, toPathCube);

        Vector3 pointOnTheLine = transform.position + toPathCube;

        sphere.position = pointOnTheLine;

        Debug.DrawLine(transform.position, sphere.position, Color.green);
        Debug.DrawLine(vectorCube.position, sphere.position, Color.white);
    }

    #region Manual Angle Between
    private void GetAngleBetweenVectors(Vector3 target)
    {
        Vector2 toTarget = target - transform.position;

        float dotProduct = Vector2.Dot(transform.up, toTarget);

        float multipliedMagnitude = Vector3.Magnitude(transform.up) * Vector3.Magnitude(toTarget);

        float angleBetween = Mathf.Acos(dotProduct / multipliedMagnitude) * Mathf.Rad2Deg;

        print(angleBetween + ", Unity: " + Vector2.Angle(transform.up, toTarget));
    }
    #endregion
}
