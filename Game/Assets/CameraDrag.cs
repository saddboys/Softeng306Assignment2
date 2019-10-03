using UnityEngine;
 
public class CameraDrag : MonoBehaviour
{
    public float dragSpeed;
    private Vector3 dragOrigin;
 
 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
        }

        if (!Input.GetMouseButton(0))
        {
            return;
        }
 
        Vector2 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector2 move = new Vector2(-pos.x * dragSpeed, -pos.y * dragSpeed);
        transform.Translate(move, Space.World);
    }
 
 
}