using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public float playerScore;

    private void Start()
    {
        playerScore = 0f;
        scoreText.text = playerScore.ToString() + " Points";
    }

    private void Update()
    {
        scoreText.text = playerScore.ToString() + " Points";
    }
}
