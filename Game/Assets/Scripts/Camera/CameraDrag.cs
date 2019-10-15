using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed;
    private Vector3 dragOrigin;

    private float minX = -5f;
    private float maxX = 5f;
    private float minY = -5f;
    private float maxY = 5f;

    private bool dragEnabled = false;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            dragEnabled = false;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            dragOrigin = Input.mousePosition;
            dragEnabled = true;
        }

        Vector2 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector2 move = -pos * dragSpeed;

        if (!Input.GetMouseButton(0) || !dragEnabled)
        {
            move *= 0;
        }

        // Pan camera via keyboard arrow keys.
        Vector2 screenKeyboardMove = new Vector2();
        screenKeyboardMove.x = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        screenKeyboardMove.y = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        move += screenKeyboardMove * 0.1f;

        transform.Translate(move, Space.World);
        
        Vector3 v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, minX, maxX);
        v3.y = Mathf.Clamp(v3.y, minY, maxY);
        transform.position = v3;
    }
    
 
}