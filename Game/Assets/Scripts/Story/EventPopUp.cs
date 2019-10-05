using System;
using System.Collections;
using System.Collections.Generic;
using Game.Story;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class EventPopUp : MonoBehaviour
{

    public GameObject canvas;
    public GameObject cityMap;

    private const int POP_UP_WIDTH = 10;
    private const int POP_UP_HEIGHT = 5;
    private const int BUTTON_WIDTH = 1;
    private const int BUTTON_HEIGHT = 1;


    // Placeholder method to create the pop up. Used for testing purposes
    public void OnClick()
    {
        CreatePopUp();
    }
    
    
    // TODO: Make this be called at a certain turn count
    public void CreatePopUp()
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
        
        // Creating the title
        GameObject title = new GameObject("Title");
        Text titleText = title.AddComponent<Text>();
        titleText.text = "I am a title";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.color = Color.black;
        titleText.fontSize = 1;
        titleText.alignment = TextAnchor.UpperCenter;
        title.transform.SetParent(panel.transform,false);
        title.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
        
        // Creating the description
        GameObject description = new GameObject("Description");
        Text descriptionText = description.AddComponent<Text>();
        descriptionText.text = "test";
        descriptionText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        descriptionText.color = Color.black;
        descriptionText.fontSize = 1;
        descriptionText.alignment = TextAnchor.UpperCenter;
        description.transform.SetParent(panel.transform,false);
        description.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
        description.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-POP_UP_HEIGHT/2 + 1);
        // TODO: Scale depending on size of the description
        description.GetComponent<RectTransform>().localScale = new Vector3(0.5f,0.5f,1);
        
        
        // Setting the buttons
        // YES?
        GameObject buttonObj = new GameObject();
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(OnYesClick);
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[0];
        buttonObj.name = "YesButton";
        buttonObj.transform.SetParent(panel.transform,false);
        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2,-1);
        
        // The no button or whatever it is going to be called
        GameObject buttonObj2 = new GameObject();
        Button noButton = buttonObj2.AddComponent<Button>();
        noButton.onClick.AddListener(OnNoClick);
        Image noButtonImage = buttonObj2.AddComponent<Image>();
        noButtonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[1];
        buttonObj2.name = "NoButton";
        buttonObj2.transform.SetParent(panel.transform,false);
        buttonObj2.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj2.GetComponent<RectTransform>().anchoredPosition = new Vector2(2,-1);
    }
    private void OnNoClick()
    {
        Debug.Log("No has been clicked");
        // Fire the no listener for event somewhere
        DestroyPanel();
    }

    private void OnYesClick()
    {
        Debug.Log("Yes has been clicked");
        DestroyPanel();
    }

    private void DestroyPanel()
    {
        cityMap.active = true;
        GameObject panel = canvas.transform.Find("Panel").gameObject;
        Destroy(panel);
    }
}
