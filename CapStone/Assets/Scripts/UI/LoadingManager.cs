using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;
    public static AsyncOperation sceanOp;
    //public int timer;
    
    private void Start()
    {
        StartCoroutine(LoadScene());
        
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        float timer = 0.0f;
        sceanOp = SceneManager.LoadSceneAsync(nextScene);
        //AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        sceanOp.allowSceneActivation = false;
        while (!sceanOp.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (timer>= 1f)
            {
                sceanOp.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
