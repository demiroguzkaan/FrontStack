using UnityEngine;

namespace Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager m_Ins;
        public static LevelManager Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = FindObjectOfType<LevelManager>();
                return m_Ins;
            }
        }

        [SerializeField] private Vector2Int m_StartMinMaxRoadCount;
        [SerializeField] private int m_MinMaxIncreaseAmountByLevel;

        private int m_Level => PlayerPrefs.GetInt(LEVEL_KEY, 0);

        private Vector2Int m_MinMaxRoadCount;
        private int m_CurrentLevelRoadCount;

        private const string LEVEL_KEY = "Level";

        private void Start()
        {
            GameManager.Ins.onGameWin += SetLevelDone;
        }

        public int GetRoadCount()
        {
            SetMinMaxRoadCount();
            if (PlayerPrefs.HasKey(LEVEL_KEY + m_Level))
            {
                m_CurrentLevelRoadCount = PlayerPrefs.GetInt(LEVEL_KEY + m_Level);
            }
            else
            {
                m_CurrentLevelRoadCount = Random.Range(m_MinMaxRoadCount.x, m_MinMaxRoadCount.y);
                PlayerPrefs.SetInt(LEVEL_KEY + m_Level, m_CurrentLevelRoadCount);
            }
            return m_CurrentLevelRoadCount;
        }

        public void SetLevelDone()
        {
            PlayerPrefs.SetInt(LEVEL_KEY, PlayerPrefs.GetInt(LEVEL_KEY) + 1);
        }

        private void SetMinMaxRoadCount()
        {
            m_MinMaxRoadCount.x = (m_Level + 1) * m_MinMaxIncreaseAmountByLevel + m_StartMinMaxRoadCount.x;
            m_MinMaxRoadCount.y = (m_Level + 1) * m_MinMaxIncreaseAmountByLevel + m_StartMinMaxRoadCount.y;
        }
    }
}
