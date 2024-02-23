using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{  
    public static void Restart()
    {
        int currentSceneID = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneID);
    }

    public static IEnumerator LoadNextScene()
    {
        int currentSceneID = SceneManager.GetActiveScene().buildIndex;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(currentSceneID + 1);
    }

    public void LoadLevel_1()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel_2()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLevel_3()
    {
        SceneManager.LoadScene(3);
    }

    public void Quit()
    {
        Application.Quit();
    }
}