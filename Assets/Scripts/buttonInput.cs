using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonInput : MonoBehaviour
{
    //Script handles target area trigger and recording start/stop.
    public audioRecorder audioRecorder;
    private GameObject currentTargetCube = null;
    public Material blueMaterial;


    // Dictionary to store original materials
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TargetCube"))
        {
            //When note enters target area, start recording is triggered repeatedly every 0.5 seconds.
            Debug.Log("TargetCube entered trigger zone, starting recording.");
            currentTargetCube = other.gameObject;
            InvokeRepeating("StartRecording", 0f, 0.5f);

            //handles the note color change when it enters the target area.
            //Skips TextMeshPro renderers to ensure that only the note changes colot.
            Renderer[] renderers = other.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                Debug.Log("Renderers found! Changing materials now.");
                foreach (Renderer renderer in renderers)
                {
                    // Skip TextMeshPro renderers
                    if (renderer.GetComponent<TMPro.TextMeshPro>() != null || renderer.GetComponent<TMPro.TextMeshProUGUI>() != null)
                    {
                        Debug.Log("Skipping TextMeshPro renderer.");
                        continue;
                    }

                    // Store the original material
                    if (!originalMaterials.ContainsKey(renderer))
                    {
                        originalMaterials[renderer] = renderer.material;
                    }

                    // Change to blue material
                    renderer.material = blueMaterial;
                }
            }
            else
            {
                Debug.LogWarning("No Renderers found on the TargetCube or its children.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //When note exits target area, stop recording is triggered and the original material is restored.
        if (other.CompareTag("TargetCube"))
        {
            Debug.Log("TargetCube exited trigger zone, stopping recording.");
            CancelInvoke("StartRecording");

            Renderer[] renderers = other.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                // Skip TextMeshPro renderers
                if (renderer.GetComponent<TMPro.TextMeshPro>() != null || renderer.GetComponent<TMPro.TextMeshProUGUI>() != null)
                {
                    continue;
                }

                // Restore the original material
                if (originalMaterials.ContainsKey(renderer))
                {
                    renderer.material = originalMaterials[renderer];
                }
            }

            // Clear the reference to the current target cube
            currentTargetCube = null;
        }
    }

    private void StartRecording()
    {
        if (audioRecorder != null)
        {
            audioRecorder.StartRecording(currentTargetCube);
        }
        else
        {
            Debug.LogError("AudioRecorder not assigned in buttonInput!");
        }
    }
}