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
        // Ensure button starts off at the correct, consistent colour.
        OnPointerExit(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            buttonText.color = Color.green;    
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = Color.white;
    }

    public void OnDisable()
    {
        // Prevent "stuck" hover states due to disabling the button.
        OnPointerExit(null);
    }
}
