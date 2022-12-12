using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> ObjectsToSpawn = new List<GameObject>();
    public GameObject SpawnEffect;
    public AudioSource soundEffect;
    public bool IsTimer;
    public float TimeToSpawn;
    private float currentTimeToSpawn;
    public bool isRandomized;

    void Start()
    {
        currentTimeToSpawn = TimeToSpawn; 
    }
    void Update()
    {
        if(IsTimer)
        {
            UpdateTimer();
        }
    }
    private void UpdateTimer()
    {
        if (currentTimeToSpawn > 0)
        {
            currentTimeToSpawn -= TimeToSpawn;
        }
        else
        {
            SpawnObject();
            currentTimeToSpawn= TimeToSpawn;
        }
    }
    public void SpawnObject()
    {
        int index = isRandomized ? Random.Range(0, ObjectsToSpawn.Count) : 0;
        if (ObjectsToSpawn.Count > 0)
        {
            Instantiate(ObjectsToSpawn[index], transform.position, transform.rotation);
        }
    }
}
