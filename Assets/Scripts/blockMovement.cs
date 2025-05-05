using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockMovement : MonoBehaviour
{
    // Script moves the GameObject forward at a constant speed after a delay - delay is only applied to the first instance of the script.
    // Subsequent instances will move immediately without delay.
    //delay is implemented to allow for 3 second coutdown before the game starts.
    public float baseSpeed = 2f;
    private bool canMove = false; // Flag to control movement
    private static bool delayCompleted = false; // Static flag to track if the delay has already occurred   

    void Start()
    {
        if (!delayCompleted) // Only wait if the delay hasn't been completed
        {
            StartCoroutine(WaitBeforeStart());
        }
        else
        {
            canMove = true; // Allow movement immediately for subsequent instances
        }
    }

    IEnumerator WaitBeforeStart()
    {
        yield return new WaitForSeconds(4); // Wait for 3 seconds
        delayCompleted = true; // Mark the delay as completed
        canMove = true; // Allow movement
    }

    void Update()
    {
        if (canMove) // Only move if the flag is true
        {
            transform.Translate(Vector3.forward * baseSpeed * Time.deltaTime);
        }
    }
}