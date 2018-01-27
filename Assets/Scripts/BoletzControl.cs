using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoletzControl : MonoBehaviour {

    [SerializeField]
    GameObject Player;

    [SerializeField]
    BoletzCamera Camera;

    [SerializeField]
    Vector2 MouseSpeed;

    [SerializeField]
    float Speed;

    private Vector3 CameraPositionOffset;

    void Start () {
        CameraPositionOffset = Camera.CameraOffset;
        Camera.transform.position = Player.transform.position + Camera.transform.localRotation * CameraPositionOffset;
    }
	
	void Update () {
        float playerHorizontalMovement = Input.GetAxis("Horizontal");
        float playerVerticalMovement = Input.GetAxis("Vertical");

        // Applied so we don't normalize zero vector
        if (Mathf.Approximately(playerHorizontalMovement, 0.0f) && Mathf.Approximately(playerVerticalMovement, 0.0f)) { return; }

        Vector3 playerTranslation = Speed * Time.deltaTime * Vector3.Normalize(new Vector3(playerHorizontalMovement, 0, playerVerticalMovement));
        Player.transform.position = Player.transform.position + Player.transform.localRotation * playerTranslation;
	}

    void LateUpdate() {
        float cameraHorizontalAngle = Input.GetAxis("Mouse X") * Time.deltaTime * MouseSpeed.x;
        float verticalAngle = -Input.GetAxis("Mouse Y") * Time.deltaTime * MouseSpeed.y;

        Vector3 cameraRotation = Camera.transform.localRotation.eulerAngles;
        float xAngle = cameraRotation.x + verticalAngle;
        if (xAngle < 0) {
            xAngle += 360;
        }
        else if (xAngle > 360) {
            xAngle -= 360;
        }

        xAngle = (xAngle <= 179) ? Mathf.Clamp(xAngle, 0, 40) : Mathf.Clamp(xAngle, 320, 360);
        cameraRotation = new Vector3(xAngle, cameraRotation.y + cameraHorizontalAngle, cameraRotation.z);
        Camera.transform.localRotation = Quaternion.Euler(cameraRotation);

        Quaternion playerRotation = Player.transform.localRotation;
        Player.transform.localRotation = Quaternion.Euler(new Vector3(playerRotation.x, cameraRotation.y, playerRotation.z));

        Camera.transform.position = Player.transform.position + Camera.transform.localRotation * CameraPositionOffset;
    }
}
