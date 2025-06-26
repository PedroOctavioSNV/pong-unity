using System;
using UnityEngine;

/// <summary>
/// Gerencia o fluxo principal do jogo, incluindo pontuação, modos de jogo, eventos de início/fim e resete de partida.
/// Implementa o padrão Singleton para acesso global.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Instância Singleton do GameManager acessível globalmente
    public static GameManager instance;

    [Header("Refs")]
    [SerializeField] Paddle paddlePlayer1;         // Referência para o jogador 1
    [SerializeField] Paddle paddlePlayer2;         // Referência para o jogador 2 ou IA
    [SerializeField] GameAudio gameAudio;          // Referência para o controlador de áudio do jogo
    public GameUI gameUI;                          // Referência para a interface de usuário do jogo
    public Shake screenshake;                      // Referência para o efeito de tremor de tela
    public Ball ball;                              // Referência para a bola do jogo

    [Header("Configs")]
    [Tooltip("Pontuação máxima necessária para vencer o jogo")]
    [SerializeField] int maxScore = 5;             // Pontuação alvo para vencer o jogo

    private int scorePlayer1, scorePlayer2;        // Contadores de pontuação para os dois jogadores

    /// <summary>
    /// Evento acionado quando o jogo deve ser resetado (nova rodada).
    /// Outros sistemas podem se inscrever usando += para responder ao reset.
    /// </summary>
    public Action onReset;

    // Enum que define os modos de jogo disponíveis
    public PlayMode playMode;
    public enum PlayMode
    {
        PlayerVsPlayer,
        PlayerVsAi
    }

    void Awake()
    {
        // Implementação do padrão Singleton
        if (instance)
        {
            Destroy(instance); // Evita múltiplas instâncias
        }
        else
        {
            instance = this;
            gameUI.onStartGame += OnStartGame; // Inscreve-se no evento de início de jogo da UI
        }
    }

    void Update()
    {
        // Verifica se o jogador pressionou ESC para cancelar/voltar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameUI.OnCancelGame();
        }
    }

    /// <summary>
    /// Reinicia os elementos do jogo para uma nova partida.
    /// Reset de pontuação, atualização de UI e reposição de posições.
    /// </summary>
    void OnStartGame()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        gameUI.UpdateScores(scorePlayer1, scorePlayer2); // Atualiza a UI com pontuações zeradas
        paddlePlayer1.ResetPosition(); // Reposiciona o jogador 1
        paddlePlayer2.ResetPosition(); // Reposiciona o jogador 2 (ou IA)
    }

    void OnDestroy()
    {
        // Remove inscrição no evento ao destruir o objeto
        gameUI.onStartGame -= OnStartGame;
    }

    /// <summary>
    /// Alterna entre os modos de jogo disponíveis.
    /// Útil para trocar entre PvP e PvE.
    /// </summary>
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

    /// <summary>
    /// Verifica se o jogador 2 está sendo controlado por IA.
    /// </summary>
    /// <returns>Retorna verdadeiro se o modo de jogo atual for PlayerVsAi.</returns>
    public bool IsPlayer2Ai()
    {
        return playMode == PlayMode.PlayerVsAi;
    }

    /// <summary>
    /// Método chamado quando a bola entra na zona de pontuação.
    /// Atualiza a pontuação, destaca na UI, toca som e verifica vitória.
    /// </summary>
    /// <param name="id">Identificador do jogador que marcou (1 ou 2)</param>
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

        gameUI.UpdateScores(scorePlayer1, scorePlayer2); // Atualiza a UI com as novas pontuações
        gameUI.HighlightScore(id);                       // Destaca visualmente o jogador que pontuou
        gameAudio.PlayScoreSound();                      // Reproduz som de pontuação
        CheckWin();                                      // Verifica se algum jogador venceu
    }

    /// <summary>
    /// Verifica se algum jogador alcançou a pontuação máxima e finaliza o jogo.
    /// Se ninguém venceu, reinicia a rodada.
    /// </summary>
    void CheckWin()
    {
        int winnerId = scorePlayer1 == maxScore ? 1 : scorePlayer2 == maxScore ? 2 : 0;

        if (winnerId != 0)
        {
            gameUI.OnGameEnds(winnerId); // Notifica fim de jogo com o vencedor
        }
        else
        {
            onReset?.Invoke(); // Reinicia o jogo para próxima rodada
        }
    }
}