using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI chronoText;

    public TextMeshProUGUI checkpointTimes;

    public GameObject endScreen;

    public GameObject menu;

    public Player player;

    public Checkpoint currentCheckpoint;

    public GameObject naturalLights;

    public List<Material> jumpable;

    private float naturalLightsSpeedfactor = 1.0f;

    private float runTime = 0.0f;

    private bool gamePaused;

    private bool gameFinished;

    private bool escapeStickDownLast = false;

    //public bool submitStickDownLast = false;

    public static GameManager Instance { get; private set; }

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
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        runTime = 0.0f;
        CurrentCheckpoint.Activated = true;
        player.FallBackPosition = CurrentCheckpoint.transform.position + new Vector3(0, 0.5f, -1.0f);
        chronoText.text = ComputeTime();
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        if (!gamePaused)
        {
            runTime += Time.deltaTime;
            chronoText.text = ComputeTime();
            //updateNaturalLights();
        }

        if (Input.GetAxis("Escape") != 0 &&!gameFinished)
        {
            if (!escapeStickDownLast)
            {
                print("start " + Time.time);
                if (gamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
                
            }
            escapeStickDownLast = true;
        }
        else
        {
            escapeStickDownLast = false;
        }

        /*
        //TODO cleanup
        if (Input.GetAxis("Submit") != 0)
        {
            submitStickDownLast = true;
        }
        else
        {
            submitStickDownLast = false;
        }*/

    }

    private void Win()
    {
        endScreen.SetActive(true);
        endScreen.transform.Find("Final time").GetComponent<TextMeshProUGUI>().text += " " + ComputeTime();
        player.SaveVelocity();
        player.rb.isKinematic = true;
        gamePaused = true;
        gameFinished = true;
    }

    private string ComputeTime()
    {
        int minutes = (int)(runTime % 3600) / 60;
        int seconds = (int)(runTime % 3600) % 60;
        return "" + minutes + ":" + seconds + ":" + System.Math.Round(((runTime - (int) runTime) * 100),0);
    }

    private void updateNaturalLights()
    {
        foreach (Transform child in naturalLights.transform)
        {
            child.transform.Rotate(Time.deltaTime * naturalLightsSpeedfactor, 0, 0);
            if(child.name == "Sun")
            {
                if(child.transform.rotation.eulerAngles.x >= 280 || child.transform.rotation.eulerAngles.x <= 210)
                {
                   // print("Sun is on");
                    if(child.transform.rotation.eulerAngles.x >= 280)
                    {
                        child.GetComponent<Light>().intensity = 1.0f - (360.0f - child.transform.rotation.eulerAngles.x)/360.0f;             
                    } else
                    {
                        child.GetComponent<Light>().intensity = 1.0f;
                    }
                } else
                {
                    child.GetComponent<Light>().intensity = 0.0f;
                    //print("Sun is off");
                }
            }
        }
    }

    public void PauseGame()
    {
        player.SaveVelocity();
        player.rb.isKinematic = true;
        gamePaused = true;
        menu.SetActive(true);
    }

    public void ResumeGame()
    {
        player.rb.isKinematic = false;
        player.RecoverVelocity();
        gamePaused = false;
        menu.SetActive(false);
    }

    public bool IsPaused()
    {
        return gamePaused;
    }

   /* public bool LeavingPause()
    {
        return !submitStickDownLast && !gamePaused;
    }*/

}
