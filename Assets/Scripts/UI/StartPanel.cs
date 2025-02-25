using UnityEngine;
using UnityEngine.UI;
using Scripts.Managers;

namespace Scripts.UI
{
    public class StartPanel : PanelBase
    {
        [SerializeField] private Button m_StartButton;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_StartButton.onClick.AddListener(GameManager.Ins.OnGameStart);
        }
    }
}
