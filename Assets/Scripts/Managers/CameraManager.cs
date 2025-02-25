using UnityEngine;
using Cinemachine;
using DG.Tweening;

namespace Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public Transform m_Target;
        public float m_Speed = 1f;

        private float m_InitialDistance;
        private float m_CurrentAngle;

        private CinemachineVirtualCamera m_Camera;
        private Vector3 m_StartPosition;
        private Vector3 m_StartRotation;

        private Sequence m_CameraSequence;

        private void Start()
        {
            m_Camera = GetComponent<CinemachineVirtualCamera>();
            m_StartPosition = transform.position;
            m_StartRotation = transform.localEulerAngles;

            GameManager.Ins.onGameStart += StopAnimation;
            GameManager.Ins.onGameWin += StartAnimation;
        }

        private void StartAnimation()
        {
            m_Camera.Follow = null;
            m_InitialDistance = Vector3.Distance(transform.position, m_Target.position);
            var directionToTarget = (transform.position - m_Target.position).normalized;
            m_CurrentAngle = Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;

            AnimateCamera();
        }

        private void AnimateCamera()
        {
            if (m_CameraSequence != null)
                m_CameraSequence.Kill();

            m_CameraSequence = DOTween.Sequence();

            m_CameraSequence.Append(DOTween.To(() => m_CurrentAngle, x => m_CurrentAngle = x, m_CurrentAngle + 360, 7f)
                .SetLoops(-1)
                .SetEase(Ease.Linear)
                .OnUpdate(UpdatePosition));

            m_CameraSequence.Play();
        }

        private void StopAnimation()
        {
            if (m_CameraSequence != null)
                m_CameraSequence.Kill();

            DoMoveAndRotate(m_StartPosition, m_StartRotation);
        }

        private void UpdatePosition()
        {
            var x = m_Target.position.x + m_InitialDistance * Mathf.Cos(m_CurrentAngle * Mathf.Deg2Rad);
            var z = m_Target.position.z + m_InitialDistance * Mathf.Sin(m_CurrentAngle * Mathf.Deg2Rad);
            var y = transform.position.y;

            transform.position = new Vector3(x, y, z);
            transform.LookAt(m_Target);
        }

        private void DoMoveAndRotate(Vector3 targetPosition, Vector3 targetRotation)
        {
            m_CameraSequence = DOTween.Sequence();

            m_CameraSequence.Append(transform.DOMove(targetPosition, 1.5f));
            m_CameraSequence.Join(transform.DORotate(targetRotation, 1.5f));
            m_CameraSequence.OnKill(() => { m_Camera.Follow = m_Target; });
        }
    }
}
