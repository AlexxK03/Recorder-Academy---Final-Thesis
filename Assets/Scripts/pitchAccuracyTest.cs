using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pitchAccuracyTest : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioPitchEstimator pitchEstimator;

    void Start()
    {
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
        audioSource.Play();

        Debug.Log("Microphone recording started: " + microphoneName);
    }

    void Update()
    {
        float frequency = pitchEstimator.Estimate(audioSource);
        if (!float.IsNaN(frequency))
        {
            Debug.Log("Estimated Frequency: " + frequency + " Hz");
        }
        else
        {
            Debug.Log("No pitch detected.");
        }

        string detectedNote = GetNameFromFrequency(frequency).Trim();
        Debug.Log("Detected Note: " + detectedNote);
    }

    string GetNameFromFrequency(float frequency)
    {
        //Debug.Log("GetNameFromFrequency called with frequency: " + frequency);
        var noteNumber = Mathf.RoundToInt(12 * Mathf.Log(frequency / 440) / Mathf.Log(2) + 69);
        string[] names = {
            "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B","C'"
        };
        return names[noteNumber % 12].ToLower();
    }
}
