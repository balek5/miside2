using UnityEngine;

public class PongGame : MonoBehaviour
{
    public RectTransform playerPaddle;  // Reference to the player paddle
    public RectTransform aiPaddle;      // Reference to the AI paddle
    public RectTransform ball;         // Reference to the ball

    public float paddleSpeed = 300f;    // Speed of the paddles
    public float ballSpeed = 300f;      // Speed of the ball
    public float aiSpeed = 300f;        // Speed of the AI paddle

    private Vector2 ballDirection = Vector2.right; // Initial direction of the ball

    void Start()
    {
        // Initialize ball's speed and direction
        ballDirection = new Vector2(1, Random.Range(-1f, 1f)).normalized; // Randomize the Y direction of the ball
    }

    void Update()
    {
        // Player paddle movement
        float move = Input.GetAxis("Vertical") * paddleSpeed * Time.deltaTime;
        playerPaddle.anchoredPosition += new Vector2(0, move);

        // Ball movement
        ball.anchoredPosition += ballDirection * ballSpeed * Time.deltaTime;

        // Ball collision with top and bottom walls
        if (ball.anchoredPosition.y >= 300 || ball.anchoredPosition.y <= -300)
        {
            ballDirection.y = -ballDirection.y;
        }

        // AI paddle movement (following the ball)
        float aiMovement = Mathf.MoveTowards(aiPaddle.anchoredPosition.y, ball.anchoredPosition.y, aiSpeed * Time.deltaTime);
        aiPaddle.anchoredPosition = new Vector2(aiPaddle.anchoredPosition.x, aiMovement);
    }

    // This method is called when the ball collides with any object that has a Collider2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ball collision with paddles
        if (collision.gameObject.CompareTag("PlayerPaddle") || collision.gameObject.CompareTag("AIPaddle"))
        {
            // Reverse the ball's direction on collision with paddles
            ballDirection.x = -ballDirection.x;
        }
    }
}