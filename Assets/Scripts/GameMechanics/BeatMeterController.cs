using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
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
    public Color beatPerfectColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

    [Header("Debug")]
    [SerializeField] bool debug = false;
    [SerializeField] AudioClip clipOnBeat;
    [SerializeField] Color debugColor = new Color(1.0f, 0.0f, 1.0f, 1.0f);

    //
    private float startTime;
    private bool clipPlayed = false;
    private AudioSource audioSource;

    private int currentBar = -1;
    public int selectedBar = -1;

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
        //Debug.Log(startTime);
    }

    // Update is called once per frame
    void Update()
    {
        int bpmInt = (int)bpm;
        bpmText.text = bpmInt.ToString();

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
            currentBar = -1;
        }
        else
        {
            if (currentBeatTimeAbs > timePerStep * 1.5f)
            {
                state = Beat.AlmostOffBeat;
                currentBar = currentBeatTime < 0 ? 0 : 4;
            }
            else if (currentBeatTimeAbs > timePerStep * 0.5f)
            {
                state = Beat.AlmostOnBeat;
                currentBar = currentBeatTime < 0 ? 1 : 3;
            }
            else
            {
                state = Beat.OnBeat;
                currentBar = 2;
            }

            if (leftToRight == false) // if the order is reversed, reverse the bar id: 0->4, 1->3, 3->1, 4->0
                currentBar = 4 - currentBar;
        }

        // color the sprites and play a debug sound (if needed)
        {
            // retrieve the color and scales for each bar, when they are the current
            Color[] colors = new Color[5] {
                almostOffBeatColor,
                almostOnBeatColor,
                onBeatColor,
                almostOnBeatColor,
                almostOffBeatColor
            };

            Vector3[] scales = new Vector3[5] {
                new Vector3(1.3f, 1.5f, 1.0f),
                new Vector3(1.3f, 1.5f, 1.0f),
                new Vector3(2.0f, 1.6f, 1.0f),
                new Vector3(1.3f, 1.5f, 1.0f),
                new Vector3(1.3f, 1.5f, 1.0f)
            };

            // reset each bar to their default color/scale
            for (int i = 0; i < 5; i++)
            {
                bars[i].color = offBeatColor;
                bars[i].transform.localScale = new Vector3(1, 1, 1);
            }

            // for the current bar apply its color and scale
            if (currentBar != -1)
            {
                bars[currentBar].color = debug ? debugColor : colors[currentBar];
                bars[currentBar].transform.localScale = scales[currentBar];
            }

            // put the selected bar in green
            if (selectedBar != -1)
            {
                bars[selectedBar].color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            }

            // if debug mode and the clip wasn't played on this beat, play it
            if (state == Beat.OnBeat)
            {
                if (clipPlayed == false && audioSource && debug)
                {
                    clipPlayed = true;
                    audioSource.PlayOneShot(clipOnBeat);
                }
            }
            else // reset the audio clip debug
            {
                clipPlayed = false;
            }
        }
    }

    public void synchBpm(float value)
    {
      bpm = 60/value;
    }

    public void setCurrentBarActive()
    {
        selectedBar = currentBar;
    }

    public void unsetActiveBar()
    {
        selectedBar = -1;
    }
}
