using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Singleton
    private static GameManager instance;

    public Player player;

    public Checkpoint currentCheckpoint;

    public static GameManager Instance { get { return instance; } }

    public Checkpoint CurrentCheckpoint { get => currentCheckpoint;
        set
        {
            currentCheckpoint.Activated = false;
            currentCheckpoint = value;
            player.FallBackPosition = Checkpoints[value.number].transform.position + new Vector3(0,0.5f,-1.0f);
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
    }

}
