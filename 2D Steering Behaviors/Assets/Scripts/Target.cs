using UnityEngine;

public class Target : MonoBehaviour
{
    Camera mainCamera;

    [SerializeField]
    float radius;

    public float Radius { get { return radius; } }

    [SerializeField]
    bool positionToMouse;

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(transform.position, radius);
    // }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (positionToMouse) SetPositionTowardsMouseCursor();
    }

    private void SetPositionTowardsMouseCursor()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }
}
