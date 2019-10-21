using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingText : MonoBehaviour
{
   public float timer;
   [SerializeField]
   public Slider slider;
    // Update is called once per frame
    void Update()
    {
        if(slider.value > 1.5){
        timer += Time.deltaTime;
  
        if(timer >= 0.5 ) {
        GetComponent<Text>().enabled = true;    
        }

        if(timer >=1){
        GetComponent<Text>().enabled = false;
        timer = 0;    
        }
        }
    }
}
