// Sound Manager - Audio management
// Version: 0.1.0
// Created: 2026-03-20

using UnityEngine;
using UnityEngine.Audio;

namespace OverlayElements.Audio
{
    /// <summary>
    /// Sound manager - handles all audio playback
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Mixer")]
        [SerializeField] private AudioMixer audioMixer;

        [Header("Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float masterVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float musicVolume = 0.7f;
        [Range(0f, 1f)]
        [SerializeField] private float sfxVolume = 1f;

        // Sound clips
        [Header("Music")]
        public AudioClip menuMusic;
        public AudioClip battleMusic;

        [Header("SFX - UI")]
        public AudioClip buttonClick;
        public AudioClip cardHover;
        public AudioClip cardPlay;
        public AudioClip turnStart;

        [Header("SFX - Combat")]
        public AudioClip attackHit;
        public AudioClip cardDestroy;
        public AudioClip heal;
        public AudioClip overlay;

        [Header("SFX - Elements")]
        public AudioClip fireSFX;
        public AudioClip waterSFX;
        public AudioClip windSFX;
        public AudioClip woodSFX;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Initialize audio
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
            }
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }

            UpdateVolumes();
        }

        /// <summary>
        /// Play music
        /// </summary>
        public void PlayMusic(AudioClip clip)
        {
            if (musicSource == null || clip == null) return;
            musicSource.clip = clip;
            musicSource.Play();
        }

        /// <summary>
        /// Play menu music
        /// </summary>
        public void PlayMenuMusic()
        {
            PlayMusic(menuMusic);
        }

        /// <summary>
        /// Play battle music
        /// </summary>
        public void PlayBattleMusic()
        {
            PlayMusic(battleMusic);
        }

        /// <summary>
        /// Play SFX
        /// </summary>
        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource == null || clip == null) return;
            sfxSource.PlayOneShot(clip, sfxVolume);
        }

        /// <summary>
        /// Play UI sound
        /// </summary>
        public void PlayUISound(UISound sound)
        {
            switch (sound)
            {
                case UISound.ButtonClick:
                    PlaySFX(buttonClick);
                    break;
                case UISound.CardHover:
                    PlaySFX(cardHover);
                    break;
                case UISound.CardPlay:
                    PlaySFX(cardPlay);
                    break;
                case UISound.TurnStart:
                    PlaySFX(turnStart);
                    break;
            }
        }

        /// <summary>
        /// Play combat sound
        /// </summary>
        public void PlayCombatSound(CombatSound sound)
        {
            switch (sound)
            {
                case CombatSound.AttackHit:
                    PlaySFX(attackHit);
                    break;
                case CombatSound.CardDestroy:
                    PlaySFX(cardDestroy);
                    break;
                case CombatSound.Heal:
                    PlaySFX(heal);
                    break;
                case CombatSound.Overlay:
                    PlaySFX(overlay);
                    break;
            }
        }

        /// <summary>
        /// Play element sound
        /// </summary>
        public void PlayElementSound(Card.ElementType element)
        {
            switch (element)
            {
                case Card.ElementType.Fire:
                    PlaySFX(fireSFX);
                    break;
                case Card.ElementType.Water:
                    PlaySFX(waterSFX);
                    break;
                case Card.ElementType.Wind:
                    PlaySFX(windSFX);
                    break;
                case Card.ElementType.Wood:
                    PlaySFX(woodSFX);
                    break;
            }
        }

        /// <summary>
        /// Update volume settings
        /// </summary>
        public void UpdateVolumes()
        {
            if (audioMixer != null)
            {
                audioMixer.SetFloat("Master", Mathf.Log10(masterVolume) * 20);
                audioMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
                audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
            }
        }

        /// <summary>
        /// Set master volume
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        /// <summary>
        /// Set music volume
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        /// <summary>
        /// Set SFX volume
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }
    }

    public enum UISound
    {
        ButtonClick,
        CardHover,
        CardPlay,
        TurnStart
    }

    public enum CombatSound
    {
        AttackHit,
        CardDestroy,
        Heal,
        Overlay
    }
}
