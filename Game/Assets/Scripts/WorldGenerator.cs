using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = System.Random;

public class WorldGenerator : MonoBehaviour
{
    public Text display;
    //public RandomTile tile2;
    public Tilemap map;
    public GameObject parent;
    private int[,] terrainMap;
    public void Generate()
    {
        int width = 10;
        int height = 10;

        if (terrainMap == null)
        {
            terrainMap = new int[width,height];
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                RandomTile tile2 = ScriptableObject.CreateInstance<RandomTile>();
                AnotherRandomTile tile1 = ScriptableObject.CreateInstance<AnotherRandomTile>();
                Random random = new Random();
                int value =  random.Next(0,2);
                Debug.Log(value);
                Vector3Int vector = new Vector3Int(-i + width/2, -j + height/2, 0);
                Vector3 mappedVector = map.CellToWorld(vector);
                if (value == 0)
                {
                    Tower tower = new Tower(parent,mappedVector);
                    tile1.SetStructure(tower);
                    map.SetTile(vector, tile1);
                    Debug.Log("Position i: " + vector.x +" , " + "j: " + vector.y );
                }
                else
                {
                    // Only random tiles have the mountain on it
                    if (random.Next(1, 6) == 4)
                    {
                        Rock rocks = new Rock(parent,mappedVector);
                        tile1.SetStructure(rocks);
                    }

                    map.SetTile(vector, tile2);
                    Debug.Log("i: " + -i + width/2 +" , " + "j: " + -j+height/2 );
                }
                
                
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = map.WorldToCell(worldPoint);
            Debug.Log("Position of: " + position.x +" , " +  position.y);
            ParentTile someOtherTile = map.GetTile<ParentTile>(position);
            if (someOtherTile.color == Color.white)
            {
                someOtherTile.color = Color.blue;
            }
            else
            {
                someOtherTile.color = Color.white;
            }

            if (someOtherTile.GetStructure() != null)
            {
                display.text = someOtherTile.GetStructure().GetCost().ToString();
            }
            someOtherTile.SetSprite(Resources.LoadAll<Sprite>("Textures/terrain")[1]);
            map.RefreshTile(position);
            //  test();
            
        }

        
    }
}
