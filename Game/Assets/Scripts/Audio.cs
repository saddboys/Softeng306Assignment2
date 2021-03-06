﻿using System.Collections;
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
        private AudioClip intro;

        public float Volume
        {
            get { return audioSource.volume; }
            set { audioSource.volume = value; }
        }

        public AudioManager()
        {
            var obj = new GameObject("AudioManager");
            obj.AddComponent<AudioBehaviour>().Audio = this;
            audioSource = obj.AddComponent<AudioSource>();
            mainLoop = Resources.Load<AudioClip>("Music/TestLoop");
            intro = Resources.Load<AudioClip>("Music/Intro");
            audioSource.clip = intro;
            audioSource.loop = false;
            audioSource.Play();
        }

        public void Play(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public void StopMusic()
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        public void StartMusic()
        {
            audioSource.clip = mainLoop;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public class AudioBehaviour : MonoBehaviour
    {
        public AudioManager Audio { get; set; }

        private void Start()
        {
            foreach (var obj in Resources.FindObjectsOfTypeAll(typeof(Button)))
            {
                AttachButton((Selectable)obj);
            }
            foreach (var obj in Resources.FindObjectsOfTypeAll(typeof(Toggle)))
            {
                AttachButton((Selectable)obj);
            }
        }

        public void Play(AudioClip clip)
        {
            Audio.Play(clip);
        }

        public void AttachButton(Selectable button)
        {
            var effects = button.gameObject.AddComponent<ButtonSoundEffects>();
            effects.Audio = Audio;
        }

        public void StopMusic()
        {
            Audio.StopMusic();
        }

        public void StartMusic()
        {
            Audio.StartMusic();
        }
    }

    public class ButtonSoundEffects : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        private AudioClip hover;
        private AudioClip click;
        private Selectable selectable;

        public AudioManager Audio { get; set; }

        private void Start()
        {
            selectable = (Selectable)gameObject.GetComponent<Button>() ?? gameObject.GetComponent<Toggle>();
            hover = Resources.Load<AudioClip>("SoundEffects/Hover");
            click = Resources.Load<AudioClip>("SoundEffects/Click");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!selectable.interactable) return;
            Audio?.Play(click);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!selectable.interactable) return;
            Audio?.Play(hover);
        }
    }
}
