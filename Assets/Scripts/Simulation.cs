using System.Collections.Generic;
using UnityEngine;
using static DebugDraw;

/// <summary>
/// Main class that update every objects of the simulation in a given order at a fixed rate
/// </summary>
public class Simulation : MonoBehaviour
{
    public const float RefreshRate = 30f;
    public const float DeltaTime = 1f / RefreshRate;

    [SerializeField]
    private List<HandheldMovement> _movements;

    [SerializeField]
    private List<SensorUpdate> _sensors;

    [SerializeField]
    private List<ViewUpdate> _views;

    void Awake()
    {
        Random.InitState(42); // Fixed seed used to compare different approaches under the same conditions

        // Fixed update is set to 30Hz
        Time.fixedDeltaTime = DeltaTime;
    }

    public void FixedUpdate () 
    {
        foreach (var o in _movements)
        {
            o.UpdateStep();
        }

        foreach (var o in _sensors)
        {
            o.UpdateSensor();
        }

        foreach (var o in _views)
        {
            o.UpdateView();
        }
    }

    void OnGUI()
    {
        GUILabel("World Simulation", GUIPosition.TopLeft);
        GUILabel("All", GUIPosition.BottomLeft);
    }
}
