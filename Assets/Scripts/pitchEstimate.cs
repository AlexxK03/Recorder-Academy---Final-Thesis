using UnityEngine;

public class PitchEstimate : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioPitchEstimator AudioPitchEstimator;

    private const float C_NOTE_FREQ = 261.63f; // Middle C (C4)
    private const float TOLERANCE = 5.0f; // Allow slight variations in frequency

    void Start()
    {

        // foreach (string device in Microphone.devices)
        // {
        //Debug.Log("Available microphone: " + device);
        //     string selectedMic = Microphone.devices[0];
        //     Debug.Log("The selected Mic is" + selectedMic);
        // }

        // Use the microphone as the audio source
        //audioSource = gameObject.AddComponent<AudioSource>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 1, 44100);
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        Debug.Log($"Microphone started: {Microphone.IsRecording(null)}");

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Microphone failed to start!");
        }

        while (!(Microphone.GetPosition(null) > 0))
        {
            audioSource.Play(); // Wait until recording starts 
            Debug.Log("Recording has started");
        } 

    }

    void Update()
    {
        audioSource.clip = StartMicrophone();

        if (audioSource.clip == null)
        {
            Debug.LogWarning("Microphone not initialized.");
            return;
        }
        else
        {
            Debug.Log(audioSource.isPlaying);
        }

        if (AudioPitchEstimator == null)
        {
            Debug.LogError("AudioPitchEstimator is not assigned!");
            return;
        }
        // Get estimated pitch
        AudioPitchEstimator.TestMethod();
        float estimatedPitch = AudioPitchEstimator.Estimate(audioSource);
        Debug.Log("estimated pitch before =" + estimatedPitch);
       // Debug.Log(spectrum[i]);


        if (estimatedPitch > 0) // Ensure a valid pitch is detected
        {
            Debug.Log("estimated pitch isss =" + estimatedPitch);
            if (Mathf.Abs(estimatedPitch - C_NOTE_FREQ) <= TOLERANCE)
            {
                Debug.Log("Yes");
            }
            else
            {
                Debug.Log("No");
            }
        }
        else
        {

            Debug.LogError("estimatedPitch is null!");

        }
    }

    private AudioClip StartMicrophone()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            return null;
        }

        string micName = Microphone.devices[0]; // Use default mic
        Debug.Log(micName);
        return Microphone.Start(micName, true, 1, 44100);
    }
}

