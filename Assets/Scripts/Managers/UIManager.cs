using Scripts.UI;
using UnityEngine;
using Scripts.Enums;
using System.Collections.Generic;

namespace Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager m_Ins;
        public static UIManager Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = FindObjectOfType<UIManager>();
                return m_Ins;
            }
        }

        [SerializeField] private List<PanelBase> m_Panels;

        private void Start()
        {
            OpenPanel(PanelType.StartPanel);

            GameManager.Ins.onGameStart += () => { ClosePanel(PanelType.StartPanel); ClosePanel(PanelType.WinPanel); };
            GameManager.Ins.onGameLose += () => { OpenPanel(PanelType.LosePanel); };
            GameManager.Ins.onGameWin += () => { OpenPanel(PanelType.WinPanel); };
        }

        public void OpenPanel(PanelType panelType)
        {
            var sound = m_Panels.Find(x => x.panelType == panelType);
            if (sound != null)
            {
                sound.gameObject.SetActive(true);
            }
        }

        public void ClosePanel(PanelType panelType)
        {
            var sound = m_Panels.Find(x => x.panelType == panelType);
            if (sound != null)
            {
                sound.gameObject.SetActive(false);
            }
        }
    }
}
