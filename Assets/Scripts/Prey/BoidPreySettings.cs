using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidPreySettings : ScriptableObject
{
    //Settings
    public float minSpeed = 3;
    public float maxSpeed = 7;
    public float perceptionRadius = 2.8f;
    public float avoidanceRadius = 1.5f;
    public float maxSteerForce = 3;

    public float alignWeight = 0.4f;
    public float cohesionWeight = 0.4f;
    public float seperateWeight = 1;

    public float targetWeight = 1;

    [Header("Collisions")]
    public LayerMask obstacleMask;
    public LayerMask hunterMask;
    public float boundsRadius = .27f;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;
}
