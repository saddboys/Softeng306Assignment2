using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        generateCity(10, 10); //change these variables later
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject HexPrefab;

    public void generateCity(float xParam, float yParam)
    {
        for (float x = 0; x < xParam; x = x + 1)
        {
            for (float y = 0; y < yParam; y = y + (float)0.75)
            {
                //best tile size = 0.102, y=0.117
                if (y % 1.5 == 0) { 
                Instantiate(HexPrefab,
                    new Vector2(x, y),
                    Quaternion.identity,
                    this.transform);
                } else
                {
                    Instantiate(HexPrefab,
                    new Vector2(x+ (float)0.5, y),
                    Quaternion.identity,
                    this.transform);
                }
            }
        }
    }
}
