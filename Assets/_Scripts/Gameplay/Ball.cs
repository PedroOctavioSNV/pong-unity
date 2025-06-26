using UnityEngine;

/// <summary>
/// Controla o comportamento da bola no jogo:
/// movimenta��o, colis�es com raquetes e paredes,
/// pontua��o, part�culas e efeitos sonoros.
/// </summary>
public class Ball : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] new Rigidbody2D rigidbody2D;         // Rigidbody2D respons�vel pela f�sica da bola
    [SerializeField] BallAudio ballAudio;                 // Refer�ncia ao script de �udio da bola
    [SerializeField] ParticleSystem collisionParticle;    // Sistema de part�culas para colis�es

    [Header("Configs")]
    [SerializeField, Range(0f, 1f)]
    float maxInitialAngle = 0.67f;                        // M�ximo �ngulo vertical inicial ao lan�ar a bola
    [SerializeField, Tooltip("The maximum ball angle after colliding with a paddle")]
    float maxCollisionAngle = 45f;                        // �ngulo m�ximo aplicado ap�s colis�o com raquete
    [SerializeField] float moveSpeed = 1.0f;              // Velocidade base da bola
    [SerializeField] float speedMultiplier = 1.1f;        // Fator multiplicador de velocidade ap�s rebote
    float startX = 0f;                                    // Posi��o X inicial da bola
    float maxStartY = 4f;                                 // Faixa de varia��o vertical da posi��o inicial

    void Start()
    {
        // Inscreve m�todos no evento de reset do jogo e in�cio da partida
        GameManager.instance.onReset += ResetBall;
        GameManager.instance.gameUI.onStartGame += ResetBall;
    }

    /// <summary>
    /// Reinicia a posi��o da bola e realiza o lan�amento inicial.
    /// </summary>
    void ResetBall()
    {
        ResetBallPosition();
        InitialPush();
    }

    /// <summary>
    /// Define uma nova posi��o aleat�ria para a bola dentro da faixa vertical.
    /// </summary>
    void ResetBallPosition()
    {
        float positionY = Random.Range(-maxStartY, maxStartY);
        Vector2 position = new Vector2(startX, positionY);
        transform.position = position;
    }

    /// <summary>
    /// Aplica uma for�a inicial aleat�ria na bola (esquerda ou direita),
    /// com uma leve varia��o vertical controlada.
    /// </summary>
    void InitialPush()
    {
        Vector2 direction = Random.value < 0.5f ? Vector2.left : Vector2.right;
        direction.y = Random.Range(-maxInitialAngle, maxInitialAngle);
        rigidbody2D.linearVelocity = direction * moveSpeed;
        EmitParticle(10);
    }

    /// <summary>
    /// Detecta entrada da bola em zonas de pontua��o.
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
    /// Trata colis�es com raquetes ou paredes.
    /// Aplica efeitos sonoros, visuais e ajuste de trajet�ria.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        Paddle paddle = collision.collider.GetComponent<Paddle>();

        if (paddle)
        {
            ballAudio.PlayPaddleSound();                     // Som de colis�o com raquete
            rigidbody2D.linearVelocity *= speedMultiplier;   // Aumenta a velocidade ap�s rebote
            EmitParticle(5);                                 // Efeito visual de colis�o
            AdjustAngle(paddle, collision);                  // Corrige o �ngulo de sa�da da bola
            GameManager.instance.screenshake.StartShake(0.1f, 0.05f);
        }

        Wall wall = collision.collider.GetComponent<Wall>();

        if (wall)
        {
            ballAudio.PlayWallSound();                       // Som de colis�o com parede
            EmitParticle(2);                                 // Part�culas de impacto
            GameManager.instance.screenshake.StartShake(0.033f, 0.033f);
        }
    }

    /// <summary>
    /// Ajusta o �ngulo de sa�da da bola com base na posi��o de impacto na raquete.
    /// Simula um efeito de controle sobre a bola (curva/�ngulo).
    /// </summary>
    void AdjustAngle(Paddle paddle, Collision2D collision)
    {
        Vector2 median = Vector2.zero;

        // Calcula o ponto m�dio entre os contatos da colis�o
        foreach (ContactPoint2D point in collision.contacts)
        {
            median += point.point;
            // Debug: desenhar ponto de contato
            // Debug.DrawRay(point.point, Vector3.right, UnityEngine.Color.red, 1f);
        }

        median /= collision.contactCount;

        // Calcula a dist�ncia relativa entre o centro da raquete e o ponto de impacto
        float absoluteDistanceFromCenter = median.y - paddle.transform.position.y;
        float relativeDistanceFromCenter = absoluteDistanceFromCenter * 2 / paddle.GetHeight();

        // Determina a dire��o do �ngulo com base no lado da raquete
        int angleSign = paddle.IsLeftPaddle() ? 1 : -1;
        float angle = relativeDistanceFromCenter * maxCollisionAngle * angleSign;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Aplica a nova dire��o com o mesmo m�dulo de velocidade
        Vector2 direction = paddle.IsLeftPaddle() ? Vector2.right : Vector2.left;
        Vector2 velocity = rotation * direction * rigidbody2D.linearVelocity.magnitude;
        rigidbody2D.linearVelocity = velocity;

        // Debug: visualizar vetores e colis�o
        // Debug.DrawRay(median, velocity, UnityEngine.Color.green, 1f);
    }

    /// <summary>
    /// Emite uma quantidade de part�culas para refor�ar o feedback visual.
    /// </summary>
    /// <param name="amount">Quantidade de part�culas a emitir</param>
    void EmitParticle(int amount)
    {
        collisionParticle.Emit(amount);
    }
}