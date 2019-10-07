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
        //[SerializeField] private Toggle[] toggles;

        public StructureFactory CurrentFactory { get; set; }
        private StructureFactory[] factories;

        public event Action BuiltEvent;

        public ToolBar(City city) { }

        // Start is called before the first frame update
        void Start() {
            factories = new StructureFactory[]
            {
                new HouseFactory(city),
                new FactoryFactory(city),
                new ParkFactory(city),
                new PowerPlantFactory(city),
                new DockFactory(city),
                new DemolishFactory(),
            };

            city.Map.TileClickedEvent += (s, e) =>
            {
                // TODO: handle when the tile e.Tile has been clicked.
                Debug.Log(e.Tile);
                // throw new System.NotImplementedException();

                OnNotify(e.Tile);
            };

            city.Stats.ChangeEvent += () =>
            {
                for (int i = 0; i < factories.Length; i++)
                {
                    if (!factories[i].CanBuild(out string reason))
                    {
                        // Disable toggles[i]
                    }
                }
            };

            // foreach toggle, add listener.
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnNotify(MapTile tile) {
            if (CurrentFactory == null)
            {
                return;
            }
            if (!CurrentFactory.CanBuildOnto(tile, out string reason))
            {
                Debug.Log(reason); // TODO: show to user.
                return;
            }
            CurrentFactory.BuildOnto(tile);
            BuiltEvent?.Invoke();
        }

        void BuildStructure(StructureFactory factory, MapTile tile) {
            factory.BuildOnto(tile);
        }

        // Called whenever a toggle is toggled on
        public void Toggle01A( bool isOn ) {
            if (isOn) CurrentFactory = new HouseFactory(city);
            else CurrentFactory = null;
        }

        public void Toggle01B( bool isOn )
        {
            if (isOn) CurrentFactory = new FactoryFactory(city);
            else CurrentFactory = null;
        }

        public void Toggle02A( bool isOn )
        {
            if (isOn) CurrentFactory = new ParkFactory(city);
            else CurrentFactory = null;
        }

        public void Toggle02B( bool isOn ) {
            if (isOn) CurrentFactory = new DockFactory(city);
            else CurrentFactory = null;
        }

        public void Toggle03A( bool isOn )
        {
            if (isOn) CurrentFactory = new PowerPlantFactory(city);
            else CurrentFactory = null;
        }
        public void Toggle03B( bool isOn )
        {
            if (isOn) CurrentFactory = null;
            else CurrentFactory = null;
        }

        public void OnRmToggleValueChanged( bool isOn )
        {
            if (isOn) CurrentFactory = new DemolishFactory();
            else CurrentFactory = null;
        }
    }
}
