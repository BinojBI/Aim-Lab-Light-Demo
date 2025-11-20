using UnityEngine;

namespace AimlabDemo { 
public class Target : MonoBehaviour
{
        public void Activate(Vector3 pos)
        {
            transform.position = pos;
            gameObject.SetActive(true);
        }

        public void Hit()
        {
            gameObject.SetActive(false);
            TargetSpawner.Instance.SpawnNext();
            UIManager.Instance.RegisterHit();
            AudioManager.Instance.Play("hit");
        }
    }
}
