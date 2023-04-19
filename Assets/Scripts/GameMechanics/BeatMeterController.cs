using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BeatMeterController : MonoBehaviour
{
  public static BeatMeterController instance;

    public Text bpmText;

    public enum Beat
    {
        OnBeat,
        AlmostOnBeat,
        AlmostOffBeat,
        OffBeat
    }

    [Header("BPM")]
    [SerializeField] public float bpm = 120;
    [SerializeField] float timePerStep = 0.1f;
    [SerializeField] bool leftToRight = false;
    [SerializeField] public Beat state = Beat.OffBeat;

    [Header("Sprites")]
    [SerializeField] Image[] bars = new Image[5];
    [SerializeField] Color onBeatColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    [SerializeField] Color almostOnBeatColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    [SerializeField] Color almostOffBeatColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    [SerializeField] Color offBeatColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    [Header("Debug")]
    [SerializeField] bool debug = false;
    [SerializeField] AudioClip clipOnBeat;
    [SerializeField] Color debugColor = new Color(1.0f, 0.0f, 1.0f, 1.0f);

    //
    private float startTime;
    private bool clipPlayed = false;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        //Yes sure, why not a checkbox ;P
        leftToRight = false;

        if (instance != null) {
          Debug.LogWarning("Il n'y a plus d'une instance de BeatMeter dans la scene");
          return;
        }

        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.timeSinceLevelLoad;
        Debug.Log(startTime);
    }

    // Update is called once per frame
    void Update()
    {
        bpmText.text = bpm.ToString();

        float timeSinceStart = Time.time - startTime;
        float secondsPerBeat = 60.0f / (float)bpm;

        int currentBeat = (int)Mathf.Round(timeSinceStart / secondsPerBeat); // number of beats since the beginning (rounded)
        float currentBeatTimeStart = currentBeat * secondsPerBeat;
        float currentBeatTime = Time.time - currentBeatTimeStart; // time spent since the last beat started, from [-secondsPerBeat/2;secondsPerBeat/2]
        float currentBeatTimeAbs = Mathf.Abs(currentBeatTime);


        // set the state according to the time since the start of the beat
        if (currentBeatTimeAbs > timePerStep * 2.5f)
        {
            state = Beat.OffBeat;
        }
        else if (currentBeatTimeAbs > timePerStep * 1.5f)
        {
            state = Beat.AlmostOffBeat;
        }
        else if (currentBeatTimeAbs > timePerStep * 0.5f)
        {
            state = Beat.AlmostOnBeat;
        }
        else
        {
            state = Beat.OnBeat;
        }


        // color the sprites and play a debug sound (if needed)
        for (int i = 0; i < 5; i++ )
        {
            bars[i].color = offBeatColor;
            bars[i].transform.localScale = new Vector3(1, 1, 1);
        }

        if (state == Beat.OnBeat)
        {
            bars[2].color = debug ? debugColor : onBeatColor;
            bars[2].transform.localScale = new Vector3(2, 1.6f, 1);

            // if debug mode and the clip wasn't played on this beat, play it
            if (clipPlayed == false && audioSource && debug)
            {
                clipPlayed = true;
                audioSource.PlayOneShot(clipOnBeat);
            }
        }
        else if (state == Beat.AlmostOnBeat)
        {
            if (leftToRight)
            {
                bars[currentBeatTime < 0 ? 1 : 3].color = debug ? debugColor : almostOnBeatColor;
                bars[currentBeatTime < 0 ? 1 : 3].transform.localScale = new Vector3(1.3f, 1.5f, 1);
            }
            else
            {
                bars[currentBeatTime > 0 ? 1 : 3].color = debug ? debugColor : almostOnBeatColor;
                bars[currentBeatTime > 0 ? 1 : 3].transform.localScale = new Vector3(1.3f, 1.5f, 1);
            }

            // reset the audio clip debug
            clipPlayed = false;
        }
        else if (state == Beat.AlmostOffBeat)
        {

            if (leftToRight)
            {
                bars[currentBeatTime < 0 ? 0 : 4].color = debug ? debugColor : almostOffBeatColor;
                bars[currentBeatTime < 0 ? 1 : 3].transform.localScale = new Vector3(1.3f, 1.5f, 1);
            }
            else
            {
                bars[currentBeatTime > 0 ? 0 : 4].color = debug ? debugColor : almostOffBeatColor;
                bars[currentBeatTime > 0 ? 0 : 4].transform.localScale = new Vector3(1.3f, 1.5f, 1);
            }
        }
    }

    public void synchBpm(float value)
    {
      bpm = 60/value;
      Debug.Log(bpm);
    }
}
