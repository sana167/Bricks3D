using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private BallMovement ball;
    [SerializeField] private int totalBricks;
    [SerializeField] private BrickLevelSpawner brickLevelSpawner;

    public void SetTotalBricks(int count)
    {
        totalBricks = count;
    }

    public void BrickDestroyed()
    {
        totalBricks--;

        if (totalBricks <= 0)
        {
            LevelCompleted();
        }
    }

    private void LevelCompleted()
    {
        Debug.Log("Level completed!");

        if (ball != null)
        {
            ball.ResetBall();
        }
        if (brickLevelSpawner != null)
        {
            brickLevelSpawner.SpawnLevel(MainMenu.GetNextLevel());
        }
    }
}