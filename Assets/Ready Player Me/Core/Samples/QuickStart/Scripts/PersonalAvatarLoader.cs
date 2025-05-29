using System;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.Analytics;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.Samples.QuickStart
{
    public class PersonalAvatarLoader : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Text openPersonalAvatarPanelButtonText;
        [SerializeField] private Text linkText;
        [SerializeField] private InputField avatarUrlField;
        [SerializeField] private Button openPersonalAvatarPanelButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button linkButton;
        [SerializeField] private Button loadAvatarButton;
        [SerializeField] private GameObject avatarLoading;
        [SerializeField] private GameObject personalAvatarPanel;

        [Header("Character Managers")]
        [SerializeField] private ThirdPersonLoader thirdPersonLoader;
        [SerializeField] private CameraOrbit cameraOrbit;
        [SerializeField] private ThirdPersonController thirdPersonController;
        public string CurrentAvatarUrl => avatarUrlField.text;

        private string defaultButtonText;

        private void Start()
        {
            AnalyticsRuntimeLogger.EventLogger.LogRunQuickStartScene();

            string savedAvatarUrl = PlayerPrefs.GetString("AvatarUrl", "");
            if (!string.IsNullOrEmpty(savedAvatarUrl))
            {
                avatarUrlField.text = savedAvatarUrl;
                thirdPersonLoader.LoadAvatar(savedAvatarUrl);
            }
        }


        private void OnEnable()
        {
            openPersonalAvatarPanelButton.onClick.AddListener(OnOpenPersonalAvatarPanel);
            closeButton.onClick.AddListener(OnCloseButton);
            linkButton.onClick.AddListener(OnLinkButton);
            loadAvatarButton.onClick.AddListener(OnLoadAvatarButton);
            avatarUrlField.onValueChanged.AddListener(OnAvatarUrlFieldValueChanged);
        }

        private void OnDisable()
        {
            openPersonalAvatarPanelButton.onClick.RemoveListener(OnOpenPersonalAvatarPanel);
            closeButton.onClick.RemoveListener(OnCloseButton);
            linkButton.onClick.RemoveListener(OnLinkButton);
            loadAvatarButton.onClick.RemoveListener(OnLoadAvatarButton);
            avatarUrlField.onValueChanged.RemoveListener(OnAvatarUrlFieldValueChanged);
        }

        private void OnOpenPersonalAvatarPanel()
        {
            linkText.text = $"https://{CoreSettingsHandler.CoreSettings.Subdomain}.readyplayer.me";
            personalAvatarPanel.SetActive(true);
            SetActiveThirdPersonalControls(false);
            AnalyticsRuntimeLogger.EventLogger.LogLoadPersonalAvatarButton();
        }

        private void OnCloseButton()
        {
            SetActiveThirdPersonalControls(true);
            personalAvatarPanel.SetActive(false);
        }

        private void OnLinkButton()
        {
            Application.OpenURL(linkText.text);
        }

        private void OnLoadAvatarButton()
        {
            thirdPersonLoader.OnLoadComplete += OnLoadComplete;
            defaultButtonText = openPersonalAvatarPanelButtonText.text;
            SetActiveLoading(true, "Loading...");

            string url = avatarUrlField.text;
            thirdPersonLoader.LoadAvatar(url);

            // ?? Guardar la URL del avatar
            PlayerPrefs.SetString("AvatarUrl", url);
            PlayerPrefs.Save();

            personalAvatarPanel.SetActive(false);
            SetActiveThirdPersonalControls(true);

            AnalyticsRuntimeLogger.EventLogger.LogPersonalAvatarLoading(url);
        }


        private void OnAvatarUrlFieldValueChanged(string url)
        {
            if (!string.IsNullOrEmpty(url) && Uri.TryCreate(url, UriKind.Absolute, out Uri _))
            {
                loadAvatarButton.interactable = true;
            }
            else
            {
                loadAvatarButton.interactable = false;
            }
        }

        private void OnLoadComplete()
        {
            thirdPersonLoader.OnLoadComplete -= OnLoadComplete;
            SetActiveLoading(false, defaultButtonText);
        }

        private void SetActiveLoading(bool enable, string text)
        {
            openPersonalAvatarPanelButtonText.text = text;
            openPersonalAvatarPanelButton.interactable = !enable;
            avatarLoading.SetActive(enable);
        }

        private void SetActiveThirdPersonalControls(bool enable)
        {
            cameraOrbit.enabled = enable;
            thirdPersonController.enabled = enable;
        }
    }
}
