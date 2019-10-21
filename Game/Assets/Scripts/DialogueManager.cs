﻿using System;
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

        // Start is called before the first frame update
        void Start()
        {
            sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue){
        
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
            tempbar.SetActive(true);
            stats.SetActive(true);
            toolbar.SetActive(true);
            Finished?.Invoke();
           //TODO:Uncomment this line if you merge to master game scene! 
           GameObject.Find("IntroStory").SetActive(false);
           GameObject.Find("ToolbarCanvas").SetActive(true);
        //GameObject.Find("DialogueCanvas").SetActive(false);
        }


    }
}