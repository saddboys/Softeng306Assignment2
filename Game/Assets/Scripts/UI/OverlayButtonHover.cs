using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OverlayButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text ButtonText;
    // Start is called before the first frame update
    void Start()
    {
        // Ensure button starts off at the correct, consistent colour.
        OnPointerExit(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonText.color = Color.black;
    }

    public void OnDisable()
    {
        // Prevent "stuck" hover states due to disabling the button.
        OnPointerExit(null);
    }
}