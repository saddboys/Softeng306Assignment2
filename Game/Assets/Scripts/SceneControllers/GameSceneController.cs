using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    public GameObject endScreen;
    public Text endStatusText;
    public Text endReasonText;
    public Text endScoreText;

    public GameObject stats;
    public GameObject toolbar;
    public GameObject tempbar;
    public GameObject restartButton;
    public GameObject nextLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This function is called when the player loses the game. 
    /// It sets all the components (Game end status, end reason, and score) in the end game overlay
    /// </summary>
    /// <param name="endReason"></param> A description giving more info on why the user lost and the consequences
    /// <param name="score"></param> The score 
    public void GameOver(string endReason, double score)
    {
        SetActiveUiComponents();
        endStatusText.text = "Game Over";
        endReasonText.text = endReason;
        endScoreText.text = "Score: " + score;
        restartButton.SetActive(true);
        nextLevelButton.SetActive(false);
    }

    /// <summary>
    /// This function is called when the player wins the game. 
    /// It sets all the components (Game end status, end reason, and score) in the end game overlay
    /// </summary>
    /// <param name="endReason"></param> A description giving more info on why the user won and the consequences
    /// <param name="score"></param> The score 
    public void GameWon(string endReason, double score)
    {
        SetActiveUiComponents();
        endStatusText.text = "You Win!";
        endReasonText.text = endReason;
        endScoreText.text = "Score: " + score;
        restartButton.SetActive(false);
        nextLevelButton.SetActive(true);
    }

    /// <summary>
    /// Helper method to control display of UI components
    /// </summary>
    private void SetActiveUiComponents()
    {
        endScreen.SetActive(true);
        stats.SetActive(false);
        toolbar.SetActive(false);
        tempbar.SetActive(false);
    }

}
