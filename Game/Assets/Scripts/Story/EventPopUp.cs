using System;
using System.Collections;
using System.Collections.Generic;
using Game.CityMap;
using Game.Story;
using Game.Story.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EventTrigger = UnityEngine.EventSystems.EventTrigger;

public class EventPopUp : MonoBehaviour
{

    public GameObject Canvas { get; set; }
    public CityMap CityMap { get; set; }
    public StoryEvent StoryEvent { get; set; }
    private int POP_UP_WIDTH;
    private int POP_UP_HEIGHT;
    private float POP_UP_LIMIT_HEIGHT;
    private float POP_UP_LIMIT_WIDTH;
    private const int BUTTON_WIDTH = 80;
    private const int BUTTON_HEIGHT = 25;
    private const int TITLE_FONT_SIZE = 45;
    private const int DESCRIPTION_FONT_SIZE = 15;

    void Start()
    {
        POP_UP_WIDTH =  Screen.currentResolution.width/2;
        Debug.Log("WIDTH IS: " + POP_UP_WIDTH);
        POP_UP_HEIGHT = Screen.currentResolution.height/4;
    }


    // Placeholder method to create the pop up. Used for testing purposes
    // May call this function when the turn event is fired
    // Maybe have an event fire whenever a create event is called
    private const string POP_UP_NAME = "popup-panel";

    private Animator animator;

    public void Create()
    {
        if (StoryEvent.EventType == StoryEvent.EventTypes.Request)
        {
            CreatePopUp(false);
        }
        else
        {
            CreatePopUp(true);
        }
    }

    private GameObject panel;
    private void CreatePopUp(bool isEvent)
    {
        
        // Creating the panel 
        panel = new GameObject(POP_UP_NAME);
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.sprite = StoryEvent.EventImage;
        panel.transform.SetParent(Canvas.transform, false);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
        StartCoroutine(Popup(isEvent));


       
//       animator = panel.AddComponent<Animator>();
//       animator.runtimeAnimatorController = Resources.Load("Animations/popup-panel") as RuntimeAnimatorController;;
//       animator.SetBool("IsOpen", true);
    }

    private void CreateDefaultElements()
    {
        // Creating the title
        GameObject title = new GameObject("Title");
        TextMeshProUGUI titleText = title.AddComponent<TextMeshProUGUI>();
        //Text titleText = title.AddComponent<Text>();
        titleText.text = StoryEvent.Title;
        titleText.font = Resources.Load<TMP_FontAsset>("Fonts/Bangers SDF");
        titleText.color = Color.white;
        titleText.outlineColor = Color.black;
        titleText.outlineWidth = 0.3f;
        titleText.fontSize = TITLE_FONT_SIZE;
        titleText.alignment = TextAlignmentOptions.Center;
        title.transform.SetParent(panel.transform,false);
        title.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_WIDTH,POP_UP_HEIGHT);
        title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,POP_UP_HEIGHT/2);
        
        // Creating the description
        GameObject description = new GameObject("Description");
        Image backgroundImage = description.AddComponent<Image>();
        backgroundImage.color = new Color32(255,255,255,50);
        
        GameObject descriptionTextObject = new GameObject("DescriptionBackground");

        Text descriptionText = descriptionTextObject.AddComponent<Text>();
        descriptionText.text = StoryEvent.Description;
        descriptionText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        descriptionText.color = Color.black;
        descriptionText.fontSize = DESCRIPTION_FONT_SIZE;
        descriptionText.alignment = TextAnchor.UpperCenter;
        description.transform.SetParent(panel.transform,false);
        description.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_LIMIT_WIDTH/2,POP_UP_LIMIT_HEIGHT/4);
        description.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        descriptionTextObject.transform.SetParent(description.transform);
        descriptionTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(POP_UP_LIMIT_WIDTH/2,20);
        descriptionTextObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);

    }
    /// <summary>
    /// Creates the additional button for an event pop up
    /// </summary>
    private void CreateEventPopUp()
    {
        GameObject panel = Canvas.transform.Find(POP_UP_NAME).gameObject;
        GameObject buttonObj = new GameObject();

        //Button button2 = GUI.Button(new Rect(), new GUIContent() );
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(DestroyPanel);
        button.onClick.AddListener(StoryEvent.OnYesClick);
        button.name = "OK_button";
        
        GameObject textContainer = new GameObject();
        textContainer.transform.SetParent(buttonObj.transform);
        Text text = textContainer.AddComponent<Text>();
        text.text = "OK";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.color = Color.black;
        text.fontSize = DESCRIPTION_FONT_SIZE;
        textContainer.transform.position = new Vector3(0 + BUTTON_WIDTH/2,-BUTTON_WIDTH/2,0);
    
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = Resources.LoadAll<Sprite>("EventSprites/button-spritesheet")[0];
        buttonObj.name = "OKButton";
        buttonObj.transform.SetParent(panel.transform,false);
        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-POP_UP_HEIGHT/2.4f);
        
        EventTrigger eventTrigger = buttonObj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) =>
        {
            buttonImage.color = new Color32(212,212,212,255);
        } );
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((eventData) =>
        {
            buttonImage.color = Color.white;
        } );
        
        
        
        eventTrigger.triggers.Add(entry);
        eventTrigger.triggers.Add(exitEntry);
    }
    
    
    /// <summary>
    /// Creates the additional buttons for a request pop up
    /// </summary>
    private void CreateRequestPopUp()
    {
        GameObject panel = Canvas.transform.Find(POP_UP_NAME).gameObject;

        StoryRequest storyRequest = (StoryRequest) StoryEvent;
        // Setting the buttons
        // The yes button
        GameObject buttonObj = new GameObject();
        buttonObj.name = "YesButton";
        buttonObj.transform.SetParent(panel.transform,false);
        
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(DestroyPanel);
        button.onClick.AddListener(storyRequest.OnYesClick);
        
        GameObject textContainer = new GameObject();
        textContainer.transform.SetParent(buttonObj.transform);
        Text text = textContainer.AddComponent<Text>();
        text.text = "Yes";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = DESCRIPTION_FONT_SIZE;
        textContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        textContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);


        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = Resources.LoadAll<Sprite>("EventSprites/button-spritesheet")[0];
        
//        Image buttonImage = buttonObj.AddComponent<Image>();
//        buttonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[0];
//        Text text = buttonObj.AddComponent<Text>();
//        text.text = "Yes";
//        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
//        text.color = Color.black;
//        text.fontSize = DESCRIPTION_FONT_SIZE;

        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-POP_UP_LIMIT_WIDTH/4,-POP_UP_HEIGHT/2.4f);
        
        
        // The no button or whatever it is going to be called
        GameObject buttonObj2 = new GameObject();
        Button noButton = buttonObj2.AddComponent<Button>();
        buttonObj2.name = "NoButton";
        buttonObj2.transform.SetParent(panel.transform,false);
        noButton.onClick.AddListener(DestroyPanel);
        noButton.onClick.AddListener(storyRequest.OnNoClick);
        
//        Image noButtonImage = buttonObj2.AddComponent<Image>();
//        noButtonImage.sprite = Resources.LoadAll<Sprite>("Textures/Structures")[1];
        GameObject textContainer2 = new GameObject();
        textContainer2.transform.SetParent(buttonObj2.transform);
        Text text2 = textContainer2.AddComponent<Text>();
        text2.text = "No";
        text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text2.color = Color.black;
        text2.alignment = TextAnchor.MiddleCenter;
        text2.fontSize = DESCRIPTION_FONT_SIZE;
        textContainer2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        textContainer2.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);


        Image buttonImage2 = buttonObj2.AddComponent<Image>();
        buttonImage2.sprite = Resources.LoadAll<Sprite>("EventSprites/button-spritesheet")[0];

        buttonObj2.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDTH,BUTTON_HEIGHT);
        buttonObj2.GetComponent<RectTransform>().anchoredPosition = new Vector2(POP_UP_LIMIT_WIDTH/4,-POP_UP_HEIGHT/2.4f);
    }

    IEnumerator Popup(bool isEvent)
    {


        Vector2 currentSize = panel.GetComponent<RectTransform>().sizeDelta;
        while (currentSize.x <= POP_UP_WIDTH && currentSize.y <= POP_UP_HEIGHT)
        {
            currentSize.x += 40;
            currentSize.y += 25;
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(currentSize.x,currentSize.y);
            yield return null;
        }
        POP_UP_LIMIT_HEIGHT = currentSize.y;
        POP_UP_LIMIT_WIDTH = currentSize.x;
        CreateDefaultElements();
        if (isEvent)
        {
            CreateEventPopUp();
        }
        else
        {
            CreateRequestPopUp();
        }
    }
    
    
    /// <summary>
    /// Upon clicking a button, we destroy the panel.
    /// </summary>
    private void DestroyPanel()
    {
        StoryEvent.GenerateScene(Canvas);
        Canvas.transform.Find("Panel").gameObject.SetActive(false);
        GameObject panel = Canvas.transform.Find(POP_UP_NAME).gameObject;
        Destroy(panel);
        Destroy(GetComponent<EventPopUp>());
    }
}

