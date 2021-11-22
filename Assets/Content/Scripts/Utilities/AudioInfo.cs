using System;
using System.Collections.Generic;
using UnityEngine;

namespace Content.Scripts.Utilities
{
    [Serializable]
    public class AudioData
    {
        public string id;
        public AudioDefault audioDefault;
        public AudioClip clip;
        [Range(0, 1)] public float volume;

        public AudioData(string id, AudioDefault audioDefault, AudioClip clip, float volume)
        {
            this.id = id;
            this.audioDefault = audioDefault;
            this.clip = clip;
            this.volume = volume;
        }
    }

    [CreateAssetMenu(fileName = "AudioInfo", menuName = "Sound/AudioInfo", order = 0)]
    public class AudioInfo : ScriptableObject
    {
        [SerializeField] private List<AudioData> data = new List<AudioData>();
        
        public AudioData GetAudioData(AudioDefault type)
        {
            foreach (var d in data)
            {
                if (d.audioDefault == type)
                {
                    return d;
                }
            }

            return null;
        }
    }

    public enum AudioDefault
    {
        Custom,
        BackGround,
        ClickButton,
        LevelFailed,
        LevelComplete,
        ShowPopup,
        ClosePopup,
        TapRight,
        TapWrong,
        ClickObject,
        Switch
    }
}