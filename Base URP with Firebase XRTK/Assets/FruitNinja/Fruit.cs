using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject DelayAction;
    public void Update()
    {
        Invoke("SelfDestruct", 5.0f);
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
        Debug.Log("Fruit Self-Destruct!!");
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }
}
