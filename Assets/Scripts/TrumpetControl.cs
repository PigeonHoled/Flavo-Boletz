using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetControl : MonoBehaviour {

    [SerializeField]
    GameObject MoleProjectile;

    [SerializeField]
    GameObject CoakroachProjectile;

    [SerializeField]
    float TrumpetOffset;

    [SerializeField]
    BoletzCamera BoletzCamera;

    [SerializeField]
    Camera Camera;

    [SerializeField]
    MeshRenderer PlaneMolehole;

    [SerializeField]
    float CatchRadius = 5.0f;

    [SerializeField]
    float ShootCooldown = 3.0f;

    [SerializeField]
    float ShootDelay = 0.5f;

    [SerializeField]
    Vector2 ProjectileSpeed = new Vector2(10.0f, 40.0f);

    [SerializeField]
    float RaiseVelocityTempoPerSecond = 8.0f;

    [SerializeField]
    Vector2 CrosshairRelativePosition = new Vector2(0.5f, 0.5f);

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

    private Vector2 CrosshairPosition;

    private EStatics.EProjectile CurrentProjectileType = EStatics.EProjectile.None;
    private GameObject HoldProjectile;

    private void Awake() {
        LineRenderer = GetComponent<LineRenderer>();
    }

    private void Start() {
        TowardsCrosshairRotation = Quaternion.Euler(0.0f, Mathf.Atan(BoletzCamera.CameraOffset.x / CommonDistance) * Mathf.Rad2Deg, 0.0f);
        RaiseVelocityUpwardsRotation = Quaternion.Euler(-RaiseVelocityUpwardsAngle, 0.0f, 0.0f);
        bWasFireHandled = true;
        CatchRadius += Mathf.Abs(BoletzCamera.CameraOffset.z);
    }


void Update () {
        if (CurrentProjectileType == EStatics.EProjectile.None) {
            CatchMode();
        }
        else {
            ShootMode();
        }
	}

    void CatchMode() {
        if (!Input.GetButton("Fire1")) { return; }

        CrosshairPosition = new Vector2(CrosshairRelativePosition.x * Camera.pixelWidth, CrosshairRelativePosition.y * Camera.pixelHeight);
        Ray ray = Camera.ScreenPointToRay(CrosshairPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, CatchRadius, 1 << LayerMask.NameToLayer("Projectile"))) {
            CurrentProjectileType = EStatics.StringToEProjectile.Get[hit.collider.gameObject.tag];

            Vector3 InitialPosition = BoletzCamera.transform.position + TowardsCrosshairRotation * BoletzCamera.transform.rotation * new Vector3(-BoletzCamera.CameraOffset.x, -BoletzCamera.CameraOffset.y, -BoletzCamera.CameraOffset.z + TrumpetOffset);
            if (CurrentProjectileType == EStatics.EProjectile.Mole) {
                HoldProjectile = Instantiate(MoleProjectile, InitialPosition, BoletzCamera.transform.rotation);
            }
            else if (CurrentProjectileType == EStatics.EProjectile.Coakroach) {
                HoldProjectile = Instantiate(CoakroachProjectile, InitialPosition, BoletzCamera.transform.rotation);
            }

            Destroy(hit.collider.gameObject);
            StartCoroutine(SetCooldownCoroutine(ShootDelay));
        }
    }

    void ShootMode() {
        InitialPosition = BoletzCamera.transform.position + TowardsCrosshairRotation * BoletzCamera.transform.rotation * new Vector3(-BoletzCamera.CameraOffset.x, -BoletzCamera.CameraOffset.y, -BoletzCamera.CameraOffset.z + TrumpetOffset);
        InitialVelocity = TowardsCrosshairRotation * BoletzCamera.transform.rotation * RaiseVelocityUpwardsRotation * new Vector3(0.0f, 0.0f, CurrentProjectileSpeed);

        HoldProjectile.transform.position = InitialPosition;
        HoldProjectile.transform.rotation = BoletzCamera.transform.rotation;

        if (bIsCooldownCoroutinePlaying) { return; }

        bShouldShoot = false;

        if (Input.GetButton("Fire1")) {
            if (bWasFireHandled) {
                StartHoldingFireKey = Time.time;
                bWasFireHandled = false;
                CurrentProjectileSpeed = ProjectileSpeed.x;
            }
            else {
                CurrentProjectileSpeed = Mathf.Clamp(ProjectileSpeed.x + (Time.time - StartHoldingFireKey) * RaiseVelocityTempoPerSecond, ProjectileSpeed.x, ProjectileSpeed.y);
            }
        }
        else if (!bWasFireHandled) {
            bShouldShoot = true;
            CurrentProjectileSpeed = Mathf.Clamp(ProjectileSpeed.x + (Time.time - StartHoldingFireKey) * RaiseVelocityTempoPerSecond, ProjectileSpeed.x, ProjectileSpeed.y);
            bWasFireHandled = true;
        }
        else {
            CurrentProjectileSpeed = ProjectileSpeed.x;
        }

        if (bShouldShoot) {
            Shoot();
        }
        else {
            PreviewProjectilePath();
        }
    }

    void PreviewProjectilePath () {
        LineRenderer.positionCount = NumOfPreviewSteps;
        Vector3 position = InitialPosition;
        Vector3 velocity = InitialVelocity;
        Vector3 lastPosition;
        int ShouldEndInNextNextIteration = 0;
        for (int i = 0; i < NumOfPreviewSteps; ++i) {
            LineRenderer.SetPosition(i, position);
            if (ShouldEndInNextNextIteration == 2) {
                LineRenderer.positionCount = i;
                return;
            }
            else if (ShouldEndInNextNextIteration == 1) {
                ShouldEndInNextNextIteration++;
            }
            lastPosition = position;
            position += velocity * PreviewDeltaTimeStep + 0.5f * Physics.gravity * PreviewDeltaTimeStep * PreviewDeltaTimeStep;
            velocity += Physics.gravity * PreviewDeltaTimeStep;
            if (Physics.Linecast(lastPosition, position)) {
                ShouldEndInNextNextIteration++;
            }
        }
    }

    void Shoot() {
        Destroy(HoldProjectile);
        GameObject sentProjectile = Instantiate(MoleProjectile, InitialPosition, BoletzCamera.transform.rotation);
        Rigidbody rb = sentProjectile.GetComponent<Rigidbody>();
        rb.velocity = InitialVelocity;
        StartCoroutine(SetCooldownCoroutine(ShootCooldown));
        CurrentProjectileType = EStatics.EProjectile.None;
    }

    IEnumerator SetCooldownCoroutine(float Delay) {
        bIsCooldownCoroutinePlaying = true;
        LineRenderer.positionCount = 0;
        yield return new WaitForSeconds(Delay);
        bIsCooldownCoroutinePlaying = false;
    }
}
