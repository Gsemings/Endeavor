using System.Collections.Generic;
using UnityEngine;

public class LaserGame : MonoBehaviour
{
    // Game setup variables
    public GameObject[] startObjects;
    public GameObject[] reflectObjects;
    public GameObject[] splitObjects;
    public GameObject[] endObjects;
    public GameObject borderObject;
    public Vector3 initialDirection;
    public Material laserMaterial;

    // Level complete indicator
    public bool complete = false;

    // Update tracking variables
    List<GameObject> hitObjects;
    List<GameObject> lineContainers;
    List<GameObject> hitSplits;
    List<GameObject> hitEnds;

    // Initiates the level
    void Start()
    {
        // Initialize line renderer for each starting object
        foreach (GameObject go in startObjects)
        {
            LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
            lineRenderer.material = laserMaterial;
        }

        // Initialize lists for each update tracker
        hitObjects = new List<GameObject>();
        lineContainers = new List<GameObject>();
        hitSplits = new List<GameObject>();
        hitEnds = new List<GameObject>();
    }

    // Executes a laser and returns the final hit object
    GameObject RunLaser(GameObject inputObject, LineRenderer lineRenderer, Vector3 startDirection)
    {
        // End script if invalid input
        if (inputObject == null || lineRenderer == null || startDirection == null)
        {
            return null;
        }

        // Reset line renderer
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, inputObject.transform.position);

        RaycastHit hit;
        Vector3 direction = startDirection;

        // Project laser while an object is hit
        while (Physics.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 1), direction, out hit))
        {
            // Add collision to line renderer path
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

            GameObject hitObject = hit.collider.gameObject;

            // If input is hit, remove last path and return null
            if (hitObject == inputObject)
            {
                if (InArray(startObjects, hitObject) || InArray(splitObjects, hitObject))
                {
                    lineRenderer.positionCount--;
                }
                return null;
            }

            // If border it hit, return border prefab
            if (hit.collider.transform.parent == borderObject.transform)
            {
                return borderObject;
            }

            // If non reflecting object hit, return it
            if (!InArray(reflectObjects, hitObject))
            {
                // Check for correct splitter direction before returning it
                if (InArray(splitObjects, hitObject))
                {
                    if (hit.normal == hit.collider.transform.forward)
                    {
                        return hitObject;
                    }
                    else
                    {
                        return null;
                    }
                    
                }
                else
                {
                    return hitObject;
                }
            }

            // Determine next raycast direction
            direction = Vector3.Reflect(direction, hit.normal);
        }

        // Should never be reached as long as border exists
        return null;
    }

    // Refreshes lasers
    void Update()
    {
        // Reset update trackers
        hitObjects.Clear();
        hitSplits.Clear();
        hitEnds.Clear();

        // Remove all non-starting line renderers
        foreach (GameObject go in lineContainers)
        {
            Destroy(go);
        }

        // Run line renderer for each start object
        foreach(GameObject go in startObjects)
        {
            hitObjects.Add(RunLaser(go, go.GetComponent<LineRenderer>(), initialDirection));
        }

        // While a collision needs to be processed, process it
        while (hitObjects.Count != 0)
        {
            // Run split object process if not previously executed
            if (InArray(splitObjects, hitObjects[0]) && !InArray(hitSplits.ToArray(), hitObjects[0]))
            {
                // Project left laser
                GameObject leftContainer = new GameObject();
                leftContainer.transform.parent = hitObjects[0].transform;
                LineRenderer leftRenderer = leftContainer.AddComponent<LineRenderer>();
                leftRenderer.material = laserMaterial;
                lineContainers.Add(leftContainer);
                hitObjects.Add(RunLaser(hitObjects[0], leftRenderer, -1 * hitObjects[0].transform.right));

                // Project right lasers
                GameObject rightContainer = new GameObject();
                rightContainer.transform.parent = hitObjects[0].transform;
                LineRenderer rightRenderer = rightContainer.AddComponent<LineRenderer>();
                rightRenderer.material = laserMaterial;
                lineContainers.Add(rightContainer);
                hitObjects.Add(RunLaser(hitObjects[0], rightRenderer, hitObjects[0].transform.right));

                // Project rear laser
                GameObject centerContainer = new GameObject();
                centerContainer.transform.parent = hitObjects[0].transform;
                LineRenderer centerRenderer = centerContainer.AddComponent<LineRenderer>();
                centerRenderer.material = laserMaterial;
                lineContainers.Add(centerContainer);
                hitObjects.Add(RunLaser(hitObjects[0], centerRenderer, -1 * hitObjects[0].transform.forward));

                // Track already hit splitters
                hitSplits.Add(hitObjects[0]);
            }

            // Keep trach of hit ends
            if (InArray(endObjects, hitObjects[0]))
            {
                hitEnds.Add(hitObjects[0]);
            }

            // Remove processed hit
            hitObjects.RemoveAt(0);
        }

        // Update status for each end object
        foreach (GameObject go in endObjects)
        {
            if (InArray(hitEnds.ToArray(), go))
            {
                go.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                go.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        // Check if level is complete
        complete = true;
        foreach (GameObject go in endObjects)
        {
            if (!InArray(hitEnds.ToArray(), go))
            {
                complete = false;
            }
        }
    }

    // Helper to determine if object is in array
    bool InArray(GameObject[] array, GameObject key)
    {
        if (array == null || key == null)
        {
            return false;
        }

        foreach (GameObject obj in array)
        {
            if (obj == key)
            {
                return true;
            }
        }

        return false;
    }
}