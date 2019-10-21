using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void GameOver(string endReason)
    {
        
        setActiveUIComponents();
        endStatusText.text = "Game Over";
        endReasonText.text = endReason;
        restartButton.SetActive(true);
        nextLevelButton.SetActive(false);
    }

    public void GameWon(string endReason)
    {
        setActiveUIComponents();
        endStatusText.text = "You Win!";
        endReasonText.text = endReason;
        restartButton.SetActive(false);
        nextLevelButton.SetActive(true);
    }

    private void setActiveUIComponents()
    {
        endScreen.SetActive(true);
        stats.SetActive(false);
        toolbar.SetActive(false);
        tempbar.SetActive(false);
    }

}
