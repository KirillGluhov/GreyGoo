using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public string loadLevel;
    //void Start()
    //{
    //    SceneManager.LoadScene(loadLevel);
    //}

    void Start()
    {
        
    }

    public void StartLoading()
    {
        StartCoroutine(LoadSceneWithDelay());
    }

    IEnumerator LoadSceneWithDelay()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadLevel);
        asyncLoad.allowSceneActivation = false;

        // Ждем, пока загрузка сцены не завершится
        while (!asyncLoad.isDone)
        {
            // Проверяем, достигла ли загрузка сцены 90%
            if (asyncLoad.progress >= 0.9f)
            {
                // Ждем 3 секунды перед переходом на новую сцену
                yield return new WaitForSeconds(3f);

                // Разрешаем активацию загруженной сцены
                asyncLoad.allowSceneActivation = true;

                yield break;
            }

            yield return null;
        }
    }

    void Update()
    {
        
    }
}
