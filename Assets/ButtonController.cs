using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public string sceneName; // Имя сцены, на которую вы хотите перейти
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName); // Загружаем новую сцену по имени
    }
}
