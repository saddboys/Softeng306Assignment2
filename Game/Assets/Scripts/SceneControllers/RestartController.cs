using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Loads start scene when restart button is clicked
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
