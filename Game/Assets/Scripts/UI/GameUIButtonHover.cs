using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUIButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text ButtonText;
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
        ButtonText.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonText.color = Color.white;
    }
}
