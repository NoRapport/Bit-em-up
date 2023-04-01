using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieToQuit : MonoBehaviour
{
    public Slider playerLife;

    void Update()
    {
      if (playerLife.value <= 0 && Time.time >= 1) {
        Application.Quit();
      }
    }
}
