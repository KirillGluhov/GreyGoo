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

        // ����, ���� �������� ����� �� ����������
        while (!asyncLoad.isDone)
        {
            // ���������, �������� �� �������� ����� 90%
            if (asyncLoad.progress >= 0.9f)
            {
                // ���� 3 ������� ����� ��������� �� ����� �����
                yield return new WaitForSeconds(3f);

                // ��������� ��������� ����������� �����
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
