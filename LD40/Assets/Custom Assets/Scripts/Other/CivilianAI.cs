using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianAI : MonoBehaviour
{

	public float speed = 5;
	public float waitTime = .3f;
	public float turnSpeed = 90;

	public Transform pathHolder;

	void Start()
	{

		Vector2[] waypoints = new Vector2[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++)
		{
			waypoints[i] = pathHolder.GetChild(i).position;
		}

		StartCoroutine(FollowPath(waypoints));

	}

	IEnumerator FollowPath(Vector2[] waypoints)
	{
		transform.position = waypoints[0];

		int targetWaypointIndex = 1;
		Vector2 targetWaypoint = waypoints[targetWaypointIndex];
		//transform.LookAt(targetWaypoint);

		while (true)
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
