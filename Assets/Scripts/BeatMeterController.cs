using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatMeterController : MonoBehaviour
{
    public enum Beat
    {
        OnBeat,
        AlmostOnBeat,
        AlmostOffBeat,
        OffBeat
    }

    [Header("BPM")]
    [SerializeField] int bpm = 120;
    [SerializeField] public Beat state = Beat.OffBeat;

    [Header("Sprites")]
    [SerializeField] Image mainBar;
    [SerializeField] Image[] secondBars = new Image[2];
    [SerializeField] Image[] thirdBars = new Image[2];

    [Header("Debug")]
    [SerializeField] bool debug = false;
    [SerializeField] AudioClip clipOnBeat;

    // 
    private float startTime;
    private bool clipPlayed = false;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceStart = Time.time - startTime;
        float secondsPerBeat = 60.0f / (float)bpm;

        int currentBeat = (int)Mathf.Floor(timeSinceStart / secondsPerBeat); // number of beats since the beginning
        float currentBeatTimeStart = currentBeat * secondsPerBeat; 
        float currentBeatTime = Time.time - currentBeatTimeStart; // time spent since the last beat started

        Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color red = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        // according from the time since the start of the beat
        // set the state, color the sprites and play a debug sound (if needed)
        if (currentBeatTime < 0.1) // 100ms from the beat start
        {
            state = Beat.OnBeat;

            // if debug mode and the clip wasn't played on this beat, play it 
            if (clipPlayed == false && debug && audioSource)
            {
                clipPlayed = true;
                audioSource.PlayOneShot(clipOnBeat);
            }

            mainBar.color = red;
            secondBars[0].color = white;
            secondBars[1].color = white;
            thirdBars[0].color = white;
            thirdBars[1].color = white;
        }
        else if (currentBeatTime < 0.2) // 200ms from the beat start
        {
            state = Beat.AlmostOnBeat;

            // reset the audio clip debug
            clipPlayed = false;

            mainBar.color = white;
            secondBars[0].color = red;
            secondBars[1].color = red;
            thirdBars[0].color = white;
            thirdBars[1].color = white;
        }
        else if (currentBeatTime < 0.3) // 300ms from the beat start
        {
            state = Beat.AlmostOffBeat;

            mainBar.color = white;
            secondBars[0].color = white;
            secondBars[1].color = white;
            thirdBars[0].color = red;
            thirdBars[1].color = red;
        }
        else
        {
            state = Beat.OffBeat;
            mainBar.color = white;
            secondBars[0].color = white;
            secondBars[1].color = white;
            thirdBars[0].color = white;
            thirdBars[1].color = white;
        }
    }
}
