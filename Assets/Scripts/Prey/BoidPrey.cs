using UnityEngine;

public class BoidPrey : MonoBehaviour {
    BoidPreySettings settings;

    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;
    [HideInInspector]
    public Vector3 zeroVector = new Vector3(0, 0, 0);

    // To Update:
    Vector3 acceleration;
    [HideInInspector]
    public Vector3 avgFlockHeading;
    [HideInInspector]
    public Vector3 avgAvoidanceHeading;
    [HideInInspector]
    public Vector3 centreOfFlockmates;
    [HideInInspector]
    public int numPerceivedFlockmates;

    // Cached
    Material material;
    Transform cachedTransform;
    Transform target;

    private void Awake() {
        material = transform.GetComponentInChildren<MeshRenderer>().material; //Returns the mesh renderer
        cachedTransform = transform;
    }

    public void Initialize (BoidPreySettings settings, Transform target) {
        this.target = target;
        this.settings = settings;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        //Start all boids moving at average speed
        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    //Sets the color of the boid
    public void SetColor(Color col) {
        if (material != null) {
            material.color = col;
        }
    }

    public void UpdateBoid() {
        Vector3 acceleration = Vector3.zero;

        //Scale movement based on the target
        if (target != null) {
            Vector3 offsetToTarget = (target.position - position);

            acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
        }

        //Scale movement based on the flock
        if (numPerceivedFlockmates != 0) {
            centreOfFlockmates /= numPerceivedFlockmates;

            Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);
            var alignmentForce = zeroVector;
            var cohesionForce = zeroVector;
            var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;

            if (isHunted()) {
                Debug.Log("IsHunted returns true");
                alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
                cohesionForce = SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
            }
            else {
                alignmentForce = -SteerTowards(avgFlockHeading) * settings.alignWeight;
                cohesionForce = -SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
            }

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        //Scale movement based on collisions
        if (isHeadingForCollision ()) {
            //Returns a new direction with no collision
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        velocity = dir * speed;

        cachedTransform.position += velocity * Time.deltaTime;
        cachedTransform.forward = dir;
        position = cachedTransform.position;
        forward = dir;
    }

    bool isHeadingForCollision() {
        RaycastHit hit;
        if (Physics.SphereCast(position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask)) {
            return true;
        }
        return false;
    }

    bool isHunted() {
        RaycastHit hit;
        if (Physics.SphereCast(position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask) && (hit.transform.tag == "Hunter")) {
            Debug.Log("Being Hunted");
            return true;
        }
        return false;
    }

    Vector3 ObstacleRays () {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++) {
            Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);
            if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask)) {
                return dir;
            }
        }

        return forward;
    }
    Vector3 SteerTowards (Vector3 vector) {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }
}
