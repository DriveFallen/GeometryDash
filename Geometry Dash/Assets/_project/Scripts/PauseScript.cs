using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseFrame;
    private bool _isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                _isPaused = false;
                Time.timeScale = 1.0f;
                pauseFrame.SetActive(false);

                AudioSource music = GameObject.Find("SoundSystem").GetComponent<AudioSource>();
                music.Play();
            }
            else
            {
                _isPaused = true;
                Time.timeScale = 0.0f;
                pauseFrame.SetActive(true);

                AudioSource music = GameObject.Find("SoundSystem").GetComponent<AudioSource>();
                music.Pause();
            }
        }
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1.0f;
        pauseFrame.SetActive(false);

        AudioSource music = GameObject.Find("SoundSystem").GetComponent<AudioSource>();
        music.Play();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
