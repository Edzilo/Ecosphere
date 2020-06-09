using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // Singleton
    private static GameManager instance;

    public Text chronoText;

    public Text checkpointTimes;

    public Player player;

    public Checkpoint currentCheckpoint;

    public GameObject naturalLights;

    private float naturalLightsSpeedfactor = 2.0f;

    public static GameManager Instance { get { return instance; } }

    public Checkpoint CurrentCheckpoint { get => currentCheckpoint;
        set
        {
            currentCheckpoint.Activated = false;
            currentCheckpoint = value;
            player.FallBackPosition = Checkpoints[value.number].transform.position + new Vector3(0,0.5f,-1.0f);
            if(CurrentCheckpoint.number != 0)checkpointTimes.text += "(" + currentCheckpoint.number 
                    + ") " + ComputeTime() + "\n";
            if(currentCheckpoint.number == Checkpoints.Count - 1)
            {
                Win();
            }
        }
    }
    
    public List<Checkpoint> Checkpoints;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        CurrentCheckpoint.Activated = true;
        player.FallBackPosition = CurrentCheckpoint.transform.position + new Vector3(0, 0.5f, -1.0f);
        chronoText.text = ComputeTime();
    }

    private void Update()
    {
        chronoText.text = ComputeTime();
        updateNaturalLights();
    }

    private void Win()
    {
        print("You won");
        Time.timeScale = 0.0f;
    }

    private string ComputeTime()
    {
        //int hours = (int)Time.time / 3600;
        int minutes = (int)(Time.time % 3600) / 60;
        int seconds = (int)(Time.time % 3600) % 60;
        return "" + minutes + ":" + seconds + ":" + System.Math.Round((( Time.time - (int) Time.time) * 100),0);
    }

    private void updateNaturalLights()
    {
        foreach (Transform child in naturalLights.transform)
        {
            child.transform.Rotate(Time.deltaTime * naturalLightsSpeedfactor, 0, 0);
            if(child.name == "Sun")
            {
                if(child.transform.rotation.eulerAngles.x >= -20 && child.transform.rotation.eulerAngles.x <= 180)
                {
                    print("Sun is on");
                    child.GetComponent<Light>().intensity = 1.0f;
                } else
                {
                    child.GetComponent<Light>().intensity = 0.0f;
                    print("Sun is off");
                }
            }
        }
    }

}
