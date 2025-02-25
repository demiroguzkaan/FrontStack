using UnityEngine;
using Scripts.Enums;
using System.Collections.Generic;

namespace Scripts.Managers
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager m_Ins;
        public static AudioManager Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = FindObjectOfType<AudioManager>();
                return m_Ins;
            }
        }

        [SerializeField] private List<Sound> m_Sounds;
        [SerializeField] private AudioSource m_Source;
        [SerializeField] private float m_DefaultPitchValue;
        [SerializeField] private float m_PitchIncreaseValue;

        public void PlaySound(SoundType type, bool clearPitch = true)
        {
            var sound = m_Sounds.Find(x => x.soundType == type);
            if (sound != null)
            {
                if (clearPitch)
                    m_Source.pitch = m_DefaultPitchValue;
                m_Source.clip = sound.clip;
                m_Source.Play();
            }
        }

        public void PlaySound(SoundType type, int combo)
        {
            m_Source.pitch = Mathf.Clamp(combo * m_PitchIncreaseValue + m_DefaultPitchValue, m_DefaultPitchValue, 3);
            PlaySound(type, false);
        }

        [System.Serializable]
        private class Sound
        {
            public SoundType soundType;
            public AudioClip clip;
        }
    }

}
