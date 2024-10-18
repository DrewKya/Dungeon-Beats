using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region Singleton
    public static SceneLoader instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"More than one instance of {instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public Animator transition;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadCoroutine(sceneName));
    }

    private IEnumerator LoadCoroutine(string sceneName)
    {
        StartTransition();
        yield return new WaitForSeconds(0.5f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        

        while (asyncOperation.progress < 1)
        {
            yield return null;
        }
        EndTransition();

        asyncOperation.allowSceneActivation = true;

    }

    private void StartTransition()
    {
        transition.SetTrigger("Start");
    }

    private void EndTransition()
    {
        transition.SetTrigger("End");
    }
}
