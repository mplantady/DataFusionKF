using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugDraw {

    public enum GUIPosition
    {
        TopLeft,
        BottomLeft,
        TopRight,
        BottomRight,
    }

    private const float PointSize = 0.008f;
    public const float DebugTime = 8f;

    public static void DrawPoint(Vector3 point, Color color)
    {
        Debug.DrawLine(point + Vector3.left * PointSize, point + Vector3.right * PointSize, color, DebugTime);
        Debug.DrawLine(point + Vector3.down * PointSize, point + Vector3.up * PointSize, color, DebugTime);
        Debug.DrawLine(point + Vector3.forward * PointSize, point + Vector3.back * PointSize, color, DebugTime);
    }

    public static void GUILabel(string text, GUIPosition position, int lineIndex = 0)
    {
        GUI.Label(new Rect(((position == GUIPosition.TopLeft || position == GUIPosition.BottomLeft)?0:Screen.width / 2f) + 10, ((position == GUIPosition.TopLeft || position == GUIPosition.TopRight) ? 0 : Screen.height / 2f) + 10 + lineIndex * 20, 250, 20), text);
    }
}
