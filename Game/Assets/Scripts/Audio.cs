using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// Game music and sound effects are handled here.
    /// </summary>
    public class AudioManager
    {
        private AudioSource audioSource;
        private AudioClip mainLoop;

        public AudioManager()
        {
            var obj = new GameObject("AudioManager");
            obj.AddComponent<AudioBehaviour>().Audio = this;
            audioSource = obj.AddComponent<AudioSource>();
            mainLoop = Resources.Load<AudioClip>("Music/TestLoop");
            audioSource.clip = mainLoop;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void Play(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public class AudioBehaviour : MonoBehaviour
    {
        public AudioManager Audio { get; set; }

        private void Start()
        {
            foreach (var obj in Resources.FindObjectsOfTypeAll(typeof(Button)))
            {
                var button = (Button)obj;
                var effects = button.gameObject.AddComponent<ButtonSoundEffects>();
                effects.Audio = Audio;
            }
            foreach (var obj in Resources.FindObjectsOfTypeAll(typeof(Toggle)))
            {
                var toggle = (Toggle)obj;
                var effects = toggle.gameObject.AddComponent<ButtonSoundEffects>();
                effects.Audio = Audio;
            }
        }

        public void Play(AudioClip clip)
        {
            Audio.Play(clip);
        }
    }

    public class ButtonSoundEffects : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        private AudioClip hover;
        private AudioClip click;

        public AudioManager Audio { get; set; }

        private void Start()
        {
            hover = Resources.Load<AudioClip>("SoundEffects/Hover");
            click = Resources.Load<AudioClip>("SoundEffects/Click");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Audio?.Play(click);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Audio?.Play(hover);
        }
    }
}
