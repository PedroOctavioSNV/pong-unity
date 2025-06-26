using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Controla a interface do usuário do jogo (menus, placares, botões, textos).
/// Gerencia interações e atualizações visuais da UI durante o jogo.
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] ScoreText scoreTextPlayer1, scoreTextPlayer2;   // Referências aos textos de pontuação dos jogadores
    [SerializeField] GameObject menuObject;                          // Objeto do menu principal (ativo/inativo)
    [SerializeField] GameObject quitButton;                          // Botão de sair (desabilitado em WebGL)
    [SerializeField] TextMeshProUGUI winText;                        // Texto que exibe o vencedor ou título
    [SerializeField] TextMeshProUGUI volumeValueText;                // Texto que mostra o valor atual do volume
    [SerializeField] TextMeshProUGUI playModeButtonText;             // Texto do botão para alternar modo de jogo

    /// <summary>
    /// Evento disparado quando o jogo é iniciado pelo usuário.
    /// </summary>
    public Action onStartGame;

    void Start()
    {
        // Atualiza o texto do botão de modo de jogo conforme configuração atual
        AdjustPlayModeButtonText();
        // Desativa o botão sair se a plataforma for WebGL (não suportado)
        CheckDisableQuitButton();
    }

    /// <summary>
    /// Método chamado ao clicar no botão "Start Game".
    /// Inicia o jogo escondendo o menu e disparando o evento onStartGame.
    /// </summary>
    public void OnStartGameButtonClicked()
    {
        Time.timeScale = 1;       // Retoma o tempo do jogo
        menuObject.SetActive(false); // Esconde o menu
        onStartGame?.Invoke();    // Dispara evento para iniciar o jogo
    }

    /// <summary>
    /// Atualiza os textos de pontuação dos dois jogadores na UI.
    /// </summary>
    /// <param name="scorePlayer1">Pontuação do jogador 1</param>
    /// <param name="scorePlayer2">Pontuação do jogador 2</param>
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
    /// Pausa o tempo e mostra título padrão.
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
    /// atualizando o texto do botão correspondente.
    /// </summary>
    public void OnSwitchPlayModeButtonClicked()
    {
        GameManager.instance.SwitchPlayMode();
        AdjustPlayModeButtonText();
    }

    /// <summary>
    /// Fecha o jogo ao clicar no botão Quit.
    /// Funciona apenas em builds nativas (não WebGL).
    /// </summary>
    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    /// <summary>
    /// Desabilita o botão Quit caso a plataforma seja WebGL,
    /// pois não é possível sair do navegador via script.
    /// </summary>
    void CheckDisableQuitButton()
    {
#if UNITY_WEBGL
        quitButton.SetActive(false);
#endif
    }

    /// <summary>
    /// Ajusta o texto do botão de modo de jogo de acordo com a configuração atual no GameManager.
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