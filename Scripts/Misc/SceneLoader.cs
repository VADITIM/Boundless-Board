using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // simple scene loader to handle scene loading on button click
    
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}