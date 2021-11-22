using System.Threading.Tasks;
using Content.Scripts.UI.UIScreens;
using Content.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Content.Scripts.Managers
{
    public class PanelManager : MonoBehaviour
    {
        public static PanelManager Instance;

        [SerializeField] private GameObject loadingScreenPanel;
        [SerializeField] private GameObject gameInfoPanel;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        [SerializeField] private UIScoreScreen scorePanel;
        [SerializeField] private UISettingsScreen settingsMenuPanel;


#if UNITY_EDITOR
        bool IsEditorAdsStarted = false;
#endif

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void OnStartScreenTap()
        {
            loadingScreenPanel.SetActive(false);
            settingPanel.gameObject.SetActive(true);
            gameInfoPanel.gameObject.SetActive(true);

            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.ClickButton);
            }
        }

        public void ShowSettingsMenu()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.ClickButton);
            }

            settingsMenuPanel.gameObject.SetActive(true);
        }

        public void ShowScoreMenu()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.ClickButton);
            }

            scorePanel.gameObject.SetActive(true);
        }

        public void HideSettingsMenu()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.ClickButton);
            }

            settingsMenuPanel.gameObject.SetActive(false);
        }

        public void HideScoreMenu()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.ClickButton);
            }

            scorePanel.gameObject.SetActive(false);
        }

        public void ShowLoadingPanel()
        {
            loadingScreenPanel.SetActive(true);
            DisableGamePanels();
        }

        private void Start()
        {
            loadingScreenPanel.SetActive(true);
            DisableGamePanels();
        }


        private void CloseWinPanel()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.ClickButton);
            }

            winPanel.gameObject.SetActive(false);
        }

        private void CloseLosePanel()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.ClickButton);
            }

            losePanel.gameObject.SetActive(false);
        }


        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        public void ShowWinPanel()
        {
            loadingScreenPanel.gameObject.SetActive(false);
            losePanel.gameObject.SetActive(false);
            settingsMenuPanel.gameObject.SetActive(false);

            settingPanel.gameObject.SetActive(true);
            winPanel.gameObject.SetActive(true);
        }


        public void ShowLosePanel()
        {
            loadingScreenPanel.gameObject.SetActive(false);
            settingsMenuPanel.gameObject.SetActive(false);
            winPanel.gameObject.SetActive(false);

            settingPanel.gameObject.SetActive(true);
            losePanel.gameObject.SetActive(true);
        }


        private void DisableGamePanels()
        {
            winPanel.gameObject.SetActive(false);
            losePanel.gameObject.SetActive(false);
            settingPanel.gameObject.SetActive(false);
            settingsMenuPanel.gameObject.SetActive(false);
        }

        private async void EnablePanels()
        {
            loadingScreenPanel.gameObject.SetActive(false);
            winPanel.gameObject.SetActive(false);
            losePanel.gameObject.SetActive(false);
            settingsMenuPanel.gameObject.SetActive(false);

            settingPanel.gameObject.SetActive(true);
            settingPanel.gameObject.SetActive(true);
#if UNITY_EDITOR
            if (IsEditorAdsStarted)
            {
                await Task.Delay(5000);
            }
#endif
        }

        public void ChangeSoundStatus()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.Switch);
            }

            PlayerData.Instance.settings.Value.isSoundEnable = !PlayerData.Instance.settings.Value.isSoundEnable;
            PlayerData.Instance.SaveAll();
            
            SoundManager.Instance.SyncSettingsWithPlayerSettings();
            settingsMenuPanel.SoundModel.ChangeView(PlayerData.Instance.settings.Value.isSoundEnable);
        }

        public void ChangeMusicStatus()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.Switch);
            }

            PlayerData.Instance.settings.Value.isMusicEnable = !PlayerData.Instance.settings.Value.isMusicEnable;
            PlayerData.Instance.SaveAll();
            
            SoundManager.Instance.SyncSettingsWithPlayerSettings();
            settingsMenuPanel.MusicModel.ChangeView(PlayerData.Instance.settings.Value.isMusicEnable);
        }


        public void ChangeVibrationStatus()
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.Switch);
            }

            PlayerData.Instance.settings.Value.isVibrationEnable =
                !PlayerData.Instance.settings.Value.isVibrationEnable;
            PlayerData.Instance.SaveAll();
            settingsMenuPanel.VibrationModel.ChangeView(PlayerData.Instance.settings.Value.isVibrationEnable);
        }
    }
}