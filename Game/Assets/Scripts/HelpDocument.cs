using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Story
{
public class HelpDocument : MonoBehaviour
{   [SerializeField]
    private GameObject newCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
    // Canvas c = newCanvas.AddComponent<Canvas>();
    // c.renderMode = RenderMode.ScreenSpaceOverlay;
    //  newCanvas.AddComponent<CanvasScaler>();
    //  newCanvas.AddComponent<GraphicRaycaster>();
     

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(){
    GameObject panel = new GameObject("Panel");
     panel.AddComponent<CanvasRenderer>();
     Image i = panel.AddComponent<Image>();
     i.color = Color.red;
     panel.transform.SetParent(newCanvas.transform, false);
    }
}
}