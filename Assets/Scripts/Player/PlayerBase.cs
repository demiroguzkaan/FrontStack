using DG.Tweening;
using UnityEngine;
using Scripts.Enums;
using Scripts.Managers;

namespace Scripts.Player
{
    public class PlayerBase : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        [SerializeField] private float m_SideWalkDuration;
        [SerializeField] private float m_FrontWalkDuration;
        [SerializeField] private float m_DieDuration;

        private Sequence m_MovementSequence;
        private bool m_IsAnimated;

        private const string RUN_KEY = "Run";
        private const string IDLE_KEY = "Idle";
        private const string DANCE_KEY = "Dance";

        private void Start()
        {
            GameManager.Ins.onGameStart += () => { m_Animator.SetTrigger(IDLE_KEY); m_IsAnimated = false; };
            GameManager.Ins.onGameLose += () => MovePlayer(null, MoveType.Die);
            GameManager.Ins.onRoadTriggered += () => MovePlayer(GameManager.Ins.lastRoad, MoveType.Road);
        }

        public void MovePlayer(Transform target, MoveType type)
        {
            m_MovementSequence?.Kill();
            m_MovementSequence = DOTween.Sequence();

            switch (type)
            {
                case MoveType.Road:
                    MoveToTarget(target, true, m_SideWalkDuration, RUN_KEY, m_FrontWalkDuration, IDLE_KEY);
                    break;
                case MoveType.Finish:
                    MoveToTarget(target, false, m_FrontWalkDuration, RUN_KEY, m_SideWalkDuration, DANCE_KEY);
                    break;
                case MoveType.Die:
                    HandleDeath();
                    break;
            }
        }

        private void MoveToTarget(Transform target, bool isTargetRoad, float firstDuration, string firstAnim, float secondDuration, string secondAnim)
        {
            if (isTargetRoad)
            {
                m_MovementSequence.Append(transform.DOMoveX(target.position.x, firstDuration).OnComplete(() => m_Animator.SetTrigger(firstAnim)))
                    .Append(transform.DOMoveZ(target.position.z, secondDuration).OnComplete(() => m_Animator.SetTrigger(secondAnim)));
            }
            else
            {
                m_Animator.SetTrigger(firstAnim);
                m_MovementSequence.Append(transform.DOMoveZ(target.position.z, firstDuration).OnComplete(() => m_Animator.SetTrigger(secondAnim)))
                    .Append(transform.DOMoveX(target.position.x, secondDuration).OnComplete(() => GameManager.Ins.OnGameWin()));
            }

            m_MovementSequence.OnKill(() =>
            {
                if (GameManager.Ins.isLevelFinished && !m_IsAnimated)
                {
                    MovePlayer(GameManager.Ins.lastRoad, MoveType.Finish);
                    m_IsAnimated = true;
                }
            });
        }

        private void HandleDeath()
        {
            var jumpPosition = transform.position;
            jumpPosition.y -= 10;
            jumpPosition.z += 5;

            m_Animator.SetTrigger(RUN_KEY);

            m_MovementSequence.Append(transform.DOJump(jumpPosition, 6, 1, m_DieDuration))
                .Join(transform.DOScale(Vector3.zero, m_DieDuration))
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}
