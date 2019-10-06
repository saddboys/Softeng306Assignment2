using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.CityMap;
using UnityEngine.UI;
using System;
namespace Game
{
    public class ToolBar : MonoBehaviour
    {
        [SerializeField] private City city;
        [SerializeField] private Toggle toggle ;
        private Rock rock;
        private bool rockBtnSelected, rmBtnSelected;

        public ToolBar(City city) { }

        // Start is called before the first frame update
        void Start() {
            city.Map.TileClickedEvent += (s, e) =>
            {
                // TODO: handle when the tile e.Tile has been clicked.
                Debug.Log(e.Tile);
                // throw new System.NotImplementedException();

                OnNotify(e.Tile);
            };
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnNotify(MapTile tile) {
            //check if one of the toggle is selected
            if (rockBtnSelected) {
                //check if the tile is free
                if(tile.Structure == null){
                tile.Structure = new Rock();
                }
                // build the structure
                
                //tile.Terrain.Sprite = Resources.LoadAll<Sprite>("Textures/terrain")[28]; 
                // StructureFactory factory = new RockFactory(city);
                // factory.BuildOnto(tile);
            }else if(rmBtnSelected){
                 tile.Structure = null;
            }

        }

    // Called whenever something is toggled on
     public void OnToggleValueChanged( bool isOn ) {
         rockBtnSelected = isOn;
     }

      public void OnRmToggleValueChanged( bool isOn ) {
         rmBtnSelected = isOn;
     }

    }
}
