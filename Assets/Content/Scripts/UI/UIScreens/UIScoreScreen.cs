using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Content.Scripts.UI.UIScreens
{
    public class UIScoreScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lastScore;
        [SerializeField] private TextMeshProUGUI previousScore_3;
        [SerializeField] private TextMeshProUGUI previousScore_2;
        [SerializeField] private TextMeshProUGUI previousScore_1;
        
        private List<int> m_scoreData = new List<int>();

        private void Start()
        {
            m_scoreData = PlayerData.Instance.scoreData.Value;
            previousScore_1.text = m_scoreData[0].ToString();
            previousScore_2.text = m_scoreData[1].ToString();
            previousScore_3.text = m_scoreData[2].ToString();
            lastScore.text = m_scoreData[3].ToString();
        }
    }
}