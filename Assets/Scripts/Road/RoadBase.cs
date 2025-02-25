using UnityEngine;
using DG.Tweening;
using Scripts.Managers;

namespace Scripts.Road
{
    public class RoadBase : MonoBehaviour
    {
        [SerializeField] private Transform m_LeftChild;
        [SerializeField] private Transform m_RightChild;
        [SerializeField] private float m_MoveDuration;
        [SerializeField] private float m_SliceUpForce;
        [SerializeField] private float m_SliceSideForce;
        [SerializeField] private float m_SliceRotationForce;

        private GameManager m_GameManager;
        private Sequence m_MovementSequence;
        private float m_StartX;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_GameManager = GameManager.Ins;
            SetScale();
            m_StartX = transform.position.x;
            StartMoving();
        }

        private void SetScale()
        {
            transform.localScale = new Vector3(m_GameManager.lastRoad.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Slice();
            }
        }

        private void StartMoving()
        {
            float targetX = -m_StartX;
            m_MovementSequence = DOTween.Sequence();
            m_MovementSequence.Append(transform.DOMoveX(targetX, m_MoveDuration).SetEase(Ease.Linear));
            m_MovementSequence.Append(transform.DOMoveX(m_StartX, m_MoveDuration).SetEase(Ease.Linear));
            m_MovementSequence.SetLoops(-1);
        }

        private void Slice()
        {
            var posDistance = Mathf.Abs(transform.position.x - m_GameManager.lastRoad.position.x);

            if (posDistance > m_GameManager.dieTreshold)
            {
                Debug.Log("Died");
            }
            else if (posDistance < m_GameManager.perfectTreshold)
            {
                TriggerPerfectActions();
            }
            else
            {
                var isLeft = transform.position.x < m_GameManager.lastRoad.position.x;
                var primaryChild = isLeft ? m_LeftChild : m_RightChild;
                var secondaryChild = isLeft ? m_RightChild : m_LeftChild;

                primaryChild.gameObject.SetActive(true);
                secondaryChild.gameObject.SetActive(true);

                TriggerSliceActions(primaryChild, secondaryChild, isLeft);
                TriggerAfterSliceActions(primaryChild, secondaryChild, isLeft);
            }
        }

        private void TriggerPerfectActions()
        {

            var pos = transform.position;
            pos.x = m_GameManager.lastRoad.position.x;

            transform.position = pos;
            AudioManager.Ins.PlaySound(Enums.SoundType.Perfect, m_GameManager.comboCount);
            m_GameManager.comboCount++;
            // Particle
            m_GameManager.lastRoad = transform;
            m_GameManager.OnRoadTriggered();
            m_MovementSequence.Kill();
            Destroy(this);
        }

        private void TriggerSliceActions(Transform primaryChild, Transform secondaryChild, bool isLeft)
        {
            var sliceAmount = Mathf.Abs(transform.position.x - m_GameManager.lastRoad.position.x) / transform.localScale.x;
            var scale = Vector3.one;
            scale.x = sliceAmount;

            primaryChild.localScale = scale;
            scale.x = 1 - sliceAmount;
            secondaryChild.localScale = scale;

            var primaryPos = Vector3.zero;
            primaryPos.x = secondaryChild.localScale.x / 2 * (isLeft ? -1 : 1);
            primaryChild.localPosition = primaryPos;

            var secondaryPos = Vector3.zero;
            secondaryPos.x = primaryChild.localScale.x / 2 * (isLeft ? 1 : -1);
            secondaryChild.localPosition = secondaryPos;
        }

        private void TriggerAfterSliceActions(Transform primaryChild, Transform secondaryChild, bool isLeft)
        {
            primaryChild.SetParent(null);
            Destroy(primaryChild.gameObject, 2f);

            var rb = primaryChild.gameObject.AddComponent<Rigidbody>();

            var force = new Vector3(isLeft ? -m_SliceSideForce : m_SliceSideForce, m_SliceUpForce, 0);
            rb.AddForce(force, ForceMode.VelocityChange);

            var rotationForce = new Vector3(0, 0, isLeft ? m_SliceRotationForce : -m_SliceRotationForce);
            rb.angularVelocity = rotationForce;

            secondaryChild.SetParent(null);
            m_GameManager.lastRoad = secondaryChild;

            m_GameManager.comboCount = 0;
            m_GameManager.OnRoadTriggered();
            m_MovementSequence.Kill();
            Destroy(gameObject);
        }
    }
}