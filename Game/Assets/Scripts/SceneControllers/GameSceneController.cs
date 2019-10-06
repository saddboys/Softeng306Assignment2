using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
    public GameObject endScreen;
    public Text endStatusText;
    
    // Start is called before the first frame update
    void Start()
    {
        endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        endScreen.SetActive(true);
        endStatusText.text = "Game Over";
    }

    public void GameWon()
    {
        endScreen.SetActive(true);
        endStatusText.text = "You Win!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("StartScene");
    }
    
}
