using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Controller : MonoBehaviour
    {
        private AudioManager audioManager;

        [SerializeField]
        private GameObject menu;

        [SerializeField]
        private GameObject statsBar;

        [SerializeField]
        private Slider volumeSlider;

        // This links to the JS function inside the plugins folder.
        [DllImport("__Internal")]
        private static extern void WebGLExit();

        private AudioClip click;

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#elif UNITY_WEBGL
            // Restart the game.
            // Note: Button may not exist (not active) if quiting outside of menu.
            GameObject
                .Find("Restart Button")
                ?.GetComponent<Button>()
                ?.onClick.Invoke();

            // Exit fullscreen.
            Screen.fullScreen = false;

            // Show html exit screen.
            WebGLExit();
#endif
        }

        private void Start()
        {
#if UNITY_WEBGL
            Screen.fullScreen = true;
#endif
            audioManager = new AudioManager();

            volumeSlider.onValueChanged.AddListener((float value) =>
            {
                audioManager.Volume = value;
            });
            volumeSlider.value = audioManager.Volume;

            click = Resources.Load<AudioClip>("SoundEffects/Click");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && statsBar.activeSelf)
            {
                menu.SetActive(!menu.activeSelf);
                audioManager.Play(click);
            }
        }
    }
}
