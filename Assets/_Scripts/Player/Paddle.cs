using UnityEngine;

/// <summary>
/// Controla o comportamento das raquetes (paddles) dos jogadores.
/// Suporta controle manual via input e movimento controlado por IA para o jogador 2.
/// </summary>
public class Paddle : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] new Rigidbody2D rigidbody2D;            // Rigidbody2D para manipulação da física da raquete

    [Header("Configs")]
    [SerializeField] int id;                                 // Identificador da raquete (1 = esquerda, 2 = direita)
    [SerializeField] float moveSpeed = 2f;                   // Velocidade base de movimentação da raquete
    Vector3 startPosition;                                   // Posição inicial para reset da raquete
    float moveSpeedMultiplier = 1f;                          // Multiplicador dinâmico da velocidade (usado pela IA)
    const string MovePlayer1InputName = "MovePlayer1";       // Nome da entrada de movimento do jogador 1 (configurado no Input Manager)
    const string MovePlayer2InputName = "MovePlayer2";       // Nome da entrada de movimento do jogador 2

    [Header("AI")]
    [SerializeField] float aiDeadzone = 1f;                  // Zona morta para o movimento da IA (evita oscilações pequenas)
    [SerializeField] float aiMoveSpeedMultiplierMin = 0.5f;  // Mínimo multiplicador aleatório para velocidade da IA
    [SerializeField] float aiMoveSpeedMultiplierMax = 1.5f;  // Máximo multiplicador aleatório para velocidade da IA
    int direction = 0;                                       // Direção atual do movimento da raquete (-1 para baixo, 1 para cima)

    void Start()
    {
        startPosition = transform.position;
        // Inscreve o método ResetPosition para ser chamado quando o jogo resetar
        GameManager.instance.onReset += ResetPosition;
    }

    void Update()
    {
        // Se esta raquete é a do jogador 2 e o modo é Player vs IA, usa o controle da IA
        if (id == 2 && GameManager.instance.IsPlayer2Ai())
        {
            MoveAi();
        }
        else
        {
            // Caso contrário, lê o input do jogador e movimenta manualmente
            float movement = GetInput();
            Move(movement);
        }
    }

    /// <summary>
    /// Reseta a posição da raquete para sua posição inicial.
    /// Chamado no evento de reset do jogo.
    /// </summary>
    public void ResetPosition()
    {
        transform.position = startPosition;
    }

    /// <summary>
    /// Obtém o valor de entrada do jogador (eixo vertical).
    /// </summary>
    /// <returns>Valor de input entre -1 e 1 para controle do paddle</returns>
    float GetInput()
    {
        return IsLeftPaddle() ? Input.GetAxis(MovePlayer1InputName) : Input.GetAxis(MovePlayer2InputName);
    }

    /// <summary>
    /// Verifica se este paddle é o da esquerda (jogador 1).
    /// </summary>
    /// <returns>True se for o paddle da esquerda, falso caso contrário</returns>
    public bool IsLeftPaddle()
    {
        return id == 1;
    }

    /// <summary>
    /// Aplica o movimento vertical ao Rigidbody da raquete.
    /// </summary>
    /// <param name="movement">Valor de movimento vertical, geralmente de -1 a 1</param>
    void Move(float movement)
    {
        Vector2 velocity = rigidbody2D.linearVelocity;
        velocity.y = moveSpeed * moveSpeedMultiplier * movement;
        rigidbody2D.linearVelocity = velocity;
    }

    /// <summary>
    /// Lógica da IA para movimentar a raquete automaticamente,
    /// buscando acompanhar a bola respeitando a zona morta.
    /// </summary>
    void MoveAi()
    {
        Vector2 ballPosition = GameManager.instance.ball.transform.position;

        // Determina direção para mover com base na posição da bola e deadzone
        if (Mathf.Abs(ballPosition.y - transform.position.y) > aiDeadzone)
        {
            direction = ballPosition.y > transform.position.y ? 1 : -1;
        }

        // Aleatoriamente ajusta o multiplicador de velocidade para variar o comportamento da IA
        if (Random.value < 0.01f)
        {
            moveSpeedMultiplier = Random.Range(aiMoveSpeedMultiplierMin, aiMoveSpeedMultiplierMax);
        }

        Move(direction);
    }

    /// <summary>
    /// Retorna a altura da raquete (escala local no eixo Y).
    /// Útil para cálculos de colisão e controle da bola.
    /// </summary>
    /// <returns>Altura do paddle</returns>
    public float GetHeight()
    {
        return transform.localScale.y;
    }
}