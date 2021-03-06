﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.CityMap;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Game
{
    public class ToolBar : MonoBehaviour
    {
        [SerializeField] private City city;
        [SerializeField] private Toggle[] toggles;
        [SerializeField] private GameObject popupInfo;
        private int popupInfoCount = 0;

        private InfoBox infoBox;

        private AudioClip invalidSound;
        private AudioClip keyboardSelectSound;

        public StructureFactory CurrentFactory
        {
            get { return currentFactory; }
            set
            {
                if (currentFactory == value) return;

                // Update the toolbar toggles if CurrentFactory manually set.
                // (I.e. not due to user clicking the toggles themselves, but by an event).
                int newToggleIdx = Array.IndexOf(factories, value);
                int oldToggleIdx = Array.IndexOf(factories, currentFactory);
                if (newToggleIdx < 0)
                {
                    if (oldToggleIdx >= 0 && toggles[oldToggleIdx].isOn)
                    {
                        // Manually clear previous toggle.
                        toggles[oldToggleIdx].isOn = false;
                    }
                }
                else if (!toggles[newToggleIdx].isOn)
                {
                    // Manually set new toggle.
                    toggles[newToggleIdx].isOn = true;
                }

                currentFactory = value;

                if (currentFactory != null)
                {
                    Ghost = currentFactory.CreateGhost();
                }
                infoBox.SetInfo(currentFactory);
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
                HideGhostOnTile(ghostTile);
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
                    Image[] images = toggles[i].GetComponentsInChildren<Image>();
                    if (images.Length < 2)
                    {
                        // Checkmark is missing. Add our own.
                        var checkedObject = new GameObject();
                        images = new Image[2]
                        {
                            images[0],
                            checkedObject.AddComponent<Image>(),
                        };
                        toggles[i].graphic = images[1];
                        images[1].canvasRenderer.SetAlpha(0);
                    }

                    // Blue background when isOn.
                    Image checkedImage = images[1];
                    checkedImage.color = new Color(0.6f, 0.8f, 1.0f);
                    checkedImage.sprite = images[0].sprite;
                    checkedImage.type = Image.Type.Sliced;
                    RectTransform transform = checkedImage.GetComponent<RectTransform>();
                    transform.SetParent(images[0].transform);
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
                    transform.SetParent(images[0].transform);
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

            // Monkeypatch the header and popup.
            Text header = GetComponentInChildren<Text>();
            header.font = Resources.Load<Font>("Fonts/visitor1");
            header.material = Resources.Load<Material>("Fonts/visitor1");
            Shadow shadow = header.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0);
            Text popupInfoText = popupInfo.GetComponentInChildren<Text>();
            popupInfoText.font = Resources.Load<Font>("Fonts/visitor1");
            popupInfoText.material = Resources.Load<Material>("Fonts/visitor1");
            popupInfoText.color = Color.white;
            shadow = popupInfoText.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0);
            popupInfo.GetComponent<Image>().color = GetComponent<Image>().color;

            infoBox = new InfoBox(gameObject.transform.parent.gameObject);

            invalidSound = Resources.Load<AudioClip>("SoundEffects/Invalid");
            keyboardSelectSound = Resources.Load<AudioClip>("SoundEffects/Click");

            // Clear current factory when game has restarted.
            // Don't let them build stuff in the start screen.
            city.RestartGameEvent += () => CurrentFactory = null;
        }

        private void Update()
        {
            // Keys 1 to n to select the nth factory.
            if (Input.anyKeyDown && Input.inputString.Length > 0)
            {
                char c = Input.inputString[0];
                int x = c - '1';
                if (x < 0 || x >= factories.Length) return;
                if (!factories[x].CanBuild(out string reason))
                {
                    ShowPopupInfo(reason);
                }
                else
                {
                    GameObject.FindObjectOfType<AudioBehaviour>().Play(keyboardSelectSound);
                    CurrentFactory = factories[x];
                    toggles[x].isOn = true;
                }
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
            var trigger = toggle.gameObject.AddComponent<EventTrigger>();
            var enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => infoBox.SetInfo(factory));
            var exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => infoBox.SetInfo(currentFactory));
            var clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener((data) =>
            {
                if (!factory.CanBuild(out string reason))
                {
                    ShowPopupInfo(reason);
                }
            });
            trigger.triggers.Add(enterEntry);
            trigger.triggers.Add(exitEntry);
            trigger.triggers.Add(clickEntry);
        }

        void OnNotify(MapTile tile) {
            if (CurrentFactory == null)
            {
                infoBox.SetInfo(tile.Structure);
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
            GameObject.FindObjectOfType<AudioBehaviour>().Play(invalidSound);
            popupInfo.GetComponentInChildren<Text>().text = text;

            // Pivot popup/tooltip on the side towards the centre of the screen
            // to prevent clipping.
            popupInfo.GetComponent<RectTransform>().pivot = new Vector2
            {
                x = Input.mousePosition.x < Screen.width / 2 ? 0 : 1,
                y = Input.mousePosition.y < Screen.height / 2 ? 0 : 1,
            };

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
            ghostTile = tile;

            if (CurrentFactory == null)
            {
                tile.HandleMouseEnter();
                return;
            }
            if (!CurrentFactory.CanBuildOnto(tile, out _)) return;
            tile.HandleMouseEnter();

            if (Ghost == null) return;
            Ghost.Tile = tile;
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
            tile?.HandleMouseLeave();
            Ghost?.Unrender();
        }
    }
}
