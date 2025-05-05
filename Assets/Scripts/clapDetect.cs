// sing UnityEngine;

// public class clapDetect : MonoBehaviour
// {
//     public float clapThreshold = 5.5E-11f; // Threshold for detecting a peak

//     private AudioClip microphoneInput;
//     private int sampleRate = 44100; // Audio sample rate
//     private int sampleSize = 1024; // Number of audio samples for analysis
//     public float[] audioSamples; // Holds raw audio data
//     public float[] frequencySpectrum; // Holds frequency data after FFT



//     void Start()
//     {
//          foreach (string device in Microphone.devices)
//         {
//             Debug.Log("Available microphone: " + device);
//             string selectedMic = Microphone.devices[0]; 
//             Debug.Log("The selected Mic is" + selectedMic);
//         }

//         // Initialize arrays to hold audio and frequency data
//         audioSamples = new float[sampleSize];
//         frequencySpectrum = new float[sampleSize];

//         // Start the microphone
//         microphoneInput = Microphone.Start(null, true, 1, sampleRate);
//         Debug.Log("Mic On");
//     }

//     void Update()
//     {

       
//         // Check if microphone is recording
//         if (microphoneInput != null && Microphone.IsRecording(null))
//         {
//             AnalyzeAudio();

//             // Look for peaks in the frequency spectrum
//             if (DetectPeak(frequencySpectrum))
//             {
//                 Debug.Log("Peak detected!");
//             }
//             else{
//                 Debug.Log("No peak detected");
//             }
//         }
//     }

//     public void AnalyzeAudio()
//     {
//         // Get audio data from the microphone
//         microphoneInput.GetData(audioSamples, 0);
//         Debug.Log("Get Data");

//         // Perform frequency analysis using Unity's FFT
//         AudioListener.GetSpectrumData(frequencySpectrum, 0, FFTWindow.Rectangular);
//         Debug.Log("FFT");
//     }

//     public bool DetectPeak(float[] spectrum)
//     {
//         Debug.Log("DetectPeak");
//         // Check for values above the threshold in the frequency spectrum
//         foreach (float amplitude in spectrum)
//         {
//             Debug.Log($"my amp: {clapThreshold} {amplitude} {spectrum}");
//             if (amplitude > clapThreshold)
//             {
//                 Debug.Log("amp test");
//                 return true; // Peak detected
//             }
//         }

//         return false; // No peak detected
//     }

//     public void OnDisable()
//     {
//         // Stop the microphone when the script is disabled
//         if (Microphone.IsRecording(null))
//         {
//             Microphone.End(null);
//             Debug.Log("Mic Off.");
//         }
//     }

//     public void TestMethod()
//     {
//         Debug.Log("ScriptTrigger has access to clapDetect");
//     }
// }

