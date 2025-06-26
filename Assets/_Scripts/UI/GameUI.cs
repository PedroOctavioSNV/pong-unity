using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Controla a interface do usu�rio do jogo (menus, placares, bot�es, textos).
/// Gerencia intera��es e atualiza��es visuais da UI durante o jogo.
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] ScoreText scoreTextPlayer1, scoreTextPlayer2;   // Refer�ncias aos textos de pontua��o dos jogadores
    [SerializeField] GameObject menuObject;                          // Objeto do menu principal (ativo/inativo)
    [SerializeField] GameObject quitButton;                          // Bot�o de sair (desabilitado em WebGL)
    [SerializeField] TextMeshProUGUI winText;                        // Texto que exibe o vencedor ou t�tulo
    [SerializeField] TextMeshProUGUI volumeValueText;                // Texto que mostra o valor atual do volume
    [SerializeField] TextMeshProUGUI playModeButtonText;             // Texto do bot�o para alternar modo de jogo

    /// <summary>
    /// Evento disparado quando o jogo � iniciado pelo usu�rio.
    /// </summary>
    public Action onStartGame;

    void Start()
    {
        // Atualiza o texto do bot�o de modo de jogo conforme configura��o atual
        AdjustPlayModeButtonText();
        // Desativa o bot�o sair se a plataforma for WebGL (n�o suportado)
        CheckDisableQuitButton();
    }

    /// <summary>
    /// M�todo chamado ao clicar no bot�o "Start Game".
    /// Inicia o jogo escondendo o menu e disparando o evento onStartGame.
    /// </summary>
    public void OnStartGameButtonClicked()
    {
        Time.timeScale = 1;       // Retoma o tempo do jogo
        menuObject.SetActive(false); // Esconde o menu
        onStartGame?.Invoke();    // Dispara evento para iniciar o jogo
    }

    /// <summary>
    /// Atualiza os textos de pontua��o dos dois jogadores na UI.
    /// </summary>
    /// <param name="scorePlayer1">Pontua��o do jogador 1</param>
    /// <param name="scorePlayer2">Pontua��o do jogador 2</param>
    public void UpdateScores(int scorePlayer1, int scorePlayer2)
    {
        scoreTextPlayer1.SetScore(scorePlayer1);
        scoreTextPlayer2.SetScore(scorePlayer2);
    }

    /// <summary>
    /// Destaca o placar do jogador que marcou ponto.
    /// </summary>
    /// <param name="id">Identificador do jogador (1 ou 2)</param>
    public void HighlightScore(int id)
    {
        if (id == 1)
            scoreTextPlayer1.Highlight();
        else
            scoreTextPlayer2.Highlight();
    }

    /// <summary>
    /// Exibe o menu final com o texto indicando o vencedor.
    /// </summary>
    /// <param name="winnerId">Id do jogador vencedor</param>
    public void OnGameEnds(int winnerId)
    {
        menuObject.SetActive(true);
        winText.text = $"Player {winnerId} wins!";
    }

    /// <summary>
    /// Exibe o menu de pausa/cancelamento do jogo.
    /// Pausa o tempo e mostra t�tulo padr�o.
    /// </summary>
    public void OnCancelGame()
    {
        Time.timeScale = 0;    // Pausa o jogo
        menuObject.SetActive(true);
        winText.text = $"PongU";
    }

    /// <summary>
    /// Atualiza o volume do jogo e o texto que mostra seu valor em porcentagem.
    /// </summary>
    /// <param name="value">Valor do volume (0 a 1)</param>
    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        volumeValueText.text = $"{Mathf.RoundToInt(value * 100)} %";
    }

    /// <summary>
    /// Alterna o modo de jogo entre Player vs Player e Player vs AI,
    /// atualizando o texto do bot�o correspondente.
    /// </summary>
    public void OnSwitchPlayModeButtonClicked()
    {
        GameManager.instance.SwitchPlayMode();
        AdjustPlayModeButtonText();
    }

    /// <summary>
    /// Fecha o jogo ao clicar no bot�o Quit.
    /// Funciona apenas em builds nativas (n�o WebGL).
    /// </summary>
    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    /// <summary>
    /// Desabilita o bot�o Quit caso a plataforma seja WebGL,
    /// pois n�o � poss�vel sair do navegador via script.
    /// </summary>
    void CheckDisableQuitButton()
    {
#if UNITY_WEBGL
        quitButton.SetActive(false);
#endif
    }

    /// <summary>
    /// Ajusta o texto do bot�o de modo de jogo de acordo com a configura��o atual no GameManager.
    /// </summary>
    void AdjustPlayModeButtonText()
    {
        switch (GameManager.instance.playMode)
        {
            case GameManager.PlayMode.PlayerVsPlayer:
                playModeButtonText.text = "2 Players";
                break;
            case GameManager.PlayMode.PlayerVsAi:
                playModeButtonText.text = "Player vs AI";
                break;
        }
    }
}