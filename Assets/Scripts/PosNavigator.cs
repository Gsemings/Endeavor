using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PosNavigator : MonoBehaviour
{
    public Object scene;
    public GameObject one;
    public GameObject two;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(one.transform.position, two.transform.position));

        if (Vector3.Distance(one.transform.position, two.transform.position) < 1)
        {
            Debug.Log("test");
            SceneManager.LoadScene(sceneName: scene.name);
        }
    }
}
