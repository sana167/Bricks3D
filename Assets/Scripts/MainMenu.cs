using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static int SelectedLevel = 1;
    [SerializeField] private TMP_Dropdown levelDropdown;
    [SerializeField] private Button playButton;

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