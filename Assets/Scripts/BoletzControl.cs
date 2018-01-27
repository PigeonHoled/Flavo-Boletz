using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoletzControl : MonoBehaviour {

    [SerializeField]
    GameObject Player;

    [SerializeField]
    float Speed;

	void Start () {
		
	}
	
	void Update () {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 translation = Speed * Time.deltaTime * Vector3.Normalize(new Vector3(horizontalMovement, 0, verticalMovement));
        Player.transform.position = Player.transform.position + translation;
	}
}
