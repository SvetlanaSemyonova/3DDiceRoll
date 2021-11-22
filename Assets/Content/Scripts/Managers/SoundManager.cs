using Content.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Content.Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private AudioSource effectsSource;
        [SerializeField] private bool playAmbientOnAwake = true;


        public AudioInfo audioInfo;
        public static SoundManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            SyncSoundWithSettings();

            ambientSource.volume = 0;
            effectsSource.volume = 0;

            if (playAmbientOnAwake)
            {
                PlayAmbient(AudioDefault.BackGround, true, 0, true, 5);
            }
        }

        private void Start()
        {
            PlayAmbient(AudioDefault.BackGround, true, 0f, false, 1f);
        }

        public void SyncSettingsWithPlayerSettings()
        {
            effectsSource.mute = !PlayerData.Instance.settings.Value.isMusicEnable;
            ambientSource.mute = !PlayerData.Instance.settings.Value.isMusicEnable;
        }


        public void ChangeMusicStatus()
        {
            PlayerData.Instance.settings.Value.isMusicEnable = !PlayerData.Instance.settings.Value.isMusicEnable;
            PlayerData.Instance.SaveAll();
            
            ambientSource.mute = !PlayerData.Instance.settings.Value.isMusicEnable;
        }

        public void ChangeSoundStatus()
        {
            PlayerData.Instance.settings.Value.isSoundEnable = !PlayerData.Instance.settings.Value.isSoundEnable;
            PlayerData.Instance.SaveAll();

            effectsSource.mute = !PlayerData.Instance.settings.Value.isSoundEnable;
        }


        public void PlayAmbient(AudioDefault audioId, bool loop = true, float delayBeforeStart = 0,
            bool fromFade = false, float durationForFade = 1)
        {
            AudioData audioClip = audioInfo.GetAudioData(audioId);
            if (audioClip != null)
            {
                PlayAmbient(audioClip, loop, delayBeforeStart, fromFade, durationForFade);
            }
            else
            {
                Debug.LogError("Haven't such audioClip " + audioId);
            }
        }

        private void PlayAmbient(AudioData audioClip, bool loop = true, float delayBeforeStart = 0,
            bool fromFade = false, float durationForFade = 1)
        {
            if (ambientSource.isPlaying)
            {
                ambientSource.Stop();
            }

            ambientSource.clip = audioClip.clip;
            if (fromFade)
            {
                ambientSource.DOFade(audioClip.volume, durationForFade).SetDelay(delayBeforeStart);
            }
            else
            {
                ambientSource.volume = audioClip.volume;
            }

            ambientSource.PlayDelayed(delayBeforeStart);
            ambientSource.loop = loop;
        }
        

        public void PlayEffect(AudioDefault audioId)
        {
            AudioData audioClip = audioInfo.GetAudioData(audioId);
            if (audioClip != null)
            {
                effectsSource.volume = audioClip.volume;
                effectsSource.PlayOneShot(audioClip.clip);
            }
            else
            {
                Debug.LogError("Haven't such audioClip " + audioId);
            }
        }


        private void MuteEffectSounds()
        {
            var findObjectsOfType = FindObjectsOfType<AudioSource>();
            if (findObjectsOfType == null || findObjectsOfType.Length <= 0)
            {
                return;
            }

            foreach (var audioSource in findObjectsOfType)
            {
                if (audioSource != ambientSource)
                {
                    audioSource.mute = true;
                }
            }
        }

        private void UnMuteEffectSounds()
        {
            var findObjectsOfType = FindObjectsOfType<AudioSource>();
            if (findObjectsOfType == null || findObjectsOfType.Length <= 0)
            {
                return;
            }

            foreach (var audioSource in findObjectsOfType)
            {
                if (audioSource != ambientSource)
                {
                    audioSource.mute = false;
                }
            }
        }

        private void MuteAmbient()
        {
            ambientSource.mute = true;
        }

        private void UnMuteAmbient()
        {
            ambientSource.mute = false;
        }

        public void SyncSoundWithSettings()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                UnMuteEffectSounds();
            }
            else
            {
                MuteEffectSounds();
            }


            if (PlayerData.Instance.settings.Value.isMusicEnable)
            {
                UnMuteAmbient();
            }
            else
            {
                MuteAmbient();
            }
        }
    }
}