using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //The enemies will be spawned at random points between the start and end
    public Transform spawnerStartPoint;
    public Transform spawnerEndPoint;
    public GameObject enemyPrefab;
    public float timeBetweenSpawns = 2f;

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(0, 0, 1, 0.5F);
        Gizmos.DrawLine(spawnerStartPoint.position, spawnerEndPoint.position);
    }

    void Start()
    {
        StartSpawner();
    }

    public void StartSpawner()
    {
        StartCoroutine("SpawnEnemies");
    }

    public void StopSpawner()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnEnemies()
    {
        //Find the vector from start point to end point and multiply that by random variable
        //Add the result to the start point
        while (true)
        {  
            float randomVar = Random.Range(0f, 1f);
            Vector3 startToEnd = spawnerEndPoint.position - spawnerStartPoint.position;
            Vector3 enemyPosition = spawnerStartPoint.position + (startToEnd * randomVar);

            GameObject.Instantiate(enemyPrefab, enemyPosition, enemyPrefab.transform.rotation);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

    }
}
