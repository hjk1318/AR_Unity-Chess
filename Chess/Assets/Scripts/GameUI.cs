using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject startScreen; // ���� ȭ�� �г�
    public GameObject gameOverScreen; // ���� ���� ȭ�� �г�
    public Text resultText; // �¸�/�й� ��� �ؽ�Ʈ

    public void StartGame()
    {
        startScreen.SetActive(false); // ���� ȭ�� ��Ȱ��ȭ
        Time.timeScale = 1; // ���� �ð� Ȱ��ȭ
    }

    public void ShowGameOverScreen(string result)
    {
        gameOverScreen.SetActive(true); // ���� ���� ȭ�� Ȱ��ȭ
        resultText.text = result; // ��� �ؽ�Ʈ ������Ʈ
        Time.timeScale = 0; // ���� �ð� ����
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� �� ��ε�
    }
}
