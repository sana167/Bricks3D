using UnityEngine;

[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(LevelManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private MenuController menuController;

    // This is now called by the InputHandler
    public void RequestQuit()
    {
        menuController.ShowQuitDialog();
    }
}