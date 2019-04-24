using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class to compare predicted results
/// </summary>
public class StatTracker
{
    private int _valueCount;
    private float _meanPrecision;

    public void AddValue(Vector3 original, Vector3 predicted)
    {
        _meanPrecision = ((_meanPrecision * _valueCount) + Vector3.Distance(original, predicted)) / (_valueCount + 1);

        _valueCount++;
    }

    public float GetMeanPrecision()
    {
        return _meanPrecision;
    }
}
