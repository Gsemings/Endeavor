using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGame : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject startObject;
    public GameObject endObject;

    // Start is called before the first frame update
    void Start()
    {
        // Warn about missing variable
        Debug.Assert(lineRenderer != null && startObject != null && endObject != null,
            "LaserGame is missing an instance variable!");
    }

    // Update is called once per frame
    void Update()
    {
        // Don't run if variable is missing
        if (lineRenderer == null || startObject == null || endObject == null)
        {
            return;
        }

        // Begin by rendering point 0 at the starting object
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startObject.transform.position);

        // Store the hit and the current laser direction
        RaycastHit hit;
        Vector3 direction = Vector3.forward;

        // Bounce the laser for every object that it hits
        while (Physics.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 1), direction, out hit))
        {
            // Draw new line to the collision point
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

            // Determine next direction to check
            direction = Vector3.Reflect(direction, hit.normal);

            // Check if goal is hit
            if (hit.collider.gameObject == endObject)
            {
                endObject.GetComponent<Renderer>().material.color = Color.green;
                return;
            } else
            {
                endObject.GetComponent<Renderer>().material.color = Color.red;
            }

            // Check the next direction
            Physics.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 1), direction, out hit);
        }

        //// Add tail to the last hit object
        //lineRenderer.positionCount++;
        //lineRenderer.SetPosition(lineRenderer.positionCount - 1, direction);
    }
}