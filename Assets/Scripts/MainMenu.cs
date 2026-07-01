using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private static int totalLevels = 2;
    private static int SelectedLevel = 1;
    [SerializeField] private TMP_Dropdown levelDropdown;
    [SerializeField] private Button playButton;

    public static int GetSelectedLevel()
    {
        return SelectedLevel;
    }

    public static int GetNextLevel()
    {
        SelectedLevel++;
        if (SelectedLevel > totalLevels)
        {
            SelectedLevel = 1;
        }
        return SelectedLevel;
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }

    public void PlayGame()
    {
        SelectedLevel = levelDropdown.value + 1;
        SceneManager.LoadScene("GameBoard");
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}