using UnityEngine;

namespace Scripts.Road
{
    public class RoadBase : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;
        [SerializeField] private Transform m_LeftChild;
        [SerializeField] private Transform m_RightChild;

        [ContextMenu("Slice")]
        private void Slice()
        {
            if (transform.position.x != m_Target.position.x)
            {
                var isLeft = transform.position.x < m_Target.position.x;
                var sliceAmount = Mathf.Abs(transform.position.x - m_Target.position.x) / transform.localScale.x;
                var scale = Vector3.one;
                scale.x = sliceAmount;

                var primaryChild = isLeft ? m_LeftChild : m_RightChild;
                var secondaryChild = isLeft ? m_RightChild : m_LeftChild;

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
        }
    }
}