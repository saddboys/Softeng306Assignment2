using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    
    public float zoomSpeed = 10.0f;
    public float targetZoom;
    public float smoothSpeed = 10.0f;
    public float minZoom = 2.0f;
    public float maxZoom = 10.0f;
    private float originalZoom;

    public void ResetZoom()
    {
        Debug.Log("Reset zoom to " + originalZoom);
        targetZoom = originalZoom;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        originalZoom = Camera.main.orthographicSize;
        ResetZoom();
    }

    // Update is called once per frame
    void Update()
    {
                 
        float scroll = Input.GetAxis ("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            targetZoom -= scroll * zoomSpeed / 4;
        }
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        Camera.main.orthographicSize += (targetZoom - Camera.main.orthographicSize) * Time.deltaTime * 4;

    }
}
