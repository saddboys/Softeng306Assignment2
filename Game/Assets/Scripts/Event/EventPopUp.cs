using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPopUp : MonoBehaviour
{

    public GameObject canvas;

    // TODO: Make this be called at a certain turn count
    // Also, rename onClick to something more appropriate :)
    public void OnClick()
    {
        //GameObject newCanvas = new GameObject("Test");
//        Canvas c = newCanvas.AddComponent<Canvas>();
//        c.renderMode = RenderMode.ScreenSpaceOverlay;
//        newCanvas.AddComponent<CanvasScaler>();
//        newCanvas.AddComponent<GraphicRaycaster>();
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
       Button button = panel.AddComponent<Button>();
       button.name = "YES";
        Image i = panel.AddComponent<Image>();
        
        i.color = Color.red;
        panel.transform.SetParent(canvas.transform, false);
    }
    
}
