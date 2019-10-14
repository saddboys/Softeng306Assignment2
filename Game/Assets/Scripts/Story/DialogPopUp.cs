using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Story
{
    public class DialogPopUp : MonoBehaviour
    {
        public GameObject Canvas { get; set; }
        public StoryEvent StoryEvent { get; set; }
        private Animator animator;
        public Action Finished;
        public void Create()
        {
            GameObject panel = new GameObject("Dialogue");
            panel.AddComponent<CanvasRenderer>();
            Image i = panel.AddComponent<Image>();
            i.color = Color.white;
            panel.transform.SetParent(Canvas.transform, false);
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(300,300);

            animator = panel.AddComponent<Animator>();
            animator.runtimeAnimatorController = Resources.Load("Animations/Dialogue") as RuntimeAnimatorController;;
            animator.SetBool("IsOpen", true);
            
            GameObject title = new GameObject("DialogueName");
            Text titleText = title.AddComponent<Text>();
            titleText.text = "Secretary";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.color = Color.black;
            titleText.fontSize = 20;
            titleText.alignment = TextAnchor.UpperCenter;
            title.transform.SetParent(panel.transform,false);
            title.GetComponent<RectTransform>().sizeDelta = new Vector2(300,300);
            title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            
            GameObject descriptions = new GameObject("DialogueText");
            Text descriptionText = descriptions.AddComponent<Text>();
            descriptionText.text = StoryEvent.Dialogues.Dequeue();
            descriptionText.font =Resources.GetBuiltinResource<Font>("Arial.ttf");
            descriptionText.color = Color.black;
            descriptionText.fontSize = 20;
            descriptionText.alignment = TextAnchor.UpperCenter;
            descriptions.transform.SetParent(panel.transform,false);
            descriptions.GetComponent<RectTransform>().sizeDelta = new Vector2(300,300);
            descriptions.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-50);
            
            GameObject nextButton = new GameObject("NextButton");
            Button button = nextButton.AddComponent<Button>();
            button.onClick.AddListener(OnNextClick);
            Text text = nextButton.AddComponent<Text>();
            text.text = "NEXT >>";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.color = Color.black;
            text.fontSize = 10;
            nextButton.GetComponent<RectTransform>().sizeDelta = new Vector2(50,20);
            nextButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(80,-80);
            nextButton.transform.SetParent(panel.transform,false);
        }

        private void OnNextClick()
        {
            if (StoryEvent.Dialogues.Count == 0)
            {
                animator.SetBool("IsOpen", false);
                DestroyDialogPopup();
                Finished?.Invoke();
                return;
            }
            Text text = Canvas.transform.Find("Dialogue").Find("DialogueText").GetComponent<Text>();
            text.text = StoryEvent.Dialogues.Dequeue();

        }

        private void DestroyDialogPopup()
        {
            Canvas.transform.Find("Panel").gameObject.SetActive(false);
            GameObject panel = Canvas.transform.Find("Dialogue").gameObject;
            Destroy(panel);
            Destroy(GetComponent<EventPopUp>());
        }
    }
}