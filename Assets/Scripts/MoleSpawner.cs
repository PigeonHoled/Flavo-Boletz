using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour {

    [SerializeField]
    Transform SpawnCenter;

    [SerializeField]
    GameObject MolePrefab;

    [SerializeField]
    float SpawnRadius;

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
        GameObject mole = Instantiate(MolePrefab, SpawnCenter.transform.position + Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f) * new Vector3(0.0f, 0.0f, SpawnRadius), Quaternion.identity);
        Mole moleScript = mole.GetComponent<Mole>();
        moleScript.MoleholeCenter = SpawnCenter;
    }

}
