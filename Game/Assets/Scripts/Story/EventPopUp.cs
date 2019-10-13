using System;
using System.Collections;
using System.Collections.Generic;
using Game.CityMap;
using Game.Story;
using Game.Story.Events;
using UnityEngine;
using UnityEngine.UI;

public class EventPopUp : MonoBehaviour
{

    public GameObject Canvas { get; set; }
    public CityMap CityMap { get; set; }

    private const int POP_UP_WIDTH = 400;
    private const int POP_UP_HEIGHT = 250;
    private const int BUTTON_WIDTH = 50;
    private const int BUTTON_HEIGHT = 30;
    private const int TITLE_FONT_SIZE = 30;
    private const int DESCRIPTION_FONT_SIZE = 15;

    public StoryEvent StoryEvent { get; set; }

    // Placeholder method to create the pop up. Used for testing purposes
    // May call this function when the turn event is fired
    // Maybe have an event fire whenever a create event is called
    private const string POP_UP_NAME = "popup-panel";

    public void Create()
    {
        if (StoryEvent.EventType == StoryEvent.EventTypes.Request)
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
        
        // Creating the panel 
        GameObject panel = new GameObject(POP_UP_NAME);
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.color = Color.white;
        panel.transform.SetParent(Canvas.transform, false);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
        
        // Creating the title
        GameObject title = new GameObject("Title");
        Text titleText = title.AddComponent<Text>();
        titleText.text = StoryEvent.Title;
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.color = Color.black;
        titleText.fontSize = TITLE_FONT_SIZE;
        titleText.alignment = TextAnchor.UpperCenter;
        title.transform.SetParent(panel.transform,false);
        title.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
        title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-10);
        
        // Creating the description
        GameObject description = new GameObject("Description");
        Text descriptionText = description.AddComponent<Text>();
        descriptionText.text = StoryEvent.Description;
        descriptionText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        descriptionText.color = Color.black;
        descriptionText.fontSize = DESCRIPTION_FONT_SIZE;
        descriptionText.alignment = TextAnchor.UpperCenter;
        description.transform.SetParent(panel.transform,false);
        description.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
        description.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-POP_UP_HEIGHT/4-10);
        // TODO: Scale depending on size of the description
       // description.GetComponent<RectTransform>().localScale = new Vector3(0.5f,0.5f,1);
       
       // Setting the image 
       GameObject imageGameObject = new GameObject("Image");
       Image sprite = imageGameObject.AddComponent<Image>();
       sprite.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH/2,POP_UP_HEIGHT/2);
       sprite.sprite = StoryEvent.EventImage;
       imageGameObject.transform.SetParent(panel.transform,false);
       description.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
       description.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-POP_UP_HEIGHT/6);

    }

    
    /// <summary>
    /// Creates the additional button for an event pop up
    /// </summary>
    private void CreateEventPopUp()
    {
        CreatePopUp();
        GameObject panel = Canvas.transform.Find(POP_UP_NAME).gameObject;
        GameObject buttonObj = new GameObject();
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(DestroyPanel);
        button.name = "HELLO";
        Text text = buttonObj.AddComponent<Text>();
        text.text = "OK";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.color = Color.black;
        text.fontSize = DESCRIPTION_FONT_SIZE;
//        Image buttonImage = buttonObj.AddComponent<Image>();
//        buttonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[0];
        buttonObj.name = "OKButton";
        buttonObj.transform.SetParent(panel.transform,false);
        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0 + BUTTON_WIDTH/2,-POP_UP_HEIGHT/2.4f);
    }
    
    
    /// <summary>
    /// Creates the additional buttons for a request pop up
    /// </summary>
    private void CreateRequestPopUp()
    {
        CreatePopUp();
        GameObject panel = Canvas.transform.Find(POP_UP_NAME).gameObject;

        StoryRequest storyRequest = (StoryRequest) StoryEvent;
        // Setting the buttons
        // The yes button
        GameObject buttonObj = new GameObject();
        Button button = buttonObj.AddComponent<Button>();
        
        button.onClick.AddListener(DestroyPanel);
        button.onClick.AddListener(storyRequest.OnYesClick);
        
//        Image buttonImage = buttonObj.AddComponent<Image>();
//        buttonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[0];
        Text text = buttonObj.AddComponent<Text>();
        text.text = "Yes";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.color = Color.black;
        text.fontSize = DESCRIPTION_FONT_SIZE;
        buttonObj.name = "YesButton";
        buttonObj.transform.SetParent(panel.transform,false);
        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-POP_UP_WIDTH/4,-POP_UP_HEIGHT/2.4f);
        
        // The no button or whatever it is going to be called
        GameObject buttonObj2 = new GameObject();
        Button noButton = buttonObj2.AddComponent<Button>();
        
        noButton.onClick.AddListener(DestroyPanel);
        noButton.onClick.AddListener(storyRequest.OnNoClick);
        
//        Image noButtonImage = buttonObj2.AddComponent<Image>();
//        noButtonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[1];
        Text noText = buttonObj2.AddComponent<Text>();
        noText.text = "No";
        noText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        noText.color = Color.black;
        noText.fontSize = DESCRIPTION_FONT_SIZE;
        buttonObj2.name = "NoButton";
        buttonObj2.transform.SetParent(panel.transform,false);
        buttonObj2.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj2.GetComponent<RectTransform>().anchoredPosition = new Vector2(POP_UP_WIDTH/4,-POP_UP_HEIGHT/2.4f);
    }
    
    /// <summary>
    /// Upon clicking a button, we destroy the panel.
    /// </summary>
    private void DestroyPanel()
    {
        
        Debug.Log("goes here");
        Canvas.transform.Find("Panel").gameObject.SetActive(false);
        GameObject panel = Canvas.transform.Find(POP_UP_NAME).gameObject;
        Debug.Log(panel.name);
        Destroy(panel);
        Destroy(GetComponent<EventPopUp>());
    }
}

