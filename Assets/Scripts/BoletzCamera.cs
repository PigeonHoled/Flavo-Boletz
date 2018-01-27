using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoletzCamera : MonoBehaviour {

    public Vector3 CameraOffset {
        get { return cameraOffset; }
        private set { cameraOffset = value; }
    }

    [SerializeField]
    private Vector3 cameraOffset;
}
