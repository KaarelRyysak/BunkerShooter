using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishAfterDelay : MonoBehaviour
{
    [SerializeField] private float delay = 20f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Vanish", delay);
    }

    private void Vanish()
    {
        Destroy(this.gameObject);
    }
}
