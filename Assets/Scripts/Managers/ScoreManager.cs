using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public float score;

    private void Start()
    {
        score = 0f;
        scoreText.text = score.ToString() + " Points";
    }

    private void Update()
    {
        scoreText.text = score.ToString() + " Points";
    }
}
