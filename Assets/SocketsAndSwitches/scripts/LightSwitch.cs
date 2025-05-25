using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


    public class LightSwitch : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onClickEvent;
        public UnityEvent OnClickEvent => onClickEvent;

        [Header("Light Objects")]
        //list of light objects to be turned on/off
        [SerializeField] private List<GameObject> lightObject;
      
        public bool lightsOn = false;

        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;

        [SerializeField] private AudioSource audioSource;


        private void Awake()
        {
            if (lightObject == null)
            {
                Debug.LogError("Light object is not assigned in the inspector.");
            }
            else
            {
                foreach (GameObject light in lightObject)
                {
                    if (light == null)
                    {
                        Debug.LogError("One or more light objects are not assigned in the inspector.");
                    }
                }
            }
        }
        public void SwitchLights()
        {
            lightsOn = !lightsOn;
            audioSource.PlayOneShot(buttonClickedSfx);
            if (lightsOn)
            {
                foreach (GameObject light in lightObject)
                {
                    light.SetActive(true);
                }
                onClickEvent?.Invoke();
            }
            else
            {
                foreach (GameObject light in lightObject)
                {
                    light.SetActive(false);
                }
                onClickEvent?.Invoke();
            }
        }

    }  
