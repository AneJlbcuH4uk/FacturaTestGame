using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyHolder;
    [SerializeField] private FlowFieldManager flowFieldManager;
    [SerializeField] private Transform enemyTarget;

    [SerializeField] private int amountOfEnemiesPerSpawn = 4;
    [SerializeField] private float spawnFrequency = 2f;
    [SerializeField] private Rect spawnWindow = new Rect(-30, 0, 60, 10);
    [SerializeField] private float spawnOffset = 60f;
    private float enemySpawnBorder;
    private Coroutine spawning;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 pos = new Vector3(spawnWindow.center.x, 0.5f, spawnWindow.center.y);
        Vector3 size = new Vector3(spawnWindow.size.x, 0, spawnWindow.size.y);
        Gizmos.DrawWireCube(pos, size);
    }

    public void StartSpawning(Vector3 border)
    {
        enemySpawnBorder = border.z;
        spawning = StartCoroutine(TriggerSpawn());
    }

    public void StopSpawning()
    {
        StopCoroutine(spawning);
    }

    private void Spawn(Vector3 position) 
    {
        var enemy = Instantiate(enemyPrefab, position, Quaternion.identity, enemyHolder);
        
        if(enemy.GetComponent<Enemy>() is Enemy enemyData ) 
            enemyData.Init(flowFieldManager, enemyTarget);

        if (spawnWindow.yMin >= enemySpawnBorder)
            StopCoroutine(spawning);
    }

    private IEnumerator TriggerSpawn() 
    {
        while (true)
        {
            FillSpace(spawnWindow);
            yield return new WaitForSecondsRealtime(spawnFrequency);
        }
    }


    private void FillSpace(Rect space) 
    {
        for(int i = 0 ; i < amountOfEnemiesPerSpawn; i++) 
        {
            var randomPosition = new Vector3(Random.Range(space.xMin, space.xMax), 0,
                                             Random.Range(space.yMin, space.yMax));

            Spawn(randomPosition);
        }
    }


    public void MoveSpawnWindow(float position)
    {
        spawnWindow.y = position + spawnOffset;
    }

}
