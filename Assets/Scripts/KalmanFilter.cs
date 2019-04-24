using LightweightMatrixCSharp;

/// <summary>
/// Kalman filter implementation
/// </summary>
public class KalmanFilter
{
    public Matrix P { get; private set; }
    public Matrix X { get; private set; }

    public KalmanFilter(Matrix initialState, Matrix initialCovariance)
    {
        X = initialState;
        P = initialCovariance;
    }

    public void Predict(Matrix F, Matrix Q)
    {
        // Update state estimate using previous state
        X  = F * X;

        // Update state covariance Matrix (pre)
        P = (F * P * Matrix.Transpose(F)) + Q;
    }

    public void Update(Matrix z, Matrix H, Matrix R)
    {
        // Calculate Kalman Gain
        var K = P * Matrix.Transpose(H) * ((H * P * Matrix.Transpose(H)) + R).Invert();

        // Update state estimate
        X = X + (K * (z - (H * X)));

        // Update state covariance Matrix (post)
        var I = Matrix.IdentityMatrix(X.rows, X.rows);
        P = (I - K * H) * P;
    }
}
