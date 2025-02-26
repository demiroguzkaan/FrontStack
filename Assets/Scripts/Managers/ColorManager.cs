using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Managers
{
    public class ColorManager : MonoBehaviour
    {
        private static ColorManager m_Ins;
        public static ColorManager Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = FindObjectOfType<ColorManager>();
                return m_Ins;
            }
        }

        [SerializeField] private Renderer m_StartPlatformRenderer;
        [SerializeField] private List<Material> m_Colors;

        private int m_CurrentColorIndex;

        private void Start()
        {
            Initialize();
        }

        public Material GetMaterial()
        {
            var material = m_Colors[m_CurrentColorIndex];
            m_CurrentColorIndex++;
            if (m_CurrentColorIndex >= m_Colors.Count) m_CurrentColorIndex = 0;
            return material;
        }

        private void Initialize()
        {
            m_CurrentColorIndex = Random.Range(0, m_Colors.Count - 1);
            m_StartPlatformRenderer.material = GetMaterial();
        }
    }
}
