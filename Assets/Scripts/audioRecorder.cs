using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class audioRecorder : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioPitchEstimator pitchEstimator;
    public buttonInput buttonInput;
    public pointSystem pointSystem;
    public CubeStatus CubeStatus;
    public Material greenMaterial;
    public Material redMaterial;
    public GameObject targetArea;
    public Material originalTargetAreaMaterial;
    public int incorrectNoteCount = 0;
    public GameObject gameOverScreenbackgorund;
    public gameOverScreen gameOverScreen;

    public void Start()
    {
        // Initialize the audio source
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found!");
            }
        }

        string microphoneName = Microphone.devices[0]; // Use the first available microphone
        audioSource.clip = Microphone.Start(microphoneName, true, 2, 44100);
        audioSource.loop = true;
       // audioSource.Play();

        Debug.Log("Microphone recording started: " + microphoneName);

        // Initialize target area renderer 
        if (targetArea != null)
        {
            Renderer targetAreaRenderer = targetArea.GetComponent<Renderer>();
            if (targetAreaRenderer != null)
            {
                originalTargetAreaMaterial = targetAreaRenderer.material; //store the original material at start
            }
        }
    }

    public void StartRecording(GameObject targetCube)
    {
        Debug.Log(" StartRecording() triggered!");

        // Find the TargetCube
        if (targetCube == null)
        {
            Debug.LogError("TargetCube not found! Cannot check required note.");
            return;
        }

        // Getting cubsetatus from target cube
        CubeStatus cubeStatus = targetCube.GetComponent<CubeStatus>();
        if (cubeStatus == null)
        {
            cubeStatus = targetCube.AddComponent<CubeStatus>();
        }

        // Get the TMP_Text component from the TargetCube
        TMP_Text textMeshPro = targetCube.GetComponentInChildren<TMP_Text>();
        if (textMeshPro == null)
        {
            Debug.LogError("No TMP_Text found on TargetCube!");
            return;
        }

        string requiredNote = textMeshPro.text.Trim().ToLower();
        Debug.Log("Required Note from TargetCube: " + requiredNote);

        // Get the detected frequency and note
        float frequency = pitchEstimator.Estimate(audioSource);
        if (float.IsNaN(frequency))
        {
            Debug.LogError("Frequency is not a number Skipping.");
            return;
        }

        string detectedNote = GetNameFromFrequency(frequency).Trim().ToLower();
        Debug.Log("Detected Note: " + detectedNote);

        // Compare notes
        if (detectedNote == requiredNote)
        {
            Debug.Log("Correct note played!");
            pointSystem.AddPoint();
            cubeStatus.pointsAwarded = true; // Mark points as awarded for this cube
            ChangeTargetAreaToGreen();

        }
        else
        {
            Debug.Log("Incorrect note played.");
            ChangeTargetAreaToRed();
            incorrectNoteCount++; // Increment the incorrect note counter
            cubeStatus.pointsAwarded = false; // Mark points as not awarded for this cube
            Debug.Log("Incorrect note count: " + incorrectNoteCount);

            if (incorrectNoteCount >= 10)
            {
                GameOver();
            }
        }

        if (cubeStatus.pointsAwarded)
        {
            Debug.Log("Points already awarded for this cube. Skipping.");
            return;
        }
    }
    // Change the target area color to green for 2 seconds
    private void ChangeTargetAreaToGreen()
    {
        if (targetArea != null)
        {
            Renderer targetAreaRenderer = targetArea.GetComponent<Renderer>();
            if (targetAreaRenderer != null)
            {
                targetAreaRenderer.material = greenMaterial;
                Invoke(nameof(RestoreTargetAreaMaterial), 2f); // Restore after 2 seconds
            }
        }
    }
    // Change the target area color to red for 2 seconds
    private void ChangeTargetAreaToRed()
    {
        if (targetArea != null)
        {
            Renderer targetAreaRenderer = targetArea.GetComponent<Renderer>();
            if (targetAreaRenderer != null)
            {
                targetAreaRenderer.material = redMaterial;
                Invoke(nameof(RestoreTargetAreaMaterial), 2f); // Restore after 2 seconds
            }
        }
    }
    // Restore the original target area material
    private void RestoreTargetAreaMaterial()
    {
        if (targetArea != null)
        {
            Renderer targetAreaRenderer = targetArea.GetComponent<Renderer>();
            if (targetAreaRenderer != null)
            {
                targetAreaRenderer.material = originalTargetAreaMaterial;
            }
        }
    }
    // Get the note name from the frequency
    string GetNameFromFrequency(float frequency)
    {
        var noteNumber = Mathf.RoundToInt(12 * Mathf.Log(frequency / 440) / Mathf.Log(2) + 69);
        string[] names = {
            "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B","C'"
        };
        return names[noteNumber % 12].ToLower();
    }
    //Trigger the game over screen 
    public void GameOver()
    {
        gameOverScreen.Setup(pointSystem.CorrectHit);
        gameOverScreenbackgorund.SetActive(true);
        Time.timeScale = 0;
    }

}