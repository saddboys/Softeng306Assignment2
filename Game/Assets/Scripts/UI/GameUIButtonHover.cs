using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUIButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
//        ButtonText.color = new Color(38, 239, 95, 255);
        if (button.interactable)
        {
            buttonText.color = Color.green;    
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = Color.white;
    }
}
