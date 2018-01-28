using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoletzControl : MonoBehaviour
{
    public bool bIsListening = false;

    [SerializeField]
    Animator Anim;

    [SerializeField]
    GameObject Player;

    [SerializeField]
    BoletzCamera Camera;

    [SerializeField]
    Vector2 MouseSpeed;

    [SerializeField]
    float Speed;

    [SerializeField]
    float JumpForce;

    private bool bJumpOnCooldown = false;
    private Vector3 CameraPositionOffset;
    private PlayerNumber Number;
    private Rigidbody Rigid;
    private TrumpetControl Control;

    private bool bIsMoving = false;

    private void Awake() {
        Number = GetComponent<PlayerNumber>();
        Rigid = GetComponent<Rigidbody>();
        Control = GetComponent<TrumpetControl>();
    }

    void Start() {
        CameraPositionOffset = Camera.CameraOffset;
        Camera.transform.position = Player.transform.position + Camera.transform.localRotation * CameraPositionOffset;
    }

    void Update() {
        if (Input.GetButtonDown("Jump_" + Number.Number) && !bJumpOnCooldown) {
            bJumpOnCooldown = true;
            Rigid.AddForce(transform.up * JumpForce);
            Invoke("ResetJumpCooldown", 1.0f);
        }

        float playerHorizontalMovement = Input.GetAxis("Horizontal_" + Number.Number);
        float playerVerticalMovement = Input.GetAxis("Vertical_" + Number.Number);

        if (Mathf.Approximately(playerHorizontalMovement, 0.0f) && Mathf.Approximately(playerVerticalMovement, 0.0f)) {
            if (bIsMoving && !bIsListening && !Control.bIsShooting)
                Anim.Play("Idle");
            bIsMoving = false;
            return;
        }

        if (!bIsMoving && !bIsListening && !Control.bIsShooting) {
            Anim.Play("Move");
            bIsMoving = true;
        }

        Vector3 playerTranslation = Speed * Time.deltaTime * Vector3.Normalize(new Vector3(playerHorizontalMovement, 0, playerVerticalMovement));
        Player.transform.position = Player.transform.position + Player.transform.localRotation * playerTranslation;
    }

    void LateUpdate() {
        float cameraHorizontalAngle = Input.GetAxis("CameraX_" + Number.Number) * Time.deltaTime * MouseSpeed.x;
        float verticalAngle = -Input.GetAxis("CameraY_" + Number.Number) * Time.deltaTime * MouseSpeed.y;

        Vector3 cameraRotation = Camera.transform.localRotation.eulerAngles;
        float xAngle = cameraRotation.x + verticalAngle;
        if (xAngle < 0) {
            xAngle += 360;
        } else if (xAngle > 360) {
            xAngle -= 360;
        }

        //clamping camera range
        xAngle = (xAngle <= 179) ? Mathf.Clamp(xAngle, 0, 60) : Mathf.Clamp(xAngle, 300, 360);
        cameraRotation = new Vector3(xAngle, cameraRotation.y + cameraHorizontalAngle, cameraRotation.z);
        Camera.transform.localRotation = Quaternion.Euler(cameraRotation);

        //offset modifier
        Vector3 trueCameraOffset = CameraPositionOffset * (0.7f * (1.0f - Mathf.Abs((xAngle > 179) ? xAngle - 360.0f : xAngle) / 60.0f) + 0.3f);

        Quaternion playerRotation = Player.transform.localRotation;
        Player.transform.localRotation = Quaternion.Euler(new Vector3(playerRotation.x, cameraRotation.y, playerRotation.z));

        Vector3 offsetRot = Camera.transform.localRotation * trueCameraOffset;
        Camera.transform.position = Player.transform.position + new Vector3(offsetRot.x, CameraPositionOffset.y - ((xAngle > 179) ? (360.0f - xAngle) / 60.0f * 2.0f : 0.0f), offsetRot.z);
        //Camera.transform.position = new Vector3(Camera.transform.position.x, CameraPositionOffset.y, Camera.transform.position.z);
    }

    private void ResetJumpCooldown() {
        bJumpOnCooldown = false;
    }
}
