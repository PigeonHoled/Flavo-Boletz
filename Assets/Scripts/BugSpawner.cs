using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSpawner : MonoBehaviour {

    [SerializeField]
    Transform SpawnBorder1;

    [SerializeField]
    Transform SpawnBorder2;

    [SerializeField]
    Transform SpawnCenter;

    [SerializeField]
    GameObject BugPrefab;


    [SerializeField]
    float SpawnDelay;


    private void Start() {
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
        GameObject mole = Instantiate(BugPrefab, SpawnBorder1.transform.position + Random.Range(0.0f, 1.0f) * (SpawnBorder2.transform.position - SpawnBorder1.transform.position), Quaternion.identity);
        Mole moleScript = mole.GetComponent<Mole>();
        moleScript.MoleholeCenter = SpawnCenter;
    }

}
