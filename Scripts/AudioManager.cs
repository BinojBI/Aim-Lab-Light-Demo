using System;
using System.Collections.Generic;
using UnityEngine;

namespace AimlabDemo
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            public float volume = 1f;

            [HideInInspector]
            public AudioSource source;
        }

        public List<Sound> sounds = new List<Sound>();

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

            foreach (var s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
            }
        }
        public void Play(string name)
        {
            Sound s = sounds.Find(x => x.name == name);
            if (s != null)
                s.source.Play();
            else
                Debug.LogWarning("Sound not found: " + name);
        }

        public void Stop(string name)
        {
            Sound s = sounds.Find(x => x.name == name);
            if (s != null)
                s.source.Stop();
        }
    }
}
