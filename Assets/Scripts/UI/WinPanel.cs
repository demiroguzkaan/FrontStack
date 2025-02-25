using UnityEngine;
using UnityEngine.UI;
using Scripts.Managers;

namespace Scripts.UI
{
    public class WinPanel : PanelBase
    {
        [SerializeField] private Button m_NextLevelButton;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_NextLevelButton.onClick.AddListener(() => { GameManager.Ins.OnGameStart(); });
        }
    }
}
