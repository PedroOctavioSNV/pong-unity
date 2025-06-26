using System;
using UnityEngine;

/// <summary>
/// Gerencia o fluxo principal do jogo, incluindo pontua��o, modos de jogo, eventos de in�cio/fim e resete de partida.
/// Implementa o padr�o Singleton para acesso global.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Inst�ncia Singleton do GameManager acess�vel globalmente
    public static GameManager instance;

    [Header("Refs")]
    [SerializeField] Paddle paddlePlayer1;         // Refer�ncia para o jogador 1
    [SerializeField] Paddle paddlePlayer2;         // Refer�ncia para o jogador 2 ou IA
    [SerializeField] GameAudio gameAudio;          // Refer�ncia para o controlador de �udio do jogo
    public GameUI gameUI;                          // Refer�ncia para a interface de usu�rio do jogo
    public Shake screenshake;                      // Refer�ncia para o efeito de tremor de tela
    public Ball ball;                              // Refer�ncia para a bola do jogo

    [Header("Configs")]
    [Tooltip("Pontua��o m�xima necess�ria para vencer o jogo")]
    [SerializeField] int maxScore = 5;             // Pontua��o alvo para vencer o jogo

    private int scorePlayer1, scorePlayer2;        // Contadores de pontua��o para os dois jogadores

    /// <summary>
    /// Evento acionado quando o jogo deve ser resetado (nova rodada).
    /// Outros sistemas podem se inscrever usando += para responder ao reset.
    /// </summary>
    public Action onReset;

    // Enum que define os modos de jogo dispon�veis
    public PlayMode playMode;
    public enum PlayMode
    {
        PlayerVsPlayer,
        PlayerVsAi
    }

    void Awake()
    {
        // Implementa��o do padr�o Singleton
        if (instance)
        {
            Destroy(instance); // Evita m�ltiplas inst�ncias
        }
        else
        {
            instance = this;
            gameUI.onStartGame += OnStartGame; // Inscreve-se no evento de in�cio de jogo da UI
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
    /// Reset de pontua��o, atualiza��o de UI e reposi��o de posi��es.
    /// </summary>
    void OnStartGame()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        gameUI.UpdateScores(scorePlayer1, scorePlayer2); // Atualiza a UI com pontua��es zeradas
        paddlePlayer1.ResetPosition(); // Reposiciona o jogador 1
        paddlePlayer2.ResetPosition(); // Reposiciona o jogador 2 (ou IA)
    }

    void OnDestroy()
    {
        // Remove inscri��o no evento ao destruir o objeto
        gameUI.onStartGame -= OnStartGame;
    }

    /// <summary>
    /// Alterna entre os modos de jogo dispon�veis.
    /// �til para trocar entre PvP e PvE.
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
    /// Verifica se o jogador 2 est� sendo controlado por IA.
    /// </summary>
    /// <returns>Retorna verdadeiro se o modo de jogo atual for PlayerVsAi.</returns>
    public bool IsPlayer2Ai()
    {
        return playMode == PlayMode.PlayerVsAi;
    }

    /// <summary>
    /// M�todo chamado quando a bola entra na zona de pontua��o.
    /// Atualiza a pontua��o, destaca na UI, toca som e verifica vit�ria.
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

        gameUI.UpdateScores(scorePlayer1, scorePlayer2); // Atualiza a UI com as novas pontua��es
        gameUI.HighlightScore(id);                       // Destaca visualmente o jogador que pontuou
        gameAudio.PlayScoreSound();                      // Reproduz som de pontua��o
        CheckWin();                                      // Verifica se algum jogador venceu
    }

    /// <summary>
    /// Verifica se algum jogador alcan�ou a pontua��o m�xima e finaliza o jogo.
    /// Se ningu�m venceu, reinicia a rodada.
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
            onReset?.Invoke(); // Reinicia o jogo para pr�xima rodada
        }
    }
}