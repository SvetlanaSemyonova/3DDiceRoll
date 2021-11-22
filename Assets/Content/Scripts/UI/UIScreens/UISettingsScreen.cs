using System;
using Content.Scripts.Managers;
using Content.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts.UI.UIScreens
{
    [Serializable]
    public class ChangeDataStatusModel
    {
        public Sprite spriteIfEnabled;
        public Sprite buttonIfEnabled;
        public Sprite spriteIfDisabled;
        public Sprite buttonIfDisabled;
        public Image modelView;
        public Image buttonView;

        public void ChangeView(bool isEnabled)
        {
            modelView.sprite = isEnabled ? spriteIfEnabled : spriteIfDisabled;
            buttonView.sprite = isEnabled ? buttonIfEnabled : buttonIfDisabled;
        }
    }

    public class UISettingsScreen : MonoBehaviour
    {
        public ChangeDataStatusModel musicModel;

        [SerializeField] private ChangeDataStatusModel soundModel;

        [SerializeField] private ChangeDataStatusModel vibrationModel;

        public ChangeDataStatusModel MusicModel => musicModel;
        public ChangeDataStatusModel SoundModel => soundModel;
        public ChangeDataStatusModel VibrationModel => vibrationModel;

        private void OnEnable()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.ClickButton);
            }
            
            SoundModel.ChangeView(PlayerData.Instance.settings.Value.isSoundEnable);
            MusicModel.ChangeView(PlayerData.Instance.settings.Value.isMusicEnable);
            VibrationModel.ChangeView(PlayerData.Instance.settings.Value.isVibrationEnable);
        }

        public void CloseButton()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.ClickButton);
            }
            
            PlayerData.Instance.SaveAll();

            gameObject.SetActive(false);
        }
    }
}