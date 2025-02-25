using UnityEngine;
using Scripts.Road;
using UnityEngine.Events;
using Scripts.Player;

namespace Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Ins;
        public static GameManager Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = FindObjectOfType<GameManager>();
                return m_Ins;
            }
        }

        public UnityAction onGameStart;
        public UnityAction onRoadTriggered;
        public UnityAction onGameWin;
        public UnityAction onGameLose;

        public float perfectTreshold;
        public float dieTreshold;

        public Transform lastRoad;
        public int comboCount;

        [SerializeField] private Transform m_EndPlatformPrefab;
        [SerializeField] private RoadBase m_RoadPrefab;
        [SerializeField] private float m_StartX;
        [SerializeField] private float m_ZDistance;
        [SerializeField] private PlayerBase m_Player;

        private Transform m_EndPlatform;
        private int m_StartDirection = 1;
        private int m_TargetRoadCount;
        private int m_CurrentRoadCount;

        private void Start()
        {
            onGameStart += () =>
            {
                m_CurrentRoadCount = 0;
                m_TargetRoadCount = LevelManager.Ins.GetRoadCount();
                SpawnEndPlatform();
                SpawnNextRoad();
            };

            onRoadTriggered += () =>
            {
                SpawnNextRoad();
            };

            onGameWin += () =>
            {
                lastRoad = m_EndPlatform;
                m_Player.MovePlayer(m_EndPlatform, Enums.MoveType.Finish);
                // move player
                // center player
                // win anim
                // camera rotate anim
                // win ui
            };
        }

        [ContextMenu("Start")]
        public void OnGameStart()
        {
            onGameStart?.Invoke();
        }

        public void OnRoadTriggered()
        {
            onRoadTriggered?.Invoke();
        }

        public void OnGameWin()
        {
            onGameWin?.Invoke();
        }

        public void OnGameLose()
        {
            onGameLose?.Invoke();
        }

        public void SpawnNextRoad()
        {
            m_CurrentRoadCount++;
            if (m_CurrentRoadCount >= m_TargetRoadCount)
            {
                OnGameWin();
            }
            else
            {
                var pos = Vector3.zero;
                pos.x = m_StartX * m_StartDirection;
                pos.z = lastRoad.position.z + m_ZDistance;

                var newRoad = Instantiate(m_RoadPrefab, pos, Quaternion.identity);

                m_StartDirection *= -1;
                dieTreshold = lastRoad.localScale.x;
            }
        }

        private void SpawnEndPlatform()
        {
            var pos = lastRoad.position;
            pos.z += m_ZDistance * m_TargetRoadCount;

            m_EndPlatform = Instantiate(m_EndPlatformPrefab, pos, Quaternion.identity);
        }
    }
}
