using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoveredSpot : MonoBehaviour
{
    public List<EnemyAI> occupiers; // all enemies within the spot's area (ideally only one)

    private void Awake()
    {
        this.occupiers = new List<EnemyAI>();
        if (this.GetComponent<Collider>() == null)
        {
            Debug.LogError("Rigidbody trigger isn't attached to " + this.gameObject.name);
        }
    }   

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();
            if (enemyAI == null)
            {
                Debug.LogError("EnemyAI isn't attached to " + other.gameObject.name);
            }
            else {
                occupiers.Add(enemyAI);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();
            if (enemyAI == null)
            {
                Debug.LogError("EnemyAI isn't attached to " + other.gameObject.name);
            }
            else {
                occupiers.Remove(enemyAI);
            }
        }
    }
}
