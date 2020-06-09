using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // Singleton
    private static GameManager instance;

    public Text chronoText;

    public Player player;

    public Checkpoint currentCheckpoint;

    public static GameManager Instance { get { return instance; } }

    public Checkpoint CurrentCheckpoint { get => currentCheckpoint;
        set
        {
            currentCheckpoint.Activated = false;
            currentCheckpoint = value;
            player.FallBackPosition = Checkpoints[value.number].transform.position + new Vector3(0,0.5f,-1.0f);
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
        SetTime();
    }

    private void Update()
    {
        SetTime();
    }

    private void Win()
    {
        print("You won");
    }

    private void SetTime()
    {
        //int hours = (int)Time.time / 3600;
        int minutes = (int)(Time.time % 3600) / 60;
        int seconds = (int)(Time.time % 3600) % 60;
        chronoText.text = "" + minutes + ":" + seconds + ":" + System.Math.Round((( Time.time - (int) Time.time) * 10),2);
    }
}
