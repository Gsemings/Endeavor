using System.Collections.Generic;
using UnityEngine;

public class LaserGame : MonoBehaviour
{
    public GameObject[] startObjects;
    public GameObject reflectObject;
    public GameObject splitObject;
    public GameObject[] endObjects;
    public GameObject borderObject;

    public int pointerLength = 10;

    public Material laserMaterial;

    public Vector3 initialDirection;

    List<GameObject> hitObjects;
    List<GameObject> lineContainers;
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

            if (hitObject != reflectObject)
            {
                return hitObject;
            }

            direction = Vector3.Reflect(direction, hit.normal);
        }

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, (lineRenderer.GetPosition(lineRenderer.positionCount - 2) + direction * pointerLength));

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

            if (hitObjects[0] == splitObject)
            {
                GameObject leftContainer = new GameObject();
                leftContainer.transform.parent = splitObject.transform;
                LineRenderer leftRenderer = leftContainer.AddComponent<LineRenderer>();
                leftRenderer.material = laserMaterial;
                lineContainers.Add(leftContainer);
                hitObjects.Add(RunLaser(splitObject, leftRenderer, Vector3.left));

                GameObject rightContainer = new GameObject();
                rightContainer.transform.parent = splitObject.transform;
                LineRenderer rightRenderer = rightContainer.AddComponent<LineRenderer>();
                rightRenderer.material = laserMaterial;
                lineContainers.Add(rightContainer);
                hitObjects.Add(RunLaser(splitObject, rightRenderer, Vector3.right));

                GameObject centerContainer = new GameObject();
                centerContainer.transform.parent = splitObject.transform;
                LineRenderer centerRenderer = centerContainer.AddComponent<LineRenderer>();
                centerRenderer.material = laserMaterial;
                lineContainers.Add(centerContainer);
                hitObjects.Add(RunLaser(splitObject, centerRenderer, Vector3.forward));
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

        hitEnds.Clear();
    }

    bool InArray(GameObject[] array, GameObject key)
    {
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