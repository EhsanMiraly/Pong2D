using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.InputSystem;
using System.Threading.Tasks;



public class LeftPaddle : Agent
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject lost;/////////Delete - Change to dialog
    [SerializeField] private GameObject win;/////////Delete - Change to dialog
    [SerializeField] private GameObject ballGameObject;
    private Rigidbody2D ballGameObject_rb2D;

    [SerializeField] private Transform environmentTransform;

    private Rigidbody2D rb2D;
    private float initialX = -9f;


    private bool initialized = false;

    public override void Initialize()
    {
        if (initialized)
            return;

        rb2D = GetComponent<Rigidbody2D>();

        ballGameObject_rb2D = ballGameObject.GetComponent<Rigidbody2D>();

        transform.localPosition = new Vector3(initialX, 0f, 0f);

        initialized = true;
    }


    public void ResetAgent()
    {
        if (!initialized)
        {
            Debug.LogError($"{name}: Initialize() must be called first.");
            return;
        }

        rb2D.linearVelocity = Vector2.zero;
        transform.localPosition = new Vector3(initialX, 0f, 0f);
    }

    public override void OnEpisodeBegin()
    {
        // Intentionally empty.
        //
        // Reset is controlled by MultiAgentEpisodeManager.
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        float paddleY = Mathf.Clamp(transform.localPosition.y / GameData.MaxPaddleYPosition, -1f, 1f);
        sensor.AddObservation(paddleY);

        float ballX = Mathf.Clamp(ballGameObject.transform.localPosition.x / GameData.MaxBallXPosition, -1f, 1f);
        sensor.AddObservation(ballX);

        float ballY = Mathf.Clamp(ballGameObject.transform.localPosition.y / GameData.MaxBallYPosition, -1f, 1f);
        sensor.AddObservation(ballY);

        if (ballGameObject_rb2D.linearVelocity.x > 0)
        {
            sensor.AddObservation(0f);
        }
        else
        {
            float ballAngle = Utilities.VelocityToAngle(ballGameObject_rb2D.linearVelocity);//130-230
            ballAngle = Mathf.Clamp(-((ballAngle - 180f) / 50f), -1f, 1f);
            sensor.AddObservation(ballAngle);
        }

        float ballSpeed = Mathf.Clamp((ballGameObject_rb2D.linearVelocity.magnitude - 15f) / 10f, -1f, 1f);
        sensor.AddObservation(ballSpeed);

        if (ballGameObject_rb2D.linearVelocity.x > 0)
        {
            sensor.AddObservation(0f);
        }
        else
        {
            RaycastHit2D finalRaycastHit2D;
            Vector2 start = ballGameObject.transform.position;
            Vector2 direction = ballGameObject_rb2D.linearVelocity.normalized;

            int x = 0;
            do
            {
                x++;

                finalRaycastHit2D = Physics2D.Raycast(start, direction, float.MaxValue, layerMask);
                if (finalRaycastHit2D.collider != null)
                {
                    start = finalRaycastHit2D.point;
                    if (environmentTransform.InverseTransformPoint(start).y > 0)
                    {
                        start.y -= 0.0001f;
                    }
                    else
                    {
                        start.y += 0.0001f;
                    }

                    if (Utilities.VelocityToAngle(direction) <= 180f)
                    {
                        float a = 180f + (180f - Utilities.VelocityToAngle(direction));
                        direction = Utilities.AngleToVelocity(a, 1f);
                    }
                    else
                    {
                        float b = 180f - (Utilities.VelocityToAngle(direction) - 180f);
                        direction = Utilities.AngleToVelocity(b, 1f);
                    }
                }

                if (x > 10)
                {
                    break;
                }
            } while (finalRaycastHit2D.collider != null && finalRaycastHit2D.collider.gameObject.tag != "LeftWallPaddle" && finalRaycastHit2D.collider.gameObject.tag != "LeftPaddle");

            Vector2 localHitPoint = new Vector2(GameData.MaxBallXPosition, 0f);

            if (finalRaycastHit2D.collider != null)
            {
                localHitPoint = environmentTransform.InverseTransformPoint(finalRaycastHit2D.point);
            }

            float ballFinalY = Mathf.Clamp(localHitPoint.y / GameData.MaxBallYPosition, -1f, 1f);
            sensor.AddObservation(ballFinalY);
        }

    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = 0;

        if (Keyboard.current.upArrowKey.isPressed)
        {
            discreteActions[0] = 1;
        }
        else if (Keyboard.current.downArrowKey.isPressed)
        {
            discreteActions[0] = 2;
        }
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        if (!GameData.IsPlaying)
            return;

        // Discrete: 0 = Nothing / 1 = Up / 2 = Down
        int action = actions.DiscreteActions[0];

        switch (action)
        {
            case 0:
                rb2D.linearVelocity = Vector2.zero;
                break;
            case 1:
                rb2D.linearVelocity = new Vector2(0f, GameData.PaddleMoveSpeed);
                break;
            case 2:
                rb2D.linearVelocity = new Vector2(0f, -GameData.PaddleMoveSpeed);
                break;
            default:
                Debug.LogWarning("Action Out Of Range.");
                break;
        }
    }

    private void FixedUpdate()
    {
        RequestAgentDecision();
    }


    private void LateUpdate()
    {
        if (!GameData.IsPlaying)
            return;

        transform.localPosition = new Vector3(transform.localPosition.x,
            Mathf.Clamp(transform.localPosition.y, -GameData.MaxPaddleYPosition, GameData.MaxPaddleYPosition), 0f);
    }


    public void FinishAgent(float reward, bool winGame)
    {
        if (!GameData.IsPlaying)
            return;

        if (winGame)
        {
            //win.SetActive(true);
        }
        else
        {
            //lost.SetActive(true);
        }

        rb2D.linearVelocity = Vector2.zero;
        transform.localPosition = new Vector3(initialX, 0f, 0f);

        //Reset Ball?????????????????????????????
    }


    public void RequestAgentDecision()
    {
        if (!initialized)
            return;

        if (!GameData.IsPlaying)
            return;

        RequestDecision();
    }

    public void EndAgentEpisode()
    {
        EndEpisode();
    }

}
