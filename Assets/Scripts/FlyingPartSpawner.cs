using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPartSpawner : MonoBehaviour {

    [SerializeField]
    GameObject PartPrefab;

    [SerializeField]
    List<Transform> Spawns;

    [SerializeField]
    List<GameObject> Parts;

    [SerializeField]
    Vector3 SpawnRotation;

    private int lastID = 0;

    [SerializeField]
    float SpawnDelay;

    private void Start() {
        Parts = new List<GameObject>();
        for (int i = 0; i < 4; i++)
            Parts.Add(null);
        Spawn();
        StartSpawnCoroutine();
    }

    private void StartSpawnCoroutine() {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine() {
        yield return new WaitForSeconds(SpawnDelay);
        Spawn();
        StartSpawnCoroutine();
    }

    private void Spawn() {
        int id = (lastID + 1) % 4;
        lastID = id;
        Vector3 position = Spawns[id].position;
        if (Parts[id] != null)
            Destroy(Parts[id]);

        GameObject spawnedPart = Instantiate(PartPrefab, position, Quaternion.Euler(SpawnRotation));
        Parts[id] = spawnedPart;
    }

}
