using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    public GameObject endScreen;
    public Text endStatusText;
    public Text endReasonText;
    
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
        endScreen.SetActive(true);
        endStatusText.text = "Game Over";
        endReasonText.text = endReason;
    }

    public void GameWon(string endReason)
    {
        endScreen.SetActive(true);
        endStatusText.text = "You Win!";
        endReasonText.text = endReason;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("StartScene");
    }
    
}
