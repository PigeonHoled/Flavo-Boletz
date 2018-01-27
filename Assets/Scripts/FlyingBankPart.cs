using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBankPart : MonoBehaviour {

    Rigidbody rb;

    public void Awake() {
        rb = GetComponentInParent<Rigidbody>();
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == (int) EStatics.ELayer.Projectile) {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Destroy(other.gameObject);
        }
    }
}
