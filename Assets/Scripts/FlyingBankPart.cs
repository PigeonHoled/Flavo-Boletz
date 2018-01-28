using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBankPart : MonoBehaviour {

    Rigidbody rb;

    public float TimeConstant = 0.2f;
    public float Amplitude = 3.0f;

    public void Awake() {
        rb = GetComponentInParent<Rigidbody>();
    }

    Vector3 initPosition;
    private bool bWasHit = false;

    private void Start() {
        initPosition = transform.parent.position;
        Debug.Log(transform.parent.position);
    }

    private void Update() {
        if (bWasHit) { return; }

        transform.parent.position = new Vector3(initPosition.x, initPosition.y + Mathf.Sin(Time.time * TimeConstant) * Amplitude, initPosition.z);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == (int) EStatics.ELayer.Projectile) {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Destroy(other.gameObject);
        }
        bWasHit = true;
    }

    public void OnCollisionEnter(Collision collision) {
        bWasHit = true;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
