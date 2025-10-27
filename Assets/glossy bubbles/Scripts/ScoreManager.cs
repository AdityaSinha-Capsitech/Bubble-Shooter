using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static ScoreManager scoreInstance;
    [SerializeField] private int totalScore;

    private void Awake()
    {
        scoreInstance = this;   
    }
    void Start()
    {
        totalScore = 0;
    }

    public void InitialScore()
    {
        Debug.Log("Initial Score called!");
        totalScore = 0;
        UpdateScoreUI(0);
    }

    public void UpdateScoreUI(int score)
    {
        totalScore += score;
        scoreText.text = "Score: " + totalScore.ToString();
    }
   
}
