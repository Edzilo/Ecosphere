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

    public static GameManager Instance { get { return instance; } }

    public Checkpoint CurrentCheckpoint { get => currentCheckpoint;
        set
        {
            currentCheckpoint.Activated = false;
            currentCheckpoint = value;
            player.FallBackPosition = Checkpoints[value.number].transform.position + new Vector3(0,0.5f,-1.0f);
            if(CurrentCheckpoint.number != 0)checkpointTimes.text += ComputeTime() + "\n";
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
    }

    private void Win()
    {
        print("You won");
    }

    private string ComputeTime()
    {
        //int hours = (int)Time.time / 3600;
        int minutes = (int)(Time.time % 3600) / 60;
        int seconds = (int)(Time.time % 3600) % 60;
        return "" + minutes + ":" + seconds + ":" + System.Math.Round((( Time.time - (int) Time.time) * 100),0);
    }

}
