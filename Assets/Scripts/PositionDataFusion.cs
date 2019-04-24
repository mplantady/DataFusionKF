using LightweightMatrixCSharp;
using UnityEngine;

/// <summary>
/// Data fusion and filtering of noisy position and velocity
/// </summary>
public class PositionDataFusion
{
    private const int StateSize = 9;
    private const int MeasureSize = 6;

    private KalmanFilter _kalmanFilter;
    private float _processCovariance;

    private Matrix _transitionMat;          // F
    private Matrix _measurementMat;         // H
    private Matrix _processNoiseCovMat;     // Q
    private Matrix _measurementNoiseCovMat; // R

    /// <summary>
    /// Initialize Kalman Filtering objects and matrix
    /// </summary>
    public PositionDataFusion(float deltaTime, Vector3 initialPosition, float processCovariance, float positionDeviation, float velocityDeviation)
    {
        _processCovariance = processCovariance;

        float a = 0.5f * Mathf.Pow(deltaTime, 2);
        _transitionMat = new Matrix(new double[,] { { 1, 0, 0, deltaTime, 0, 0, a, 0, 0 },
                                                    { 0, 1, 0, 0, deltaTime, 0, 0, a, 0 },
                                                    { 0, 0, 1, 0, 0, deltaTime, 0, 0, a },
                                                    { 0, 0, 0, 1, 0, 0, deltaTime, 0, 0 },
                                                    { 0, 0, 0, 0, 1, 0, 0, deltaTime, 0 },
                                                    { 0, 0, 0, 0, 0, 1, 0, 0, deltaTime },
                                                    { 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                                                  });

        _measurementMat = Matrix.IdentityMatrix(MeasureSize, StateSize);
        _processNoiseCovMat = Matrix.IdentityMatrix(StateSize, StateSize, _processCovariance);

        float cP = Mathf.Pow(positionDeviation, 2);
        float cV = Mathf.Pow(velocityDeviation, 2);
        _measurementNoiseCovMat = new Matrix(new double[,] { { cP, 0, 0, 0, 0, 0},
                                                             { 0, cP, 0, 0, 0, 0},
                                                             { 0, 0, cP, 0, 0, 0},
                                                             { 0, 0, 0, cV, 0, 0},
                                                             { 0, 0, 0, 0, cV, 0},
                                                             { 0, 0, 0, 0, 0, cV}
                                                           });

        var initialState = new Matrix(new double[,] { { initialPosition.x, initialPosition.y, initialPosition.z, 0, 0, 0, 0, 0, 0 } }, false);
        _kalmanFilter = new KalmanFilter(initialState, _processNoiseCovMat);
    }

    /// <summary>
    /// Update data estimation with new measurement
    /// </summary>
    public void Update(Vector3? measurePosition, Vector3? measureVelocity)
    {
        // Predict phase Kalman Filter
        _kalmanFilter.Predict(_transitionMat, _processNoiseCovMat);

        var newPosition = measurePosition ?? GetPositionFromFilter();
        var newVelocity = measureVelocity ?? GetVelocityFromFilter();

        var measure = new Matrix(new double[,] { { newPosition.x,
                                                   newPosition.y,
                                                   newPosition.z,
                                                   newVelocity.x,
                                                   newVelocity.y,
                                                   newVelocity.z
                                                   } }, false);

        // Update Kalman Filter with new measures
        _kalmanFilter.Update(measure, _measurementMat, _measurementNoiseCovMat);
    }

    public Vector3 GetPositionFromFilter()
    {
        return new Vector3((float)_kalmanFilter.X[0, 0], (float)_kalmanFilter.X[1, 0], (float)_kalmanFilter.X[2, 0]);
    }

    public Vector3 GetVelocityFromFilter()
    {
        return new Vector3((float)_kalmanFilter.X[3, 0], (float)_kalmanFilter.X[4, 0], (float)_kalmanFilter.X[5, 0]);
    }

    public Vector3 GetAccelerationFromFilter()
    {
        return new Vector3((float)_kalmanFilter.X[6, 0], (float)_kalmanFilter.X[7, 0], (float)_kalmanFilter.X[8, 0]);
    }
}