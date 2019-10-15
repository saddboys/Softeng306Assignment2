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

        public StructureFactory CurrentFactory
        {
            get { return currentFactory; }
            set
            {
                currentFactory = value;
                if (currentFactory != null)
                {
                    Ghost = currentFactory.CreateGhost();
                }
            }
        }
        private StructureFactory currentFactory;
        private StructureFactory[] factories;
        private Structure ghost;
        private Structure Ghost
        {
            get { return ghost; }
            set
            {
                ghost?.Unrender();
                ghost = value;
                if (ghostTile != null)
                {
                    ShowGhostOnTile(ghostTile);
                }
            }
        }
        private MapTile ghostTile;

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
                new ForestFactory(city), 
                new WindFarmFactory(city), 
                new SolarFarmFactory(city), 
                //Always Last
                new DemolishFactory(city),
            };

            city.Map.TileClickedEvent += (s, e) =>
            {
                OnNotify(e.Tile);
            };

            city.Map.TileMouseEnterEvent += (s, e) => ShowGhostOnTile(e.Tile);
            city.Map.TileMouseLeaveEvent += (s, e) => HideGhostOnTile(e.Tile);

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
                if (factories[i].Sprite != null)
                {
                    // Blue background when isOn.
                    Image checkedImage = toggles[i]
                        .GetComponentsInChildren<Image>()[1];
                    checkedImage.color = new Color(0.6f, 0.8f, 1.0f);
                    checkedImage.sprite = toggles[i].GetComponentsInChildren<Image>()[0].sprite;
                    checkedImage.type = Image.Type.Sliced;
                    RectTransform transform = checkedImage.GetComponent<RectTransform>();
                    transform.SetParent(toggles[i].GetComponentInChildren<Image>().transform);
                    transform.anchorMin = new Vector2(0, 0);
                    transform.anchorMax = new Vector2(1, 1);
                    transform.offsetMin = new Vector2(0, 0);
                    transform.offsetMax = new Vector2(0, 0);

                    // Fully opaque structure image when isOn.
                    GameObject background = new GameObject();
                    Image checkedSprite = background.AddComponent<Image>();
                    checkedSprite.sprite = factories[i].Sprite;
                    transform = background.GetComponent<RectTransform>();
                    transform.SetParent(checkedImage.transform);
                    transform.anchorMin = new Vector2(0, 0);
                    transform.anchorMax = new Vector2(1, 1);
                    transform.offsetMin = new Vector2(2, 2);
                    transform.offsetMax = new Vector2(-2, -2);
                    background.SetActive(true);

                    // Show semi-transparent image when not isOn.
                    GameObject buttonSprite = new GameObject();
                    Image image = buttonSprite.AddComponent<Image>();
                    image.sprite = factories[i].Sprite;
                    image.color = new Color(1.0f, 1.0f, 1.0f, 0.7f);
                    transform = buttonSprite.GetComponent<RectTransform>();
                    transform.SetParent(toggles[i].GetComponentInChildren<Image>().transform);
                    transform.anchorMin = new Vector2(0, 0);
                    transform.anchorMax = new Vector2(1, 1);
                    transform.offsetMin = new Vector2(2, 2);
                    transform.offsetMax = new Vector2(-2, -2);
                    buttonSprite.SetActive(true);
                }
            }

            // Fix popup info tooltip's scaling.
            // (Don't want to modify game scene just for this)
            popupInfo.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            popupInfo.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            popupInfo.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            popupInfo.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
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

            // Update Ghosts
            HideGhostOnTile(tile);
            ShowGhostOnTile(tile);

            BuiltEvent?.Invoke();
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

        private void ShowGhostOnTile(MapTile tile)
        {
            if (CurrentFactory == null) return;
            if (!CurrentFactory.CanBuildOnto(tile, out _)) return;
            tile.HandleMouseEnter();

            if (Ghost == null) return;
            Ghost.RenderOnto(tile.Canvas, tile.ScreenPosition);
            foreach (var renderer in Ghost.GameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                Color color = renderer.color;
                color.a = 0.5f;
                renderer.color = color;
            }
        }

        private void HideGhostOnTile(MapTile tile)
        {
            tile.HandleMouseLeave();
            Ghost?.Unrender();
        }
    }
}
