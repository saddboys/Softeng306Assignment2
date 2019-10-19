using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game
{
public class BarHandler : MonoBehaviour
{   [SerializeField]
    public Slider slider;
    public float speed = 1.2F;
    private double currentValue = 0.0;
    [SerializeField]
    private StatsBar stats;
    public double CurrentValue {
    get {
        return currentValue;
    }
    set {
        currentValue = (float)stats.Temperature;
        slider.value =(float)currentValue;
       
    }
}

void Update(){
    slider.value =Mathf.Lerp(slider.value,(float)stats.Temperature,Time.deltaTime * speed);
}
 
}
}
