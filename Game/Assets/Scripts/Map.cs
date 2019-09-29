
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TilePosition;

public class Map : MonoBehaviour
{

    GameObject[,] Tiles;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCity(11, 11); //change these variables later
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject HexPrefab;

    private void GenerateCity(float xParam, float yParam)
    {
        Tiles = new GameObject[(int)xParam, (int)yParam];
        //for (float y = yParam; y >= 0; y--)
        for (float y = 0; y < yParam; y++)
        {
            for (float x = 0; x < xParam; x++)
            {
                float z = y;
                TilePosition tp = new TilePosition(x, y, z);
                //best tile size = 0.102, y=0.117
                Vector3 tileVector = tp.Pos();

                GameObject tile = (GameObject) Instantiate(HexPrefab,
                    tileVector,
                    Quaternion.identity,
                    this.transform);
                Tiles[(int)x, (int)y] = tile;
            }
        }
    }
}

