using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.CityMap
{
    public abstract class Terrain : MonoBehaviour
    {

        public Sprite Sprite
        {
            get { return sprite; }
            set
            {
                sprite = value;
                SpriteChange?.Invoke();
            }
        }

        private Sprite sprite;

        public event Action SpriteChange;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public Sprite[] GetSprites()
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/terrain");
            Debug.Log("Length is: " + sprites.Length);
            return sprites;
        }
    }
}
