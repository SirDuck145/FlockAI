using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPrey : MonoBehaviour
{
    public enum GizmoType { Never, SelectedOnly, Always }

    public BoidPrey prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;
    public Color color;
    public GizmoType showSpawnRegion;

    void Awake() {
        for (int i = 0; i < spawnCount; i++) {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            BoidPrey boid = Instantiate(prefab);
            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;

            boid.SetColor(color);
        }
    }

    private void onDrawGizmos() {
        if (showSpawnRegion == GizmoType.Always) {
            DrawGizmos();
        }
    }

    private void onDrawGizmosSelected() {
        if (showSpawnRegion == GizmoType.SelectedOnly) {
            DrawGizmos();
        }
    }

    void DrawGizmos() {
        Gizmos.color = new Color(color.r, color.g, color.b, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}
