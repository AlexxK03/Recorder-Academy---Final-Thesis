using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//Handles the three second timer before game start. 
public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public float remainingTime;
    public GameObject startScreen;
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            startScreen.SetActive(false);

        }
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = seconds.ToString();

    }
}
