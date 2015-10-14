using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Populate : MonoBehaviour
{
    public static List<GameObject> populate = new List<GameObject>();
    static GameObject helicopter;
    public GameObject copter;

    void Start()
    {
        helicopter = copter;
    }

    [MenuItem("populate/Spawn")]
    public static void Spawn()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject spawnCopters;
            spawnCopters = Instantiate(helicopter) as GameObject;
            float posx = Random.Range(-20, 20);
            float posy = Random.Range(1, 20);
            float posz = Random.Range(-20, 20);
            spawnCopters.transform.position = new Vector3(posx, posy, posz);
            populate.Add(spawnCopters);
        }
    }

    [MenuItem("populate/deSpawn")]
    public static void deSpawn()
    {
        foreach (GameObject go in populate)
        {
            DestroyImmediate(go);
        }
        populate.Clear();
    }

    
}
