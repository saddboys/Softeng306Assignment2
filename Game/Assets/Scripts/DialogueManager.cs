using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DialogueManager : MonoBehaviour
    {
        public Text nameText;
        public Text dialogueText;
        public Queue<string> sentences;
        public GameObject stats;
        public GameObject toolbar;
        public GameObject tempbar;
        public Animator animator;
        public Action Finished;
        public Button endTurn;
        private int counter = 0;
  
    private bool isIntro = true;

        // Start is called before the first frame update
        void Start()
        {
            sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue){
            endTurn.interactable = false;
            Debug.Log("start conversation!"+ dialogue.name);
            nameText.text = dialogue.name;
            sentences.Clear();
            if(GameObject.Find("ToolbarCanvas")!=null){
                GameObject.Find("ToolbarCanvas").SetActive(false);
            }
          
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            DisplayNextSentence();
        }

        public void DisplayNextSentence(){
            if(sentences.Count == 0){
                EndDialogue();
                return;
            }
            string sentence = sentences.Dequeue();
            Debug.Log("next conversation!"+ sentence);
            dialogueText.text = sentence;
            Debug.Log("counter:"+ counter);
            //this ensures the correct gameobject display on the correct dialgue
            if(counter == 5){
                 endTurn.interactable = false;
            stats.SetActive(true);
            
            }else if(counter == 7)
            {
            tempbar.SetActive(true);
            }
            else if( counter == 8){
            toolbar.SetActive(true);
            }
            
            
            counter ++;
            
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));

        }
        
        IEnumerator TypeSentence (string sentence)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }

        public void EndDialogue(){
            animator.SetBool("isOpen", false);
             if(isIntro){
                Debug.Log("Tutorial start");
                CreateTutorial();
                isIntro=false;
                GameObject.Find("IntroStory").SetActive(true);
            }else {
                 GameObject.Find("IntroStory").SetActive(false);
            }
             endTurn.interactable = true;
           
            Finished?.Invoke();
           if(counter > 9){
                stats.SetActive(true);
                tempbar.SetActive(true);
                toolbar.SetActive(true);
           }
            
        }

        public void CreateTutorial(){
         
            Dialogue dialog = new Dialogue(); 
            dialog.name = "Secretary";
             dialog.sentences =   new String[] {"So how do you run a city?",
             "At the top of the screen, you’ll see an overview of your city.", " Make sure your money balance and your happiness"+
              "is above zero. Your city produces CO2. The more CO2, the faster the temperature rises!",
              "On the right, you’ll see the temperature bar. You’d want to keep that low.",
             "On the left, you get to pick what you want to build.",
             "Once you’re done, end your turn to see what happens!",
             "To move around the city, drag your mouse or use your arrow keys. Press the spacebar to rotate."};
       
            FindObjectOfType<DialogueManager>().StartDialogue(dialog);
            } 
        
    }
}