using UnityEngine;

namespace Content.Scripts.UI
{
    public class UiRateUs : MonoBehaviour
    {
        public void OpenStore()
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=");
        }
    }
}