using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    private void Start()
    {
    
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("map");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}