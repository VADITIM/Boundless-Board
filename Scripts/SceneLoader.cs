using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


}