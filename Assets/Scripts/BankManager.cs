using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankManager : MonoBehaviour {

    public int MaxPoints {
        get { return maxPoints; }
        private set { maxPoints = value; }
    }

    [SerializeField]
    int maxPoints = 20000;

    public static BankManager Instance = null;
    public List<int> Scores { get; private set; }

    BankManager() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void ChangeScore(int ChangeInPoints, int PlayerID) {
        if (PlayerID < 0 || PlayerID > 2) {
            Debug.LogError("Incorrect playerID sent to ChangeScore");
            return;
        }

        Scores[PlayerID] += ChangeInPoints;
    }

    private void Start() {
        Scores = new List<int>();
        Scores.Add(0);
        Scores.Add(0);
    }
}
