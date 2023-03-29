using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader : MonoBehaviour
    {
        public void Load(string sceneName, Action onLoaded = null) =>
            StartCoroutine(LoadScene(sceneName, onLoaded));

        private IEnumerator LoadScene(string sceneName, Action onLoaded = null)
        {
            string activeScene = SceneManager.GetActiveScene().name;
            if (activeScene == sceneName)
            {
                Debug.Log($"Scene {sceneName} is already loaded");
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(sceneName);

            yield return new WaitUntil(() => waitNextScene.isDone);

            onLoaded?.Invoke();
        }
    }
}