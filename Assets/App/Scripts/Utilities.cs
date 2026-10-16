using UnityEngine;


public class Utilities
{
    public static float VelocityToAngle(Vector2 velocity)
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        return Mathf.Repeat(angle, 360f);
    }

    public static Vector2 AngleToVelocity(float angle, float speed)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        return velocity * speed;
    }
}
