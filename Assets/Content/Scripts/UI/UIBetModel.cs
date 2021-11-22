using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts.UI
{
    public class UIBetModel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button closeButton;
        
        public event Action<int> OnAcceptButtonClick = delegate(int i) {  };
        public event Action OnCloseButtonClick = delegate {  };

        private void Start()
        {
            acceptButton.onClick.AddListener(OnAcceptButtonClickHandler);
            closeButton.onClick.AddListener(OnCloseButtonClickHandler);
        }

        private void OnDestroy()
        {
            acceptButton.onClick.RemoveListener(OnAcceptButtonClickHandler);
            closeButton.onClick.RemoveListener(OnCloseButtonClickHandler);
        }
        
        private void OnAcceptButtonClickHandler()
        {
            OnAcceptButtonClick.Invoke(Convert.ToInt32(inputField.text));
        }

        private void OnCloseButtonClickHandler()
        {
            OnCloseButtonClick.Invoke();
        }
    }
}
