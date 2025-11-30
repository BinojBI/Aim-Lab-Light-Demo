using System.Collections;
using UnityEngine;

namespace AimlabDemo { 
public class Target : MonoBehaviour
{
        public float waitingTime = 0.15f;


        public void Activate(Vector3 pos, Vector3 size)
        {
            transform.position = pos;
            transform.localScale = size;
            gameObject.SetActive(true);
        }

        public void Hit()
        {
            StartCoroutine(ShrinkRoutine());
            
            TargetSpawner.Instance.SpawnNext();
            UIManager.Instance.RegisterHit();
            AudioManager.Instance.Play("hit");
        }

        IEnumerator ShrinkRoutine()
        {
            yield return new WaitForSeconds(waitingTime);
            gameObject.SetActive(false);
        }
    }
}
