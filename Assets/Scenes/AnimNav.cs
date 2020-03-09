using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimNav : MonoBehaviour
{
    public GameObject trigger;
    public int transitionSecs;
    public Object transitionScene;
    public Object targetScene;
    

    void OnCollisionEnter(Collision collision)
    {

        // Public Variable Checks (Maybe worth moving to initialization
        if (transitionScene == null || transitionScene.GetType().Name != "SceneAsset" ||
            targetScene == null || targetScene.GetType().Name != "SceneAsset")
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
            StartCoroutine(transition());
        }
    }


    IEnumerator transition()
    {
        SceneManager.LoadScene(sceneName: transitionScene.name, LoadSceneMode.Additive);

        //Wait for transitionSecs seconds`
        yield return new WaitForSeconds(transitionSecs);

        SceneManager.LoadScene(sceneName: targetScene.name, LoadSceneMode.Additive);
    }
}