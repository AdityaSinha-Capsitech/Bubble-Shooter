using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public Button button;
    public GameObject handleGame;
    public static GameController gameInstance;
    void Start()
    {
        gameInstance = this;
    }

    public void PauseGame()
    {
      
        FireCannon.fireInstance.DestroySpawnBubble();
        handleGame.SetActive(true);
        SetActivePauseButton(false);
        Time.timeScale = 0;  
    }
    public void ResumeGame()
    {
      
        handleGame.SetActive(false);
        SetActivePauseButton(true);
        Time.timeScale = 1;  
    }
    public void QuitGame()
    {
       #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
       #endif
            Application.Quit();
    }

    public void SetActivePauseButton(bool status)
    {
        button.gameObject.SetActive(status);
    }
}
