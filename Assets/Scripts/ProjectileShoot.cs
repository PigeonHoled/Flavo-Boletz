using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour {

    [SerializeField]
    GameObject Projectile;

    [SerializeField]
    float TrumpetOffset;

    [SerializeField]
    BoletzCamera Camera;

    [SerializeField]
    float Cooldown = 3.0f;

    [SerializeField]
    float ProjectileSpeed = 10.0f;

    /// <summary>
    /// Used to calculate how to rotate direction of shot projectile towards camera's crosshair's direction
    /// </summary>
    [SerializeField]
    float CommonDistance = 50.0f;

    [SerializeField]
    float RaiseVelocityUpwardsAngle = 20.0f;

    [SerializeField]
    int NumOfPreviewSteps = 100;

    [SerializeField]
    float PreviewDeltaTimeStep = 0.1f;

    private LineRenderer LineRenderer;
    private Quaternion TowardsCrosshairRotation;
    private Quaternion RaiseVelocityUpwardsRotation;
    bool bIsCoroutinePlaying;

    private Vector3 InitialPosition;
    private Vector3 InitialVelocity;

    private void Awake() {
        LineRenderer = GetComponent<LineRenderer>();
    }

    private void Start() {
        TowardsCrosshairRotation = Quaternion.Euler(0.0f, Mathf.Atan(Camera.CameraOffset.x / CommonDistance) * Mathf.Rad2Deg, 0.0f);
        RaiseVelocityUpwardsRotation = Quaternion.Euler(-RaiseVelocityUpwardsAngle, 0.0f, 0.0f);
    }


void Update () {
        if (bIsCoroutinePlaying) { return; }

        InitialPosition = Camera.transform.position + TowardsCrosshairRotation * Camera.transform.rotation * new Vector3(-Camera.CameraOffset.x, -Camera.CameraOffset.y, -Camera.CameraOffset.z + TrumpetOffset);
        InitialVelocity = TowardsCrosshairRotation * Camera.transform.rotation * RaiseVelocityUpwardsRotation * new Vector3(0.0f, 0.0f, ProjectileSpeed);

        PreviewProjectilePath();
        TryShoot();
	}

    void PreviewProjectilePath () {
        LineRenderer.positionCount = NumOfPreviewSteps;
        Vector3 position = InitialPosition;
        Vector3 velocity = InitialVelocity;
        Vector3 lastPosition;
        for (int i = 0; i < NumOfPreviewSteps; ++i) {
            LineRenderer.SetPosition(i, position);
            lastPosition = position;
            position += velocity * PreviewDeltaTimeStep + 0.5f * Physics.gravity * PreviewDeltaTimeStep * PreviewDeltaTimeStep;
            velocity += Physics.gravity * PreviewDeltaTimeStep;
            if (Physics.Linecast(lastPosition, position)) {
                LineRenderer.positionCount = i;
                return;
            }
        }
    }

    void TryShoot() {
        if (Input.GetButton("Fire1")) {
            GameObject sentProjectile = Instantiate(Projectile, InitialPosition, Camera.transform.rotation);
            Rigidbody rb = sentProjectile.GetComponent<Rigidbody>();
            rb.velocity = InitialVelocity;
            StartCoroutine(SetCooldownCoroutine());
        }
    }

    IEnumerator SetCooldownCoroutine() {
        bIsCoroutinePlaying = true;
        LineRenderer.positionCount = 0;
        yield return new WaitForSeconds(Cooldown);
        bIsCoroutinePlaying = false;
    }
}
