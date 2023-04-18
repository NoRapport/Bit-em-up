using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;

    public static Pause instance;

    void Awake()
    {
      if (instance != null) {
        Debug.LogWarning("Il n'y a plus d'une instance de Pause dans la scene");
        return;
      }

      instance = this;
    }

    public void PausePressed()
    {
      if (gameIsPaused)
      {
        Resume();
      }
      else
      {
        Paused();
      }
    }

    void Paused()
    {
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0;
      gameIsPaused = true;
    }

    void Resume()
    {
      pauseMenuUI.SetActive(false);
      Time.timeScale = 1;
      gameIsPaused = false;
    }
}
