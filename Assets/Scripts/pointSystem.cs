using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pointSystem : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text pointAwardedText; // Add this for the point-awarded message
    public int CorrectHit = 0;

    private void Start()
    {
        UpdateScoreUI();
        pointAwardedText.gameObject.SetActive(false); // Ensure the message is hidden initially
    }

    public void AddPoint()
    {
        CorrectHit++;
        UpdateScoreUI();
        StartCoroutine(DisplayPointAwardedMessage("Point Awarded +1")); // Show "+1" when a point is added
    }

    private void UpdateScoreUI()
    {
        scoreText.text = CorrectHit.ToString();
    }

    private IEnumerator DisplayPointAwardedMessage(string message)
    {
        pointAwardedText.text = message;
        pointAwardedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f); // Display the message for 1 second
        pointAwardedText.gameObject.SetActive(false);
    }
}