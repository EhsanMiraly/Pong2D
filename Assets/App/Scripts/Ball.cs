using UnityEngine;

public class Ball : MonoBehaviour
{
    //[SerializeField] private PaddleAgent paddleAgent;
    [SerializeField] private AudioClip ballHitAudioClip;
    [SerializeField] private AudioClip ballHitWallAudioClip;

    private AudioSource audioSource;

    private Rigidbody2D rb2D;

    private float currentSpeed = 0f;



    private void OnEnable()
    {
        transform.localPosition = Vector2.zero;
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!GameData.IsPlaying)
            return;

        rb2D.linearVelocity = rb2D.linearVelocity.normalized * currentSpeed;
    }


    public void StartMoving()
    {
        currentSpeed = GameData.MinBallSpeed;

        Vector2 initialDirection = Random.Range(-1f, 1f) < 0f ? Vector2.left : Vector2.right;//Vector2.right;
        initialDirection.y = Random.Range(-1f, 1f);
        initialDirection = initialDirection.normalized;

        rb2D.linearVelocity = initialDirection * currentSpeed;
    }

    public void StopMoving()
    {
        currentSpeed = 0f;
        rb2D.linearVelocity = Vector2.zero;
        transform.localPosition = Vector2.zero;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameData.IsPlaying)
            return;

        if (collision.gameObject.tag == "Paddle")
        {
            PlayBallHitObject();

            currentSpeed = Mathf.Clamp(currentSpeed + GameData.BallSpeedIncrease, GameData.MinBallSpeed, GameData.MaxBallSpeed);

            ApplyPaddleVelocityAngle(collision.gameObject);
        }

        else if (collision.gameObject.tag == "LeftWall")
        {
            PlayBallHitEndWall();

            GameData.YouScore++;
        }
        else if (collision.gameObject.tag == "RightWall")
        {
            PlayBallHitEndWall();

            GameData.AIScore++;
        }
        else
        {
            PlayBallHitObject();
        }

        FixAngle();
    }

    private void ApplyPaddleVelocityAngle(GameObject paddle)
    {
        Rigidbody2D paddleRb = paddle.GetComponent<Rigidbody2D>();

        if (paddleRb == null)
            return;

        float currentAngle = Utilities.VelocityToAngle(rb2D.linearVelocity);

        float randomAngle;

        if (Mathf.Abs(paddleRb.linearVelocity.y) < 0.01f)
        {
            randomAngle = Random.Range(-10f, 10f);
        }
        else
        {
            randomAngle = Random.Range(-30f, 30f);
        }

        float newAngle = currentAngle + randomAngle;

        Vector2 newVelocity = Utilities.AngleToVelocity(newAngle, currentSpeed);

        rb2D.linearVelocity = newVelocity;
    }


    private void FixAngle()
    {
        if (rb2D.linearVelocity == Vector2.zero)
            return;

        float angle = Utilities.VelocityToAngle(rb2D.linearVelocity);

        // Vertical direction
        if (angle >= 50f && angle <= 90f)
            angle = 50f;
        else if (angle > 90f && angle <= 130f)
            angle = 130f;

        else if (angle >= 230f && angle < 270f)
            angle = 230f;
        else if (angle >= 270f && angle <= 310f)
            angle = 310f;

        // Horizontal direction
        else if (angle >= 0f && angle <= 1f)
            angle = 1f;
        else if (angle >= 359f && angle <= 360f)
            angle = 359f;

        else if (angle >= 179f && angle <= 180f)
            angle = 179f;
        else if (angle > 180f && angle <= 181f)
            angle = 181f;

        rb2D.linearVelocity = Utilities.AngleToVelocity(angle, currentSpeed);
    }


    private void PlayBallHitObject()
    {
        if (!SettingsData.isSoundEffectsOn)
            return;

        audioSource.clip = ballHitAudioClip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    private void PlayBallHitEndWall()
    {
        if (!SettingsData.isSoundEffectsOn)
            return;

        audioSource.clip = ballHitWallAudioClip;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }

}
