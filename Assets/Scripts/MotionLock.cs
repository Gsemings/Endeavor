using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionLock : MonoBehaviour
{

    Vector3 oldPosition;
    Quaternion oldRotation;

    // Start is called before the first frame update
    void Start()
    {
        oldPosition = gameObject.transform.position;
        oldRotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = oldPosition;

        Vector3 rot = oldRotation.eulerAngles;

        float test = gameObject.transform.rotation.eulerAngles.y;

        gameObject.transform.rotation = Quaternion.Euler(rot.x, test, rot.z);
    }
}
