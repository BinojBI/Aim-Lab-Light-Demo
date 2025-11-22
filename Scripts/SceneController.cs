
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AimlabDemo
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance;

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

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
