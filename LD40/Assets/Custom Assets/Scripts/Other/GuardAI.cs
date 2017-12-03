using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour {

	public bool canSeeTarget = false;
	public float turnSpeed = 20;
	public float timeBetweenShots = 2;
	public GameObject Bullet;
	public Transform muzzle;

	public Transform target;
	private float timeTillNextShot = 0;

	public float speed = 5;
	public float waitTime = .3f;

	public Transform pathHolder;

	private bool hasLostTarget;

	// Use this for initialization
	void Start () {
		target = FindObjectOfType<PlayerMovementMouse>().transform;
		Vector2[] waypoints = new Vector2[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++)
		{
			waypoints[i] = pathHolder.GetChild(i).position;
		}

		transform.position = waypoints[0];
		StartCoroutine(FollowPath(waypoints));
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			Debug.DrawRay(transform.position, (target.position - transform.position) * 50f, Color.green);

			Vector3 direction = target.position - transform.position;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			//float angle = Vector3.Angle((target.position - transform.position), Vector3.up);

			if (canSeeTarget)
			{
				hasLostTarget = false;
				StopAllCoroutines();
				OnSee(angle);
			}
			else if (!hasLostTarget && !canSeeTarget)
			{
				Vector2[] waypoints = new Vector2[pathHolder.childCount];
				for (int i = 0; i < waypoints.Length; i++)
				{
					waypoints[i] = pathHolder.GetChild(i).position;
				}
				StartCoroutine(FollowPath(waypoints));
				hasLostTarget = true;
			}
		}
	}

	private void OnSee(float angle) {
		transform.eulerAngles = Vector3.forward * (270 + angle);
		Shoot();
	}

	private void Shoot() {
		print("shot");
		if (Time.time > timeTillNextShot)
		{
			timeTillNextShot += timeBetweenShots;
			Destroy(Instantiate(Bullet, muzzle.position, transform.rotation), 5);
		}
	}

	IEnumerator FollowPath(Vector2[] waypoints)
	{
		int targetWaypointIndex = 1;
		Vector2 targetWaypoint = waypoints[targetWaypointIndex];
		//transform.LookAt(targetWaypoint);

		while (true && !canSeeTarget)
		{
			transform.position = Vector2.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
			if ((Vector2)transform.position == targetWaypoint)
			{
				targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints[targetWaypointIndex];
				yield return new WaitForSeconds(waitTime);
				yield return StartCoroutine(TurnToFace(targetWaypoint));
			}
			yield return null;
		}
	}

	IEnumerator TurnToFace(Vector2 lookTarget)
	{
		Vector2 dirToLookTarget = (lookTarget - (Vector2)transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.x, dirToLookTarget.y) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle)) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.forward * angle;
			yield return null;
		}
	}

	void OnDrawGizmos()
	{
		Vector2 startPosition = pathHolder.GetChild(0).position;
		Vector2 previousPosition = startPosition;

		foreach (Transform waypoint in pathHolder)
		{
			Gizmos.DrawSphere(waypoint.position, .3f);
			Gizmos.DrawLine(previousPosition, waypoint.position);
			previousPosition = waypoint.position;
		}
		Gizmos.DrawLine(previousPosition, startPosition);
	}
}
