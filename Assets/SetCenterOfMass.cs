using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SetCenterOfMass : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody rigidbody => _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>());

    public Vector3 offset;

    private void OnEnable()
    {
        rigidbody.centerOfMass = offset;
    }

    private void OnDisable()
    {
        rigidbody.ResetCenterOfMass();
    }
}
