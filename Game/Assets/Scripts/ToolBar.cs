﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ToolBar : MonoBehaviour
    {
        public ToolBar(City city)
        {
            // E.g.
            city.Map.TileClickedEvent += (s, e) =>
            {
                // TODO: handle when the tile e.Tile has been clicked.
                Debug.Log(e.Tile);
                throw new System.NotImplementedException();
            };
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}