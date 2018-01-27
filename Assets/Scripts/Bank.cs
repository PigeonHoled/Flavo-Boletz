using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour {
    [SerializeField]
    float BanksHeight;

    [SerializeField]
    int PlayerID = 0;

    [SerializeField]
    int PointsForFlyingPart = 1000;

    private Vector3 InitialBanksModelPosition;

    private void Awake() {
        InitialBanksModelPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == (int) EStatics.ELayer.BankPart) {
            BankManager.Instance.ChangeScore(PointsForFlyingPart, PlayerID);
            Destroy(other.gameObject);
            float pointsRatio = (float)BankManager.Instance.Scores[PlayerID] / (float)BankManager.Instance.MaxPoints;
            Debug.Log(pointsRatio);
            transform.position = new Vector3(InitialBanksModelPosition.x, InitialBanksModelPosition.y + pointsRatio * BanksHeight, InitialBanksModelPosition.z);
        }
    }
}
