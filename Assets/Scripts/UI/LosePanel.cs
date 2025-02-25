using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class LosePanel : PanelBase
    {
        [SerializeField] private Button m_RestartButton;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_RestartButton.onClick.AddListener(() => { SceneManager.LoadScene(0); });
        }
    }
}
