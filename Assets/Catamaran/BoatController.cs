using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody rigidbody => _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>());

    public float speed;
    public float backwardsSpeed;
    public Vector2 yzTurnSpeed;

    private Vector3 force;
    private Vector3 torque;

    public Transform playerSpawnPoint;

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
        force = Vector3.zero;
        if (Keyboard.current.wKey.isPressed)
        {
            force += Vector3.forward * speed * Mathf.Lerp(1f, 0.9f, torque.z);
            //rigidbody.AddRelativeForce(force);
        }

        if (Keyboard.current.sKey.isPressed)
        {
            force += Vector3.back * backwardsSpeed * Mathf.Lerp(1f, 0.9f, torque.z);
            //rigidbody.AddRelativeForce(force);
        }

        // Apply drag
        //if (Keyboard.current.sKey.wasPressedThisFrame)
        //{
        //    rigidbody.drag *= backwardsSpeed;
        //    rigidbody.angularDrag *= backwardsSpeed;
        //}

        //if (Keyboard.current.sKey.wasReleasedThisFrame)
        //{
        //    rigidbody.drag /= backwardsSpeed;
        //    rigidbody.angularDrag /= backwardsSpeed;
        //}

        // Disembark
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            var forward = transform.forward;
            forward.y = 0f;

            var position = transform.position;
            var rotation = Quaternion.LookRotation(forward.normalized, Vector3.up);

            var newGameObject = Instantiate(gameObject, position, rotation);
            newGameObject.name = gameObject.name;

            var newRigidbody = newGameObject.GetComponent<Rigidbody>();
            newRigidbody.WakeUp();
            newRigidbody.velocity = rigidbody.velocity;
            newRigidbody.angularVelocity = rigidbody.angularVelocity;
            
            var newBoatController = newGameObject.GetComponent<BoatController>();

            gameObject.name += " Old";

            GameObject.Find("CatamaranFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = newGameObject.GetComponent<ThirdPersonCinemachineController>().CinemachineCameraTarget.transform;

            Interact.TogglePlayerAndBoat(true);

            DestroyImmediate(gameObject);

            newBoatController.StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                var player = GameObject.Find("PlayerArmature");

                int i = 0;
                do
                {
                    if (i > 0)
                    {
                        Debug.Log($"Fail {i}");
                    }
                    i++;

                    player.transform.SetPositionAndRotation(newBoatController.playerSpawnPoint.position, newBoatController.playerSpawnPoint.rotation);

                    yield return null;
                    yield return null;
                }
                while ((player.transform.position - newBoatController.playerSpawnPoint.position).sqrMagnitude > 1f);

                Debug.Log("Success");
            }
        }
    }

    void FixedUpdate()
    {
        rigidbody.AddRelativeForce(force);
        rigidbody.AddRelativeTorque(torque * yzTurnSpeed.y);
        rigidbody.AddTorque(Vector3.up * torque.z * -yzTurnSpeed.x * Mathf.Lerp(0.7f, 0.9f, rigidbody.velocity.magnitude * 0.8f / speed));
    }
}
