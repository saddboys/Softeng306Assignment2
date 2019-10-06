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
        private bool btnSelect01A, btnSelect01B, btnSelect02A, btnSelect02B, btnSelect03A, btnSelect03B, btnRemove;

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
            if (btnSelect01A) {
                //check if the tile is free
                if(tile.Structure == null){
                    tile.Structure = new Rock();
                    // build the structure
                    // BuildStructure(new HouseFactory(city), tile);
                }
            } 
            else if (btnSelect01B) {
                if (tile.Structure == null) {
                    BuildStructure(new ParkFactory(city), tile);
                }
            }
            else if (btnSelect02A) {
                if (tile.Structure == null) {
                    BuildStructure(new TowerFactory(city), tile);
                }
            } 
            else if (btnSelect02B) {
                if (tile.Structure == null) {
                    BuildStructure(new DocksFactory(city), tile);
                }
            } 
            else if (btnSelect03A) {
                if (tile.Structure == null) {
                    BuildStructure(new PowerPlantFactory(city), tile);
                }
            } 
            else if (btnSelect03B) {
                if (tile.Structure == null) {
                    BuildStructure(new FactoryFactory(city), tile);
                }
            } 
            else if (btnRemove){
                 tile.Structure = null;
            }

        }

        void BuildStructure(StructureFactory factory, MapTile tile) {
            factory.BuildOnto(tile);
        }

        // Called whenever a toggle is toggled on
        public void Toggle01A( bool isOn ) {
            btnSelect01A = isOn;
        }

        public void Toggle01B( bool isOn ) {
            btnSelect01B = isOn;
        }

        public void Toggle02A( bool isOn ) {
            btnSelect02A = isOn;
        }

        public void Toggle02B( bool isOn ) {
            btnSelect02B = isOn;
        }

        public void Toggle03A( bool isOn ) {
         btnSelect03A = isOn;
        }
        public void Toggle03B( bool isOn ) {
         btnSelect03B = isOn;
        }

        public void OnRmToggleValueChanged( bool isOn ) {
            btnRemove = isOn;
        }

    }
}
