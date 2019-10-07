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
        [SerializeField] private Toggle[] toggles;
        [SerializeField] private GameObject popupInfo;
        private int popupInfoCount = 0;

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

            city.Stats.ChangeEvent += UpdateToggleEnabled;
            Invoke("UpdateToggleEnabled", 0.1f);

            foreach (var t in toggles)
            {
                t.interactable = false;
            }

            for (int i = 0; i < factories.Length; i++)
            {
                toggles[i].interactable = true;
                addToggleHandler(toggles[i], factories[i]);
            }
        }

        private void UpdateToggleEnabled()
        {
            for (int i = 0; i < factories.Length; i++)
            {
                toggles[i].interactable = factories[i].CanBuild(out _);
                if (toggles[i].isOn && !toggles[i].interactable)
                {
                    toggles[i].isOn = false;
                }
            }
        }

        private void addToggleHandler(Toggle toggle, StructureFactory factory)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    CurrentFactory = factory;
                }
                else
                {
                    CurrentFactory = null;
                }
            });
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
                ShowPopupInfo(reason);
                return;
            }
            CurrentFactory.BuildOnto(tile);
            BuiltEvent?.Invoke();
        }

        void BuildStructure(StructureFactory factory, MapTile tile) {
            factory.BuildOnto(tile);
        }

        void ShowPopupInfo(string text)
        {
            popupInfo.GetComponentInChildren<Text>().text = text;
            popupInfo.transform.position = Input.mousePosition;
            popupInfo.SetActive(true);
            Invoke("HidePopupInfo", 2);
            popupInfoCount++;
        }

        void HidePopupInfo()
        {
            popupInfoCount--;
            if (popupInfoCount == 0)
            {
                popupInfo.SetActive(false);
            }
        }
    }
}
