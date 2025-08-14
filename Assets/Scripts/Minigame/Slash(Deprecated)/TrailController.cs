using UnityEngine;

public class TrailController : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        mainCamera = Camera.main;

        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            trailRenderer.emitting = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            trailRenderer.emitting = false;
        }
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);  // Convert mouse position in screen space to world space
        mousePos.z = 0;
        transform.position = mousePos;
    }
}
