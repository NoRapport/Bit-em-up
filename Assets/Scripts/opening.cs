using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class opening : MonoBehaviour
{
    void Start()
    {
      StartCoroutine(WaitAndLoad(4));
    }

    private IEnumerator WaitAndLoad(float time)
    {
          yield return new WaitForSeconds(time * 1.0f);
          SceneManager.LoadScene("Base");
    }
}
