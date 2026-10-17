using UnityEngine;

public class RightPaddle : MonoBehaviour
{
    private Rigidbody2D rb2D;

    private int currentMoveDirection = 0;


    private void OnEnable()
    {
        rb2D = GetComponent<Rigidbody2D>();

        ConnectEvents();
    }

    private void OnDisable()
    {
        DisconnectEvents();
    }


    private void FixedUpdate()
    {
        if (!GameData.IsPlaying)
            return;

        MovePaddle();
    }


    private void LateUpdate()
    {
        if (!GameData.IsPlaying)
            return;

        transform.localPosition = new Vector3(transform.localPosition.x,
            Mathf.Clamp(transform.localPosition.y, -GameData.MaxPaddleYPosition, GameData.MaxPaddleYPosition)
            , 0f);
    }


    public void ResetPaddle()
    {
        rb2D.linearVelocity = Vector2.zero;
        transform.localPosition = new Vector3(transform.localPosition.x, 0f, 0f);
    }


    private void MovePaddle()
    {
        if (currentMoveDirection == 0)
        {
            rb2D.linearVelocity = Vector2.zero;
        }
        else if (currentMoveDirection == 1)
        {
            rb2D.linearVelocity = Vector2.up * GameData.PaddleMoveSpeed;
        }
        else if (currentMoveDirection == -1)
        {
            rb2D.linearVelocity = Vector2.down * GameData.PaddleMoveSpeed;
        }
    }


    #region EventsManager

    private void ConnectEvents()
    {
        EventsManager.MoveUpButtonSelected_Event += OnMoveUP;
        EventsManager.MoveDownButtonSelected_Event += OnMoveDown;
        EventsManager.MoveButtonReleased_Event += OnStopMoving;
    }

    private void DisconnectEvents()
    {
        EventsManager.MoveUpButtonSelected_Event -= OnMoveUP;
        EventsManager.MoveDownButtonSelected_Event -= OnMoveDown;
        EventsManager.MoveButtonReleased_Event -= OnStopMoving;
    }


    private void OnMoveUP()
    {
        currentMoveDirection = 1;
    }

    private void OnMoveDown()
    {
        currentMoveDirection = -1;
    }

    private void OnStopMoving()
    {
        currentMoveDirection = 0;
    }

    #endregion
}
