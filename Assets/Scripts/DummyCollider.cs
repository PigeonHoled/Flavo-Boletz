using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCollider : MonoBehaviour {

    Rigidbody rb;
    FlyingBankPart bank;


    private void Start() {
        rb = GetComponent<Rigidbody>();
        bank = GetComponentInChildren<FlyingBankPart>();
    }

    private void OnCollisionEnter(Collision collision) {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        bank.Hit();
    }
}
