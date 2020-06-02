using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Singleton
    private static GameManager instance;

    public Player player;

    public int currentCheckpoint = 0;

    public static GameManager Instance { get { return instance; } }

    public int CurrentCheckpoint { get => currentCheckpoint;
        set
        {
            currentCheckpoint = value;
            player.FallBackPosition = Checkpoints[value].transform.position + new Vector3(0,0.5f,-1.0f);
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

}
