using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    
    public float zoomSpeed = 10.0f;
    public float targetZoom;
    public float smoothSpeed = 10.0f;
    public float minZoom = 1.0f;
    public float maxZoom = 20.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        targetZoom = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
                 
        float scroll = Input.GetAxis ("Mouse ScrollWheel");
        if (scroll != 0.0f) {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp (targetZoom, minZoom, maxZoom);
            Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetZoom, smoothSpeed * Time.deltaTime);
        }
        else
        {
            targetZoom = Camera.main.orthographicSize;
        }
        
    }
}
