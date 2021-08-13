using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 20f;
    [SerializeField] private float suppressionRadius = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Vanish", lifetime);
    }

    private void Vanish()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Suppress surrounding enemies
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, suppressionRadius);
        foreach (var nearbyCollider in nearbyColliders)
        {
            if (nearbyCollider.tag == "Enemy")
            {
                EnemyAI enemyAI = nearbyCollider.gameObject.GetComponent<EnemyAI>();
                if (enemyAI == null)
                {
                    Debug.LogError("EnemyAI isn't attached to the enemy " + nearbyCollider.gameObject.name);
                }
                else
                {
                    enemyAI.SuppressEnemy();
                }
            }
        }

        GameObject.Destroy(this.gameObject);
        //Debug.Log("bullet colliding with " + collision.gameObject.name);
    }
}
