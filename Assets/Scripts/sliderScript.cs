// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class sliderScript : MonoBehaviour
// {
//     public Slider slider;
//     public clapDetect ClapDetect;

//     // Start is called before the first frame update
//     void Start()
//     {
//         slider.value = ClapDetect.clapThreshold;
//         slider.onValueChanged.AddListener(UpdateValue);
//     }

//     // Update is called once per frame
//     void UpdateValue(float value)
//     {
//         ClapDetect.clapThreshold = value;
//     }
// }
