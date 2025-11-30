using AimlabDemo;
using NUnit.Framework;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public static TargetSpawner Instance;

    public Transform targetBase;
    public float targetSize = 0.2f;
    public Vector2 spawnRange = new Vector2(4f, 2.5f);
    public int activeTargets = 5;

    Camera cam;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    void Start()
    {
        for (int i = 0; i < activeTargets; i++)
            SpawnNext();
    }

    public void SpawnNext()
    {
        GameObject t = TargetPool.Instance.GetTarget();
        if (t == null) return;

        Vector3 spawnPos;

        // try up to 30 attempts to find a non-overlapping position
        int attempts = 0;
        do
        {
            attempts++;

            float offsetX = Random.Range(-spawnRange.x, spawnRange.x);
            float offsetY = Random.Range(-spawnRange.y, spawnRange.y);

            spawnPos = targetBase.position + new Vector3(offsetX, offsetY, 0f);

            // break if too many attempts (fallback)
            if (attempts > 30)
                break;

        } while (IsOverlapping(spawnPos));

        t.GetComponent<Target>().Activate(spawnPos, new Vector3(targetSize, targetSize, targetSize));
    }

    bool IsOverlapping(Vector3 newPos)
    {
        foreach (var obj in TargetPool.Instance.pool)
        {
            if (obj.activeInHierarchy)
            {
                float dist = Vector3.Distance(newPos, obj.transform.position);
                if (dist < spawnRange.x/5)
                    return true;
            }
        }
        return false;
    }

}
