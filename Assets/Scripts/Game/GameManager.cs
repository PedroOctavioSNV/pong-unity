using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance of GameManager
    public static GameManager instance;

    [Header("Refs")]
    public GameUI gameUI;
    public GameAudio gameAudio;
    public Shake screenshake;
    public Ball ball;
    public Paddle paddlePlayer1;
    public Paddle paddlePlayer2;

    private int scorePlayer1, scorePlayer2;

    [Header("Configs")]
    [Tooltip("Maximum score needed to win the game")]
    public int maxScore = 5;

    // Delegate used as a reset event hook; external systems can subscribe via += to execute logic when the game resets.
    // Invoked with onReset?.Invoke() to safely trigger all subscribed methods if any.
    public Action onReset;
    public PlayMode playMode;
    public enum PlayMode
    {
        PlayerVsPlayer,
        PlayerVsAi
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
            gameUI.onStartGame += OnStartGame;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameUI.OnCancelGame();
        }
    }

    public void OnScoreZoneReached(int id)
    {
        if (id == 1)
        {
            scorePlayer1++;
        }

        if (id == 2)
        {
            scorePlayer2++;
        }

        gameUI.UpdateScores(scorePlayer1, scorePlayer2);
        gameUI.HighlightScore(id);
        gameAudio.PlayScoreSound();
        CheckWin();
    }

    private void CheckWin()
    {
        int winnerId = scorePlayer1 == maxScore ? 1 : scorePlayer2 == maxScore ? 2 : 0;

        if (winnerId != 0)
        {
            gameUI.OnGameEnds(winnerId);
        }
        else
        {
            onReset?.Invoke();
        }
    }

    private void OnStartGame()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        gameUI.UpdateScores(scorePlayer1, scorePlayer2);
        paddlePlayer1.ResetPosition();
        paddlePlayer2.ResetPosition();
    }

    private void OnDestroy()
    {
        gameUI.onStartGame -= OnStartGame;
    }

    public void SwitchPlayMode()
    {
        switch (playMode)
        {
            case PlayMode.PlayerVsPlayer:
                playMode = PlayMode.PlayerVsAi;
                break;
            case PlayMode.PlayerVsAi:
                playMode = PlayMode.PlayerVsPlayer;
                break;
        }
    }

    public bool IsPlayer2Ai()
    {
        return playMode == PlayMode.PlayerVsAi;
    }
}