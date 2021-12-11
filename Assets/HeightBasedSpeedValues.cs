using StarterAssets;
using UnityEngine;

public class HeightBasedSpeedValues : MonoBehaviour
{
    public float height;

    [Space(10)]
    public float moveSpeed;
    public float sprintSpeed;
	[Range(0.0f, 0.3f)] public float rotationSmoothTime;
    public float speedChangeRate;

    [Space(10)]
    public float jumpHeight = 1.2f;
    public float gravity = -15.0f;

    [Space(10)]
    public float jumpTimeout = 0.50f;
    public float fallTimeout = 0.15f;

    private ThirdPersonController controller;

    private float originalMoveSpeed;
    private float originalSprintSpeed;
    private float originalRotationSmoothTime;
    private float originalSpeedChangeRate;

    private float originalJumpHeight;
    private float originalGravity;

    private float originalJumpTimeout;
    private float originalFallTimeout;

    private void Start()
    {
        controller = GetComponent<ThirdPersonController>();

        originalMoveSpeed = controller.MoveSpeed;
        originalSprintSpeed = controller.SprintSpeed;
        originalRotationSmoothTime = controller.RotationSmoothTime;
        originalSpeedChangeRate = controller.SpeedChangeRate;

        originalJumpHeight = controller.JumpHeight;
        originalGravity = controller.Gravity;

        originalJumpTimeout = controller.JumpTimeout;
        originalFallTimeout = controller.FallTimeout;
    }

    private void Update()
    {
        bool low = transform.position.y < height;

        controller.MoveSpeed = low ? moveSpeed : originalMoveSpeed;
        controller.SprintSpeed = low ? sprintSpeed : originalSprintSpeed;
        controller.RotationSmoothTime = low ? rotationSmoothTime : originalRotationSmoothTime;
        controller.SpeedChangeRate = low ? speedChangeRate : originalSpeedChangeRate;

        controller.JumpHeight = low ? jumpHeight : originalJumpHeight;
        controller.Gravity = low ? gravity : originalGravity;

        controller.JumpTimeout = low ? jumpTimeout : originalJumpTimeout;
        controller.FallTimeout = low ? fallTimeout : originalFallTimeout;
    }
}
