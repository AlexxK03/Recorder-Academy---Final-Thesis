using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBlockTrigger : MonoBehaviour
{
    public string TargetCubeTag = "TargetCube"; // Tag for the target cube

    public string[] _names = {
        "C", "D",  "E", "F", "G", "A", "B", "C'"
    };
    public float[] _positions = {
        -9.59f, -7.87f, -6.26f, -4.50f, -2.91f, -1.23f, 0.45f, 2.15f
    };

    private Dictionary<string, float> _namePositionMap;

    private void Start()
    {
        _namePositionMap = new Dictionary<string, float>();
        for (int i = 0; i < _names.Length; i++)
        {
            _namePositionMap.Add(_names[i], _positions[i]);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BlockSpawnLayer")) // Only react to this layer
        {
            // Find the target cube by tag
            GameObject targetCube = GameObject.FindGameObjectWithTag(TargetCubeTag);
            if (targetCube != null)
            {
                // Instantiate a new block and store reference
                GameObject newCube = Instantiate(targetCube);

                if (!newCube.GetComponent<CubeStatus>())
                {
                    CubeStatus cubeStatus = newCube.AddComponent<CubeStatus>();
                    cubeStatus.pointsAwarded = false; // Mark points as not awarded for this cube 
                    Debug.Log("CubeStatus component added to the new cube. Points awarded: " + cubeStatus.pointsAwarded);
                }

                // Modify only the new block's TextMeshPro component
                TextMeshPro textComponent = newCube.GetComponentInChildren<TextMeshPro>();
                if (textComponent != null)
                {
                    string randomName = _names[Random.Range(0, _names.Length)];
                    textComponent.text = randomName;

                    float position;
                    if (_namePositionMap.TryGetValue(randomName, out position))
                    {
                        Vector3 spawnPosition = new Vector3(-13.22f, -0.98f, position);
                        newCube.transform.position = spawnPosition;
                        newCube.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    }
                }
            }
            else
            {
                Debug.LogError("Target cube with tag " + TargetCubeTag + " not found.");
            }
        }
    }
}