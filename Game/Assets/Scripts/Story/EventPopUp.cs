using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPopUp : MonoBehaviour
{

    public GameObject canvas;
    public GameObject cityMap;

    private const int POP_UP_WIDTH = 10;
    private const int POP_UP_HEIGHT = 5;
    private const int BUTTON_WIDTH = 1;
    private const int BUTTON_HEIGHT = 1;

    // TODO: Make this be called at a certain turn count
    // Also, rename onClick to something more appropriate :)
    public void OnClick()
    {
        // Upon creating we want to disable everything else.
        // TODO: Need some way to make the background darker probably.
        cityMap.active = false;

        // Creating the panel
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.color = Color.white;
        panel.transform.SetParent(canvas.transform, false);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
        
        // Setting the buttons
        // YES?
        GameObject buttonObj = new GameObject();
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[0];
        buttonObj.name = "YesButton";
        buttonObj.transform.SetParent(panel.transform,false);
        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2,-1);
        
        GameObject buttonObj2 = new GameObject();
        Button noButton = buttonObj2.AddComponent<Button>();
        noButton.onClick.AddListener(OnButtonClick);
        Image noButtonImage = buttonObj2.AddComponent<Image>();
        noButtonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[1];
        buttonObj2.name = "NoButton";
        buttonObj2.transform.SetParent(panel.transform,false);
        buttonObj2.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj2.GetComponent<RectTransform>().anchoredPosition = new Vector2(2,-1);
    }
    
    /// <summary>
    /// Upon selection, probably send a signal to the listeners and delete the pop up.
    /// </summary>
    private void OnButtonClick()
    {
        cityMap.active = true;
        GameObject panel = canvas.transform.Find("Panel").gameObject;
        Destroy(panel);
    }
}
