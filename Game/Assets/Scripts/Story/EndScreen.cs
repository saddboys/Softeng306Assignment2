using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public GameObject parent;
    public GameObject gameOver;
    public GameObject gameWon;
    
    // Start is called before the first frame update
    void Start()
    {
        // Hide endscreen upon initialisation
        parent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // TODO: Needs to have an event listener on turn counter and stats. 
}
