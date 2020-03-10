using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public LaserGame laserGame;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (laserGame.complete)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
       else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
