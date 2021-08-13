using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 20f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Vanish", lifetime);
    }

    private void Vanish()
    {
        Destroy(this.gameObject);
    }
}
