using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public string sceneName; // Имя сцены, на которую вы хотите перейти
    public void ChangeScene()
    {
        // Загружаем новую сцену по имени
        SceneManager.LoadScene(sceneName);
    }
}
