using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GuardAI : MonoBehaviour {

	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask obstacleMask;
	public LayerMask playerMask;

	[HideInInspector]
	public List<Transform> visiblePlayer;

	public float meshResolution;
	public MeshFilter viewMeshFilter;
	Mesh viewMesh;

	public bool canSeeTarget;
	private bool hasLostTarget;
	public float turnSpeed = 20;
	public float turnDampning;
	public GameObject Bullet;
	public Transform muzzle;

	Transform target;
	private float timeTillNextShot = 0f;
	public float fireRate;

	public float speed = 5;
	public float waitTime = .3f;

	public Transform pathHolder;

	AudioSource audioSource;
	public AudioClip shotSound;
	public AudioClip heySound;
	bool hasAudioPlayed = false;

	private Animator anim;

	// Use this for initialization
	private void Start () {
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;

		anim = GetComponentInChildren<Animator>();
		audioSource = GetComponent<AudioSource>();

		target = FindObjectOfType<PlayerMovementMouse>().transform;
		Vector2[] waypoints = new Vector2[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++)
		{
			waypoints[i] = pathHolder.GetChild(i).position;
		}

		transform.position = waypoints[0];
		StartCoroutine(FollowPath(waypoints));
	}

	private void OnEnable()
	{
		Start();
	}

	private void LateUpdate()
	{
		DrawFieldOfView();
	}

	public Vector2 DirFromAngle (float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += -transform.eulerAngles.z;
		}
		return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	private void CheckVisibility()
	{
		canSeeTarget = false;
		visiblePlayer.Clear();

		Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll (transform.position, viewRadius, playerMask);

		for (int i = 0; i < targetsInRadius.Length; i++)
		{
			Transform player = targetsInRadius[0].transform;
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			if (Vector2.Angle(transform.up, dirToPlayer) < viewAngle / 2)
			{
				float distToPlayer = Vector2.Distance(transform.position, player.position);
				if (!Physics2D.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
				{
					visiblePlayer.Add(player);
					canSeeTarget = true;
				}
			}
		}
	}

	private void DrawFieldOfView()
	{
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;

		List<Vector2> viewPoints = new List<Vector2>();

		for (int i = 0; i < stepCount; i++)
		{
			float angle = -transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle);
			viewPoints.Add(newViewCast.point);
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector2.zero;
		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

			if (i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}

	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector2 dir = DirFromAngle(globalAngle, true);
		RaycastHit2D hit;

		hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);
		if (hit)
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, (Vector2)transform.position + dir * viewRadius, viewRadius, globalAngle);
		}
	}

	private void Shoot() {
		if (Time.time >= timeTillNextShot)
		{
			audioSource.PlayOneShot(shotSound, 0.4f);
			timeTillNextShot = Time.time + 1f / fireRate;
			Destroy(Instantiate(Bullet, muzzle.position, transform.rotation), 5);
		}
	}

	IEnumerator FollowPath(Vector2[] waypoints)
	{
		int targetWaypointIndex = 1;
		Vector2 targetWaypoint = waypoints[targetWaypointIndex];
		Vector3 direction = targetWaypoint - (Vector2)transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.eulerAngles = Vector3.forward * (270 + angle);

		while (true)
		{
			CheckVisibility();

			anim.SetBool("IsShooting", canSeeTarget);
			anim.SetBool("IsWalking", true);

			if (!canSeeTarget)
			{
				transform.position = Vector2.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
				if ((Vector2)transform.position == targetWaypoint)
				{
					targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
					targetWaypoint = waypoints[targetWaypointIndex];
					yield return new WaitForSeconds(waitTime);
					yield return StartCoroutine(TurnToFace(targetWaypoint));
				}
			}
			else
			{
				if (!hasAudioPlayed)
				{
					audioSource.PlayOneShot(heySound);
					hasAudioPlayed = true;
				}
				direction = target.position - transform.position;
				angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				transform.eulerAngles = Vector3.forward * (270 + angle);

				Shoot();
			}
			yield return null;
		}
	}

	IEnumerator TurnToFace(Vector2 lookTarget)
	{
		Vector2 dirToLookTarget = (lookTarget - (Vector2)transform.position).normalized;
		float targetAngle = 360 - Mathf.Atan2(dirToLookTarget.x, dirToLookTarget.y) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle)) > 0.05f)
		{
			anim.SetBool("IsWalking", false);
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);

			CheckVisibility();
			if (canSeeTarget)
			{
				Vector2 direction = target.position - transform.position;
				angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				transform.eulerAngles = Vector3.forward * (270 + angle);
				Shoot();
				anim.SetBool("IsShooting", true);
			} else
			{
				transform.eulerAngles = Vector3.forward * angle;
				hasAudioPlayed = false;
				anim.SetBool("IsShooting", false);
			}

			yield return null;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Vector2 startPosition = pathHolder.GetChild(0).position;
		Vector2 previousPosition = startPosition;

		foreach (Transform waypoint in pathHolder)
		{
            Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(waypoint.position, .3f);
			Gizmos.DrawLine(previousPosition, waypoint.position);
			previousPosition = waypoint.position;
		}
        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(previousPosition, startPosition);
	}

	public struct ViewCastInfo
	{
		public bool hit;
		public Vector2 point;
		public float distance;
		public float angle;

		public ViewCastInfo(bool _hit, Vector2 _point, float _distance, float _angle)
		{
			hit = _hit;
			point = _point;
			distance = _distance;
			angle = _angle;
		}
	}
}
