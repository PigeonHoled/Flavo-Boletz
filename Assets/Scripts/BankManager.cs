using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankManager : MonoBehaviour {

    public int MaxPoints {
        get { return maxPoints; }
        private set { maxPoints = value; }
    }

    public Text scoreText;

    [SerializeField]
    int maxPoints = 20000;

    public static BankManager Instance = null;
    public List<int> Scores { get; private set; }

    BankManager() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void Start() {
        scoreText.text = "";

        Scores = new List<int>();
        Scores.Add(0);
        Scores.Add(0);
    }

    public void ChangeScore(int ChangeInPoints, int PlayerID) {
        if (PlayerID < 0 || PlayerID > 2) {
            Debug.LogError("Incorrect playerID sent to ChangeScore");
            return;
        }

        Scores[PlayerID] += ChangeInPoints;
        if (Scores[PlayerID] >= maxPoints) {
            scoreText.text = "Player " + (PlayerID + 1).ToString() + " Won";
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame() {
        yield return new WaitForSeconds(5.0f);
        Application.LoadLevel(0);
    }
}
