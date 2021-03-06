using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private ENEMY_STATE state;
    [SerializeField] private int health = 8;

    [SerializeField] private float maxDistanceFromCover = 10f;
    [SerializeField] private float hidingSpotSize = 2f;
    private Material surfaceMaterial;
    private Color defaultEnemyColor;
    [SerializeField] private Color onHitEnemyColor;
    [SerializeField] private float fadeDuration = 0.5f;
    
    #region unity methods
    void Awake()
    {
        navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("The navmesh agent isn't attached to " + this.gameObject.name);
        }
        surfaceMaterial = this.gameObject.GetComponent<Renderer>().material;
        defaultEnemyColor = surfaceMaterial.color;
    }

    void Start()
    {
        state = ENEMY_STATE.RUNNING;
        StartCoroutine("EnemyFSM");
    }

    private void SetNewAgentDestination(Vector3 targetPos)
    {
        if (targetPos != null)
        {
            navMeshAgent.SetDestination(targetPos);
        }
    }
    private void MoveAgentDestinationForwards()
    {
        Vector3 targetPos = this.transform.position + (Vector3.left * 20f);
        navMeshAgent.SetDestination(targetPos);
    }

    private CoveredSpot FindClosestUnoccupiedCover()
    {
        CoveredSpot closestSpot = null;
        float closestSpotDistance = float.MaxValue;
        foreach (CoveredSpot spot in GameController.I.coverManager.levelCover)
        {
            float distanceFromSpot = Vector3.Magnitude(spot.transform.position - this.transform.position);
            if (spot.occupiers.Count == 0 && distanceFromSpot < closestSpotDistance)
            {
                closestSpot = spot;
                closestSpotDistance = distanceFromSpot;
            }
        }
        return closestSpot;
    }

    public void SuppressEnemy()
    {
        state = ENEMY_STATE.ESCAPING;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("taking damage");
            state = ENEMY_STATE.ESCAPING;
            health--;
            if (health <= 0)
            {
                GameObject.Destroy(this.gameObject);
                GameController.I.IncreaseScore(1);
            }

            // trigger visual damage effect
            StopCoroutine("FadeBetweenColors");
            StartCoroutine("FadeBetweenColors");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyGoal")
        {
            GameController.I.DamagePlayer(1);
        }
    }
    #endregion

#region coroutines
    IEnumerator FadeBetweenColors()
    {
        float currentTime = 0.0f;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            surfaceMaterial.color = Color.Lerp(onHitEnemyColor, defaultEnemyColor, (currentTime / fadeDuration));
            yield return null;
        }
    }

    // we're using a finite state machine to control this AI
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator RUNNING()
    {
        // entering the state

        while (state == ENEMY_STATE.RUNNING)
        {
            MoveAgentDestinationForwards();

            yield return new WaitForSeconds(0.5f);
        }

        // leaving the state
    }

    IEnumerator ESCAPING()
    {
        // entering the state

        while (state == ENEMY_STATE.ESCAPING)
        {
            CoveredSpot closestSpot = FindClosestUnoccupiedCover();
            float distanceFromClosestSpot = Vector3.Magnitude(transform.position - closestSpot.transform.position);
            
            // if the hiding spot is too far away, the enemy will give up on escaping
            if (distanceFromClosestSpot > maxDistanceFromCover)
            {
                state = ENEMY_STATE.RUNNING;
                yield break;
            }

            SetNewAgentDestination(closestSpot.transform.position);

            yield return new WaitForSeconds(0.5f);

            // when the enemy has moved sufficiently close to a hiding spot, they stop and hide
            distanceFromClosestSpot = Vector3.Magnitude(transform.position - closestSpot.transform.position);
            if (distanceFromClosestSpot < hidingSpotSize)
            {
                state = ENEMY_STATE.HIDING;
            }
        }

        // leaving the state
    }

    IEnumerator HIDING()
    {
        // entering the state

        yield return new WaitForSeconds(2f);
        state = ENEMY_STATE.RUNNING;

        // leaving the state
    }
#endregion


}

#region enums
public enum ENEMY_STATE {RUNNING, ESCAPING, HIDING}
#endregion
