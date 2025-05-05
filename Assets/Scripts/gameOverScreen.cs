using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameOverScreen : MonoBehaviour
{
    public TMP_Text pointsText;
    public void Setup(int score)
    {
        pointsText.text = "Total Points: " + score.ToString();
    }

}
