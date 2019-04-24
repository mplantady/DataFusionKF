using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that act as a Sensor on an object to process Velocity
/// </summary>
public class InertialMeasurementUnit : SensorUpdate
{
    [Header("Sensor Settings")]
    [SerializeField]
    private float _noise = 0.01f;

    private Vector3 _lastPosition;
    private Vector3 _lastVelocity;

    public Vector3 MeasuredVelocity { get; private set; }
    public Vector3 MeasuredAcceleration { get; private set; }
    public float Noise { get => _noise; }

    protected void Start()
    {
        _lastPosition = transform.localPosition;
    }

    /// <summary>
    /// Simulate linear velocity captured from position with noise and latency
    /// </summary>
    public override void UpdateSensor()
    {
        var realVelocity = (transform.localPosition - _lastPosition) / Simulation.DeltaTime;
        var realAcceleration = (realVelocity - _lastVelocity) / Simulation.DeltaTime;

        MeasuredVelocity = realVelocity + Random.insideUnitSphere * _noise;
        MeasuredAcceleration = realAcceleration + Random.insideUnitSphere * _noise;

        _lastPosition = transform.localPosition;
        _lastVelocity = realAcceleration;
    }
}
