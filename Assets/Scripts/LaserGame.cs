using System.Collections.Generic;
using UnityEngine;

public class LaserGame : MonoBehaviour
{
    public GameObject[] startObjects;
    public GameObject[] reflectObjects;
    public GameObject[] splitObjects;
    public GameObject[] endObjects;

    public GameObject borderObject;
    public Vector3 initialDirection;
    public Material laserMaterial;

    public bool complete = false;

    List<GameObject> hitObjects;
    List<GameObject> lineContainers;
    List<GameObject> hitSplits;
    List<GameObject> hitEnds;

    void Start()
    {
        foreach (GameObject go in startObjects)
        {
            LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
            lineRenderer.material = laserMaterial;
        }

        hitObjects = new List<GameObject>();
        lineContainers = new List<GameObject>();
        hitSplits = new List<GameObject>();
        hitEnds = new List<GameObject>();
    }

    GameObject RunLaser(GameObject inputObject, LineRenderer lineRenderer, Vector3 startDirection)
    {
        if (inputObject == null || lineRenderer == null || startDirection == null)
        {
            return null;
        }

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, inputObject.transform.position);

        RaycastHit hit;
        Vector3 direction = startDirection;

        while (Physics.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 1), direction, out hit))
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject == inputObject)
            {
                return null;
            }

            if (hit.collider.transform.parent == borderObject.transform)
            {
                return borderObject;
            }

            if (!InArray(reflectObjects, hitObject))
            {
                return hitObject;
            }

            direction = Vector3.Reflect(direction, hit.normal);
        }
        return null;
    }

    void Update()
    {
        hitObjects.Clear();

        foreach (GameObject go in lineContainers)
        {
            Destroy(go);
        }

        foreach(GameObject go in startObjects)
        {
            hitObjects.Add(RunLaser(go, go.GetComponent<LineRenderer>(), initialDirection));
        }

        while (hitObjects.Count != 0)
        {

            if (InArray(splitObjects, hitObjects[0]) && !InArray(hitSplits.ToArray(), hitObjects[0]))
            {
                GameObject leftContainer = new GameObject();
                leftContainer.transform.parent = hitObjects[0].transform;
                LineRenderer leftRenderer = leftContainer.AddComponent<LineRenderer>();
                leftRenderer.material = laserMaterial;
                lineContainers.Add(leftContainer);
                hitObjects.Add(RunLaser(hitObjects[0], leftRenderer, Vector3.left));

                GameObject rightContainer = new GameObject();
                rightContainer.transform.parent = hitObjects[0].transform;
                LineRenderer rightRenderer = rightContainer.AddComponent<LineRenderer>();
                rightRenderer.material = laserMaterial;
                lineContainers.Add(rightContainer);
                hitObjects.Add(RunLaser(hitObjects[0], rightRenderer, Vector3.right));

                GameObject centerContainer = new GameObject();
                centerContainer.transform.parent = hitObjects[0].transform;
                LineRenderer centerRenderer = centerContainer.AddComponent<LineRenderer>();
                centerRenderer.material = laserMaterial;
                lineContainers.Add(centerContainer);
                hitObjects.Add(RunLaser(hitObjects[0], centerRenderer, initialDirection));

                hitSplits.Add(hitObjects[0]);
            }

            if (InArray(endObjects, hitObjects[0]))
            {
                hitEnds.Add(hitObjects[0]);
            }

            hitObjects.RemoveAt(0);
        }

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

        complete = true;
        foreach (GameObject go in endObjects)
        {
            if (!InArray(hitEnds.ToArray(), go))
            {
                complete = false;
            }
        }

        hitEnds.Clear();
        hitSplits.Clear();
    }

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