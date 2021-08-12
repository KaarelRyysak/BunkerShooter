using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField] private ENEMY_STATE state;
    #region unity methods

    void Awake()
    {
        navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("The navmesh agent isn't attached to " + this.gameObject.name);
        }
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
    private void MoveAgentForwards()
    {
        Vector3 targetPos = this.transform.position + (Vector3.left * 20f);
        navMeshAgent.SetDestination(targetPos);
    }

    #endregion

#region coroutines

    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator RUNNING()
    {
        //Entering the state

        while (state == ENEMY_STATE.RUNNING)
        {
            MoveAgentForwards();
            yield return new WaitForSeconds(0.5f);
        }

        //leaving the state
    }

    IEnumerator ESCAPING()
    {
        //Entering the state

        while (state == ENEMY_STATE.RUNNING)
        {
            //Find closest unoccupied cover
            CoveredSpot closestSpot = null;
            float closestSpotDistance = float.MaxValue;
            foreach (CoveredSpot spot in CoverManager.I.levelCover)
            {
                float distanceFromSpot = Vector3.Magnitude(spot.transform.position - this.transform.position);
                if (spot.occupiers.Count == 0 && distanceFromSpot < closestSpotDistance)
                {
                    closestSpot = spot;
                    closestSpotDistance = distanceFromSpot;
                }
            }

            SetNewAgentDestination(closestSpot.transform.position);
            yield return new WaitForSeconds(5f);
        }

        //leaving the state
    }

#endregion


}

#region enums
public enum ENEMY_STATE {RUNNING, ESCAPING, HIDING}
#endregion
