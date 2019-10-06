using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameStartControl : MonoBehaviour
    {
        public void LoadGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}