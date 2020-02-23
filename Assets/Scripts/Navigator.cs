using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    public GameObject trigger;
    public Object scene;

    void OnCollisionEnter(Collision collision)
    {

        // Public Variable Checks
        if (scene == null || scene.GetType().Name != "SceneAsset")
        {
            Debug.LogError("Invalid Scene Asset");
            return;
        }

        if (trigger == null || trigger.GetType().Name != "GameObject")
        {
            Debug.LogError("Invalid Game Object");
            return;
        }

        // Switch scene if object name matches
        if (collision.gameObject.name == trigger.name)
        {
            SceneManager.LoadScene(sceneName: scene.name);
        }
    }
}