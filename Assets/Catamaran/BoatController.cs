using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody rigidbody => _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>());

    public float speed;
    public float breakSpeed;
    public Vector2 yzTurnSpeed;

    private Vector3 force;
    private Vector3 torque;

    private void Update()
    {
        rigidbody.WakeUp();

        var velocityDir = rigidbody.velocity;
        velocityDir.y = 0f;

        var targetDir = transform.forward;
        targetDir.y = 0f;

        var angle = Vector3.SignedAngle(velocityDir, targetDir, Vector3.up);
        rigidbody.AddRelativeForce(Vector3.right * angle / 90f * 15f);

        // Calculate Torque
        torque = Vector3.zero;

        if (Keyboard.current.aKey.isPressed)
        {
            torque += Vector3.forward;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            torque -= Vector3.forward;
        }

        // Calculate Force
        if (Keyboard.current.wKey.isPressed)
        {
            force = Vector3.forward * speed * Mathf.Lerp(1f, 0.7f, torque.z);
            rigidbody.AddRelativeForce(force);
        }
        else
        {
            force = Vector3.zero;
            torque = Vector3.zero;
        }

        // Apply drag
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            rigidbody.drag *= breakSpeed;
            rigidbody.angularDrag *= breakSpeed;
        }

        if (Keyboard.current.sKey.wasReleasedThisFrame)
        {
            rigidbody.drag /= breakSpeed;
            rigidbody.angularDrag /= breakSpeed;
        }
    }

    void FixedUpdate()
    {
        rigidbody.AddRelativeTorque(torque * yzTurnSpeed.y);
        rigidbody.AddTorque(Vector3.up * torque.z * -yzTurnSpeed.x * Mathf.Lerp(0.7f, 0.9f, rigidbody.velocity.magnitude * 0.8f / speed));
    }
}
