using System;
using System.Collections;
using System.Collections.Generic;
using Game.Story;
using Game.Story.Events;
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

    private StoryEvent storyEvent;

    // Placeholder method to create the pop up. Used for testing purposes
    // May call this function when the turn event is fired
    // Maybe have an event fire whenever a create event is called
    
    public void OnClick()
    {
        // The pool does not work currently due to a new instance of manager being called each time
        // the button is clicked.
        StoryManager manager = new StoryManager();
        storyEvent = manager.CreateEvent();
        if (storyEvent.EventType == StoryEvent.EventTypes.Request)
        {
            CreateRequestPopUp();
        }
        else
        {
            CreateEventPopUp();
        }
    }
    
    private void CreatePopUp()
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
        titleText.text = storyEvent.Title;
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.color = Color.black;
        titleText.fontSize = 1;
        titleText.alignment = TextAnchor.UpperCenter;
        title.transform.SetParent(panel.transform,false);
        title.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
        
        // Creating the description
        GameObject description = new GameObject("Description");
        Text descriptionText = description.AddComponent<Text>();
        descriptionText.text = storyEvent.Description;
        descriptionText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        descriptionText.color = Color.black;
        descriptionText.fontSize = 1;
        descriptionText.alignment = TextAnchor.UpperCenter;
        description.transform.SetParent(panel.transform,false);
        description.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
        description.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        // TODO: Scale depending on size of the description
        description.GetComponent<RectTransform>().localScale = new Vector3(0.5f,0.5f,1);
    }

    
    /// <summary>
    /// Creates the additional button for an event pop up
    /// </summary>
    private void CreateEventPopUp()
    {
        CreatePopUp();
        GameObject panel = canvas.transform.Find("Panel").gameObject;
        GameObject buttonObj = new GameObject();
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(DestroyPanel);
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[0];
        buttonObj.name = "OKButton";
        buttonObj.transform.SetParent(panel.transform,false);
        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-1);
    }
    
    
    /// <summary>
    /// Creates the additional buttons for a request pop up
    /// </summary>
    private void CreateRequestPopUp()
    {
        CreatePopUp();
        GameObject panel = canvas.transform.Find("Panel").gameObject;

        StoryRequest storyRequest = (StoryRequest) storyEvent;
        // Setting the buttons
        // The yes button
        GameObject buttonObj = new GameObject();
        Button button = buttonObj.AddComponent<Button>();
        
        button.onClick.AddListener(DestroyPanel);
        button.onClick.AddListener(storyRequest.OnYesClick);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[0];
        buttonObj.name = "YesButton";
        buttonObj.transform.SetParent(panel.transform,false);
        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2,-1);
        
        // The no button or whatever it is going to be called
        GameObject buttonObj2 = new GameObject();
        Button noButton = buttonObj2.AddComponent<Button>();
        
        noButton.onClick.AddListener(DestroyPanel);
        noButton.onClick.AddListener(storyRequest.OnNoClick);
        
        Image noButtonImage = buttonObj2.AddComponent<Image>();
        noButtonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[1];
        buttonObj2.name = "NoButton";
        buttonObj2.transform.SetParent(panel.transform,false);
        buttonObj2.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj2.GetComponent<RectTransform>().anchoredPosition = new Vector2(2,-1);
    }
    
    /// <summary>
    /// Upon clicking a button, we destroy the panel.
    /// </summary>
    private void DestroyPanel()
    {
        cityMap.active = true;
        GameObject panel = canvas.transform.Find("Panel").gameObject;
        Destroy(panel);
    }
}
