using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public string sceneName; // ��� �����, �� ������� �� ������ �������
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName); // ��������� ����� ����� �� �����
    }
}
