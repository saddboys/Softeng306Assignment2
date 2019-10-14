using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Story
{
public class EventManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Queue<string> sentences;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue){
       animator.SetBool("isOpen", true);
        Debug.Log("start conversation!"+ dialogue.name);
        nameText.text = dialogue.name;
        sentences.Clear();

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
      //GameObject.Find("DialogueCanvas").SetActive(false);
    }


}
}