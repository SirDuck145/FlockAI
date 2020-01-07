﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidPreyHelper : MonoBehaviour {
    const int numViewDirections = 300;
    public static readonly Vector3[] directions;

    static BoidPreyHelper() {
        directions = new Vector3[BoidPreyHelper.numViewDirections];

        float goldenRule = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRule;

        for (int i = 0; i < numViewDirections; i++) {
            float t = (float)i / numViewDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin (inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin (inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos (inclination);

            directions [i] =  new Vector3 (x, y, z);
        }
    }
}