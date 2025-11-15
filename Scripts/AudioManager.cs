using UnityEngine;

namespace AimlabDemo
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        private AudioSource m_AudioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        private void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }
        public void PlayHitSound()
        {
            m_AudioSource.Play();
        }
    }
}
