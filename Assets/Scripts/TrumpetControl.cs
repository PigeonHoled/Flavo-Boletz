using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetControl : MonoBehaviour
{

    [SerializeField]
    GameObject Projectile;

    [SerializeField]
    float TrumpetOffset;

    [SerializeField]
    BoletzCamera Camera;

    [SerializeField]
    float Cooldown = 3.0f;

    [SerializeField]
    Vector2 ProjectileSpeed = new Vector2(10.0f, 40.0f);

    [SerializeField]
    float RaiseVelocityTempoPerSecond = 8.0f;

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
    private float CurrentProjectileSpeed;

    private bool bWasFireHandled;
    private bool bIsCooldownCoroutinePlaying;
    private bool bShouldShoot;

    private float StartHoldingFireKey;

    private Vector3 InitialPosition;
    private Vector3 InitialVelocity;
    private PlayerNumber Number;

    private void Awake() {
        LineRenderer = GetComponent<LineRenderer>();
        Number = GetComponent<PlayerNumber>();
    }

    private void Start() {
        TowardsCrosshairRotation = Quaternion.Euler(0.0f, Mathf.Atan(Camera.CameraOffset.x / CommonDistance) * Mathf.Rad2Deg, 0.0f);
        RaiseVelocityUpwardsRotation = Quaternion.Euler(-RaiseVelocityUpwardsAngle, 0.0f, 0.0f);
        bWasFireHandled = true;
    }


    void Update() {
        if (bIsCooldownCoroutinePlaying) { return; }

        bShouldShoot = false;

        if (Input.GetButton("Fire_" + Number.Number)) {
            if (bWasFireHandled) {
                StartHoldingFireKey = Time.time;
                bWasFireHandled = false;
                CurrentProjectileSpeed = ProjectileSpeed.x;
            } else {
                CurrentProjectileSpeed = Mathf.Clamp(ProjectileSpeed.x + (Time.time - StartHoldingFireKey) * RaiseVelocityTempoPerSecond, ProjectileSpeed.x, ProjectileSpeed.y);
            }
        } else if (!bWasFireHandled) {
            bShouldShoot = true;
            CurrentProjectileSpeed = Mathf.Clamp(ProjectileSpeed.x + (Time.time - StartHoldingFireKey) * RaiseVelocityTempoPerSecond, ProjectileSpeed.x, ProjectileSpeed.y);
            bWasFireHandled = true;
        } else {
            CurrentProjectileSpeed = ProjectileSpeed.x;
        }

        InitialPosition = Camera.transform.position + TowardsCrosshairRotation * Camera.transform.rotation * new Vector3(-Camera.CameraOffset.x, -Camera.CameraOffset.y, -Camera.CameraOffset.z + TrumpetOffset);
        InitialVelocity = TowardsCrosshairRotation * Camera.transform.rotation * RaiseVelocityUpwardsRotation * new Vector3(0.0f, 0.0f, CurrentProjectileSpeed);

        if (bShouldShoot) {
            Shoot();
        } else {
            PreviewProjectilePath();
        }
    }

    void PreviewProjectilePath() {
        LineRenderer.positionCount = NumOfPreviewSteps;
        Vector3 position = InitialPosition;
        Vector3 velocity = InitialVelocity;
        Vector3 lastPosition;
        bool bShouldEndInNextIteration = false;
        for (int i = 0; i < NumOfPreviewSteps; ++i) {
            LineRenderer.SetPosition(i, position);
            if (bShouldEndInNextIteration) {
                LineRenderer.positionCount = i;
                return;
            }
            lastPosition = position;
            position += velocity * PreviewDeltaTimeStep + 0.5f * Physics.gravity * PreviewDeltaTimeStep * PreviewDeltaTimeStep;
            velocity += Physics.gravity * PreviewDeltaTimeStep;
            if (Physics.Linecast(lastPosition, position)) {
                bShouldEndInNextIteration = true;
            }
        }
    }

    void Shoot() {
        GameObject sentProjectile = Instantiate(Projectile, InitialPosition, Camera.transform.rotation);
        Rigidbody rb = sentProjectile.GetComponent<Rigidbody>();
        rb.velocity = InitialVelocity;
        StartCoroutine(SetCooldownCoroutine());
    }

    IEnumerator SetCooldownCoroutine() {
        bIsCooldownCoroutinePlaying = true;
        LineRenderer.positionCount = 0;
        yield return new WaitForSeconds(Cooldown);
        bIsCooldownCoroutinePlaying = false;
    }
}
