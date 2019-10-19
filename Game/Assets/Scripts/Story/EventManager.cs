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
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue){
       animator.SetBool("isOpen", true);
       nameText.text = dialogue.name;
       sentences.Clear();

	    foreach (string sentence in dialogue.sentences)
	    {
	        sentences.Enqueue(sentence);
	    }
	    DisplayNextSentence();
    }

    private void DisplayNextSentence(){
	    if (sentences.Count == 0)
	    {
		    EndDialogue();
		    return;
	    }

	    string sentence = sentences.Dequeue();
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