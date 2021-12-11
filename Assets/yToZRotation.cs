using UnityEngine;

[ExecuteAlways]
public class yToZRotation : MonoBehaviour
{
    public Transform yRotationTransform;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, yRotationTransform.eulerAngles.y);
    }
}
