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
        public StructureFactory currentFactory;

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
            currentFactory?.BuildOnto(tile);
        }

        void BuildStructure(StructureFactory factory, MapTile tile) {
            factory.BuildOnto(tile);
        }

        // Called whenever a toggle is toggled on
        public void Toggle01A( bool isOn ) {
            if (isOn) currentFactory = new HouseFactory(city);
        }

        public void Toggle01B( bool isOn )
        {
            if (isOn) currentFactory = new FactoryFactory(city);
        }

        public void Toggle02A( bool isOn )
        {
            if (isOn) currentFactory = new ParkFactory(city);
        }

        public void Toggle02B( bool isOn ) {
            if (isOn) currentFactory = null;
        }

        public void Toggle03A( bool isOn )
        {
            if (isOn) currentFactory = null;
        }
        public void Toggle03B( bool isOn )
        {
            if (isOn) currentFactory = null;
        }

        public void OnRmToggleValueChanged( bool isOn )
        {
            if (isOn) currentFactory = new DemolishFactory();
        }
    }
}
