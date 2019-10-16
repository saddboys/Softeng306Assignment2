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
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue){
        // GameObject.Find("ToolbarCanvas").SetActive(false);
        // GameObject.Find("Game Stats Overlay").SetActive(false);
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
    //GameObject.Find("ToolbarCanvas").SetActive(true);
    // stats =  GameObject.Find("ToolbarCanvas");
    // stats.SetActive(true);
      GameObject.Find("DialogueCanvas").SetActive(false);
    }


}
}