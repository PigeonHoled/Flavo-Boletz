using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMan : MonoBehaviour {

    public void LoadMainScene() {
        Debug.Log("clicked");
        Application.LoadLevel(1);
    }

    public void LoadIntroScene() {
        Application.LoadLevel(0);
    }
}
