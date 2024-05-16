using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static int score = 0;
    public Text scoreText;

    void Update()
    {
        scoreText.text = "Score: " + ScoreCounter.score; 
    }
}
