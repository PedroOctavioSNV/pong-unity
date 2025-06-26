using UnityEngine;

/// <summary>
/// Controla o comportamento da bola no jogo:
/// movimentação, colisões com raquetes e paredes,
/// pontuação, partículas e efeitos sonoros.
/// </summary>
public class Ball : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] new Rigidbody2D rigidbody2D;         // Rigidbody2D responsável pela física da bola
    [SerializeField] BallAudio ballAudio;                 // Referência ao script de áudio da bola
    [SerializeField] ParticleSystem collisionParticle;    // Sistema de partículas para colisões

    [Header("Configs")]
    [SerializeField, Range(0f, 1f)]
    float maxInitialAngle = 0.67f;                        // Máximo ângulo vertical inicial ao lançar a bola
    [SerializeField, Tooltip("The maximum ball angle after colliding with a paddle")]
    float maxCollisionAngle = 45f;                        // Ângulo máximo aplicado após colisão com raquete
    [SerializeField] float moveSpeed = 1.0f;              // Velocidade base da bola
    [SerializeField] float speedMultiplier = 1.1f;        // Fator multiplicador de velocidade após rebote
    float startX = 0f;                                    // Posição X inicial da bola
    float maxStartY = 4f;                                 // Faixa de variação vertical da posição inicial

    void Start()
    {
        // Inscreve métodos no evento de reset do jogo e início da partida
        GameManager.instance.onReset += ResetBall;
        GameManager.instance.gameUI.onStartGame += ResetBall;
    }

    /// <summary>
    /// Reinicia a posição da bola e realiza o lançamento inicial.
    /// </summary>
    void ResetBall()
    {
        ResetBallPosition();
        InitialPush();
    }

    /// <summary>
    /// Define uma nova posição aleatória para a bola dentro da faixa vertical.
    /// </summary>
    void ResetBallPosition()
    {
        float positionY = Random.Range(-maxStartY, maxStartY);
        Vector2 position = new Vector2(startX, positionY);
        transform.position = position;
    }

    /// <summary>
    /// Aplica uma força inicial aleatória na bola (esquerda ou direita),
    /// com uma leve variação vertical controlada.
    /// </summary>
    void InitialPush()
    {
        Vector2 direction = Random.value < 0.5f ? Vector2.left : Vector2.right;
        direction.y = Random.Range(-maxInitialAngle, maxInitialAngle);
        rigidbody2D.linearVelocity = direction * moveSpeed;
        EmitParticle(10);
    }

    /// <summary>
    /// Detecta entrada da bola em zonas de pontuação.
    /// Atualiza o placar e aciona efeitos visuais.
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreZone scoreZone = collision.GetComponent<ScoreZone>();
        if (scoreZone)
        {
            GameManager.instance.OnScoreZoneReached(scoreZone.id);
            GameManager.instance.screenshake.StartShake(0.33f, 0.1f);
        }
    }

    /// <summary>
    /// Trata colisões com raquetes ou paredes.
    /// Aplica efeitos sonoros, visuais e ajuste de trajetória.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        Paddle paddle = collision.collider.GetComponent<Paddle>();

        if (paddle)
        {
            ballAudio.PlayPaddleSound();                     // Som de colisão com raquete
            rigidbody2D.linearVelocity *= speedMultiplier;   // Aumenta a velocidade após rebote
            EmitParticle(5);                                 // Efeito visual de colisão
            AdjustAngle(paddle, collision);                  // Corrige o ângulo de saída da bola
            GameManager.instance.screenshake.StartShake(0.1f, 0.05f);
        }

        Wall wall = collision.collider.GetComponent<Wall>();

        if (wall)
        {
            ballAudio.PlayWallSound();                       // Som de colisão com parede
            EmitParticle(2);                                 // Partículas de impacto
            GameManager.instance.screenshake.StartShake(0.033f, 0.033f);
        }
    }

    /// <summary>
    /// Ajusta o ângulo de saída da bola com base na posição de impacto na raquete.
    /// Simula um efeito de controle sobre a bola (curva/ângulo).
    /// </summary>
    void AdjustAngle(Paddle paddle, Collision2D collision)
    {
        Vector2 median = Vector2.zero;

        // Calcula o ponto médio entre os contatos da colisão
        foreach (ContactPoint2D point in collision.contacts)
        {
            median += point.point;
            // Debug: desenhar ponto de contato
            // Debug.DrawRay(point.point, Vector3.right, UnityEngine.Color.red, 1f);
        }

        median /= collision.contactCount;

        // Calcula a distância relativa entre o centro da raquete e o ponto de impacto
        float absoluteDistanceFromCenter = median.y - paddle.transform.position.y;
        float relativeDistanceFromCenter = absoluteDistanceFromCenter * 2 / paddle.GetHeight();

        // Determina a direção do ângulo com base no lado da raquete
        int angleSign = paddle.IsLeftPaddle() ? 1 : -1;
        float angle = relativeDistanceFromCenter * maxCollisionAngle * angleSign;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Aplica a nova direção com o mesmo módulo de velocidade
        Vector2 direction = paddle.IsLeftPaddle() ? Vector2.right : Vector2.left;
        Vector2 velocity = rotation * direction * rigidbody2D.linearVelocity.magnitude;
        rigidbody2D.linearVelocity = velocity;

        // Debug: visualizar vetores e colisão
        // Debug.DrawRay(median, velocity, UnityEngine.Color.green, 1f);
    }

    /// <summary>
    /// Emite uma quantidade de partículas para reforçar o feedback visual.
    /// </summary>
    /// <param name="amount">Quantidade de partículas a emitir</param>
    void EmitParticle(int amount)
    {
        collisionParticle.Emit(amount);
    }
}