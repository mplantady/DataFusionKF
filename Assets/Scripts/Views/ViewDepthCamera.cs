using UnityEngine;
using static DebugDraw;

/// <summary>
/// View of the depth camera and how the tracked object position is predicted
/// </summary>
public class ViewDepthCamera : ViewUpdate
{
    [SerializeField]
    private DepthSensingCamera _depthCamera;

    [SerializeField]
    private TrailRenderer _trailPrefab;

    private TrailRenderer _debugTrail;

    private StatTracker _trackerNormal = new StatTracker();
    private StatTracker _trackerDropOut = new StatTracker();

    /// <summary>
    /// Update debug visual and stats
    /// </summary>
    public override void UpdateView()
    {
        transform.position = _depthCamera.MeasuredPosition;

        if (_depthCamera.MeasureAvailable)
        {
            if (_debugTrail == null)
            {
                _debugTrail = Instantiate(_trailPrefab, transform);
            }
            _trackerNormal.AddValue(_depthCamera.TrackedTarget.position, _depthCamera.MeasuredPosition);
        }
        else
        {
            if (_debugTrail != null)
            {
                _debugTrail.transform.parent = null;
                _debugTrail = null;
            }
            _trackerDropOut.AddValue(_depthCamera.TrackedTarget.position, _depthCamera.MeasuredPosition);
        }
    }

    void OnGUI()
    {
        DebugDraw.GUILabel("Motion tracking with Depth Camera", GUIPosition.TopRight);
        GUILabel("Average precision (cm)", GUIPosition.TopRight, 1);
        GUILabel("• Normal " + (100f * _trackerNormal.GetMeanPrecision()).ToString("0.00"), GUIPosition.TopRight, 2);
        GUILabel("• Drop-out " + (100f * _trackerDropOut.GetMeanPrecision()).ToString("0.00"), GUIPosition.TopRight, 3);
    }
}
