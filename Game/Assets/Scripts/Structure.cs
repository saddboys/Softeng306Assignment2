using UnityEngine.Tilemaps;
using System.Runtime.CompilerServices;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public abstract class Structure
{
    private Image structureImage;
    private GameObject canvas;
    private Vector3 vector;
    
    // Will replace with stats class
    protected float cost;
    public Structure(GameObject canvas, Vector3 vector)
    {
        this.canvas = canvas;
        this.vector = vector;
        //Create();
    }

    public float GetCost()
    {
        return cost;
    }


    protected void Create(int spriteNumber,Vector2 imageSize)
    {
        GameObject structure = new GameObject();
        Image image = structure.AddComponent<Image>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/structures");
        image.sprite = sprites[spriteNumber];
        Vector2 structureSize = imageSize;
        // Determines the size of the structure
        structure.GetComponent<RectTransform>().sizeDelta = structureSize;
        Vector2 xy = new Vector2(vector.x + structureSize.x - 1,vector.y + structureSize.y - 1);
        // Determines where the structure will be placed
        structure.GetComponent<RectTransform>().anchoredPosition = xy;
        structure.GetComponent<RectTransform>().SetParent(canvas.transform);
        structure.SetActive(true);
    }
    
}