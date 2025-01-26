using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public TMP_Text scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int score = PlayerPrefs.GetInt("NewScore", 0);
        int decimals = (int)Mathf.Floor(Mathf.Log10(score) + 1);
        string zeroes = "";
        for (int i = 0; i < 4 - decimals; i++)
        {
            zeroes += "0";
        }
        scoreText.text = zeroes + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Game");
    }
}
