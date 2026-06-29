using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Button noButton;

    public void ShowQuitDialog()
    {
        Time.timeScale = 0f; // Pause the game
        confirmationPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(noButton.gameObject);
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("TitleScreen");
    }

    public void Cancel()
    {
        confirmationPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
}