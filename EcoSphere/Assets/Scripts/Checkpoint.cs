using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int number;
    private bool activated;

    public Vector3 camera_offset_modification;
    public Vector3 camera_rotation_modification;


    public GameObject model_on;
    public GameObject model_off;

    public Range range;

    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    private void Start()
    {
        model_off.SetActive(true);
        model_on.SetActive(false);
    }

    public bool Activated {
        get => activated;
        set
        {      
            if (value)
            {
                if( !activated)
                {
                    audioSource.PlayOneShot(clip, volume);
                    //print("Checkpoint " + number + " activated");
                    GameManager.Instance.CurrentCheckpoint = this;
                    model_off.SetActive(false);
                    model_on.SetActive(true);
                    GameManager.Instance.ChangeCameraOffset(camera_offset_modification);
                    GameManager.Instance.ChangeCameraRotation(camera_rotation_modification);
                }
            } else
            {
                model_off.SetActive(true);
                model_on.SetActive(false);
            }
            activated = value;
        }
    }

    public void EnterInRange(Range range)
    {      
        if (!Activated && GameManager.Instance.CurrentCheckpoint.number == number-1)
        {
            Activated = true;
        }
    }
}

