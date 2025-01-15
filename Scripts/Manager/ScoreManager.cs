using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private AnimationManagerGame animationManager;
    
    [SerializeField] private TextMeshProUGUI scoreText;

    private int playerScore = 0;

    void Start()
    {
        UpdateScoreText();
    }

    public void IncrementScore(int amount)
    {
        playerScore += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        animationManager.OnScorePopInEnter();
        scoreText.text = playerScore.ToString();
    }
}
