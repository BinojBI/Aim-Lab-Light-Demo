using System.Collections.Generic;
using UnityEngine;

namespace AimlabDemo
{
    public class TargetPool : MonoBehaviour
    {
        public static TargetPool Instance;

        public GameObject targetPrefab;
        public int poolSize = 6;

        public List<GameObject> pool => _pool;
        private List<GameObject> _pool = new List<GameObject>();

        void Awake()
        {
            Instance = this;

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(targetPrefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        public GameObject GetTarget()
        {
            foreach (var obj in pool)
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }

            return null; 
        }
    }
}
