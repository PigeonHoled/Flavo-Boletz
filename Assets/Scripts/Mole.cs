﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour {

    [SerializeField]
    float Speed = 1.0f;

    [SerializeField]
    Vector2 StraightMoveDuration = new Vector2(4.0f, 9.0f);

    [SerializeField]
    Transform MoleholeCenter;

    [SerializeField]
    Vector2 ChangeDirectionAngle = new Vector2(20.0f, 100.0f);

    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        transform.position = new Vector3(transform.position.x, MoleholeCenter.position.y, transform.position.z);
        StartCoroutine();
    }

    private void StartCoroutine() {
        StartCoroutine(StraightMoveCoroutine());
    }

    IEnumerator StraightMoveCoroutine() {
        Quaternion currentRotation = Quaternion.Euler(0.0f, Random.Range(ChangeDirectionAngle.x, ChangeDirectionAngle.y), 0.0f);
        Vector3 direction = currentRotation * (MoleholeCenter.transform.position - transform.position);
        Vector3 velocity = direction.normalized * Speed;
        transform.forward = direction;
        rb.velocity = velocity;
        yield return new WaitForSeconds(Random.Range(StraightMoveDuration.x, StraightMoveDuration.y));
        StartCoroutine();
    }
}
