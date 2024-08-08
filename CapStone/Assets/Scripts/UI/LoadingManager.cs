using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;
    public static AsyncOperation sceanOp;
    public int timer;
    
    private void Start()
    {
        LoadScene("School");
        StartCoroutine(LoadScene());
        timer = 0;
        
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene(nextScene);
        Debug.Log(nextScene);
    }

    IEnumerator LoadScene()
    {
        yield return null;
        timer++;
        Debug.Log(nextScene);
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        Debug.Log(timer);
        while (!op.isDone && timer>=180)
        {
            yield return null;
            if (op.isDone && timer>= 180)
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
