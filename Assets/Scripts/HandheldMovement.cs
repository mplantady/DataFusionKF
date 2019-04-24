using UnityEngine;

/// <summary>
/// Component to simulate random movement on an object in a limited area
/// </summary>
public class HandheldMovement : MonoBehaviour
{
    private const float _smoothRatio = 0.2f;
    private const float _testRatio = 0.9f;

    [SerializeField]
    private float _areaUpperLimit = 0.7f;

    [SerializeField]
    private float _areaLowerLimit = 0.6f;

    [SerializeField]
    private float _maxAcceleration = 0.005f;

    private Vector3 _previousMove = Vector3.zero;
    private Vector3 _centerPosition = Vector3.zero;

    private void Start()
    {
        _centerPosition = transform.position;
    }

    public void UpdateStep()
    {
        Vector3 move = _previousMove + Random.insideUnitSphere * _maxAcceleration;
        float distanceToCenter = Vector3.Distance(transform.position + move, _centerPosition);

        if (distanceToCenter < _areaLowerLimit)
        {
            // Move randomly if we are not outside the lower boundary area radius
            transform.position = Vector3.Lerp(transform.position, transform.position + move, _smoothRatio);
        }
        else if (distanceToCenter < _areaUpperLimit)
        {
            // Move toward center if we are on the rim of the boundary area radius
            move = Vector3.Lerp(_previousMove, _previousMove + (_centerPosition - transform.position) * _maxAcceleration, _smoothRatio);
            transform.position = Vector3.Lerp(transform.position, transform.position + move, _smoothRatio);
        }
        else
        {
            move = Vector3.zero;
        }

        _previousMove = move;
    }
}
