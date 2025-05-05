// using System.Collections;
// using UnityEngine;
// using UnityEngine.Events;
// using System.Collections.Generic;
// using Unity.VisualScripting;


// public class TriggerArea : MonoBehaviour
// {
//     [SerializeField] UnityEvent onTriggerEnter;
//     [SerializeField] UnityEvent onTriggerExit;
// clapDetect ClapDetect;
//     private void OnTriggerEnter(Collider other)
//     {
//         onTriggerEnter.Invoke();
//         if (other.CompareTag("noteBlock")) 
//         {
//             Debug.Log("note block entered the trigger area!");

//             if(ClapDetect !=null){
//             ClapDetect.TestMethod();
//             ClapDetect.enabled = true;
//             //ClapDetect.AnalyzeAudio();
//             //ClapDetect.DetectPeak(float[] spectrum);
//             }
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         onTriggerExit.Invoke();

//         if (other.CompareTag("noteBlock"))
//         {
//             Debug.Log("noteblock exited the trigger area!");
//             ClapDetect.enabled = false;

//         }
//     }
// }
