using UnityEngine;

/// <summary>
/// Simulate a depth camera software that extract sporadically and noisy 3D position of a target object
/// </summary>
public class DepthSensingCamera : SensorUpdate
{
    private const float CalibrationTime = 10f; // Time without drop-out

    [SerializeField]
    private Transform _trackedTarget;

    [Header("Sensor settings")]
    [SerializeField]
    private float _noise = 0.05f;

    [SerializeField]
    private float _dropoutProbability = 0.01f;

    [SerializeField]
    private float _dropoutDuration = 1f;

    private float _dropOutEnd;

    public Vector3 MeasuredPosition { get; private set; }
    public bool MeasureAvailable { get; private set; } = true;
    public float Noise { get => _noise; }

    public Transform TrackedTarget { get => _trackedTarget; }

    public override void UpdateSensor()
    {
        if (_trackedTarget != null)
        {
            transform.LookAt(_trackedTarget);
        }

        if (Time.realtimeSinceStartup < CalibrationTime || Time.realtimeSinceStartup > _dropOutEnd)
        {
            // Simulate a noisy position extraction
            MeasuredPosition = _trackedTarget.position + Random.insideUnitSphere * _noise;
            MeasureAvailable = true;
        }
        else
        {
            MeasureAvailable = false;
        }

        if (Random.Range(0, 1f) < _dropoutProbability)
        {
            _dropOutEnd = Time.realtimeSinceStartup + _dropoutDuration;
        }
    }
}
