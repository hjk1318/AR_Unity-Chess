using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject startScreen; // 시작 화면 패널
    public GameObject gameOverScreen; // 게임 종료 화면 패널
    public Text resultText; // 승리/패배 결과 텍스트

    public void StartGame()
    {
        startScreen.SetActive(false); // 시작 화면 비활성화
        Time.timeScale = 1; // 게임 시간 활성화
    }

    public void ShowGameOverScreen(string result)
    {
        gameOverScreen.SetActive(true); // 게임 종료 화면 활성화
        resultText.text = result; // 결과 텍스트 업데이트
        Time.timeScale = 0; // 게임 시간 정지
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 재로드
    }
}
