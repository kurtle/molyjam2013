using UnityEngine;
using System.Collections;

public class Agent : Actor
{
	public NavMeshAgent navMeshAgent;

	private NavMeshPath currentPath;
	private int currentCornerIndex;

	protected string behaviorState;
	protected int numStateChanges;
	protected int lastBehaviorChangeTime;

	private bool pathfindingEnabled;

	protected virtual void Awake()
	{
		if (this.navMeshAgent == null)
		{
			Debug.LogError("NO NAVMESHAGENT assigned to " + this);
		}
		this.currentPath = null;
	}

	protected override void FixedUpdate()
	{
		if (this.pathfindingEnabled)
		{
			if (this.currentPath == null)
			{
				if (this.navMeshAgent.hasPath)
				{
					this.currentPath = this.navMeshAgent.path;
					this.currentCornerIndex = 0;
				}
			}
			else
			{
				if (this.debugEnabled)
				{
					Debug.DrawLine(this.transform.localPosition, this.currentPath.corners[this.currentCornerIndex], Color.red);
				}

				if (Vector3.Distance(this.transform.localPosition, this.currentPath.corners[this.currentCornerIndex]) < 0.25f)
				{
					if (this.currentCornerIndex < this.currentPath.corners.Length - 1)
					{
						++this.currentCornerIndex;
					}
					else
					{
						this.currentPath = null;

						this.navMeshAgent.ResetPath();
					}
				}
				else
				{
					Vector3 dir = this.currentPath.corners[this.currentCornerIndex] - this.transform.localPosition;
					dir = dir.normalized;

					if (this.debugEnabled)
					{
						Debug.Log("" + this.transform.localPosition + " " + this.currentPath.corners[this.currentCornerIndex] + " " + dir);
					}

					this.moveDelta(dir);
				}
			}
		}

		base.FixedUpdate();
	}

	public void setDestination(Vector3 dest)
	{
		this.currentPath = null;

		this.navMeshAgent.SetDestination(dest);
	}
	
	public bool isDestinationReached()
	{
		return this.currentPath == null;
	}


	protected void setPathfindingEnabled(bool value)
	{
		if (this.pathfindingEnabled != value)
		{
			if (!value)
			{
				this.currentPath = null;
				this.navMeshAgent.ResetPath();
			}

			this.pathfindingEnabled = value;
		}
	}

	public bool seesEntity(Actor entity)
	{
		Vector3 origin = this.transform.position;
		Vector3 entityPos = entity.transform.position;

		RaycastHit[] hitInfos;
		hitInfos = Physics.RaycastAll(origin, entityPos - origin);

		float distanceToWall = float.MaxValue;
		float distanceToPlayer = float.MaxValue;
		for (int i = 0; i < hitInfos.Length; ++i)
		{
			if (hitInfos[i].rigidbody == null)
			{
				// wall case
				float distance = Vector3.Distance(hitInfos[i].collider.transform.position, origin);
				distanceToWall = Mathf.Min(distanceToWall, distance);
			}
			else if (hitInfos[i].collider.gameObject == Registry.Instance.player.gameObject)
			{
				// player case
				distanceToPlayer = Vector3.Distance(origin, entityPos);
			}
		}

		return (distanceToPlayer < distanceToWall);
	}

	protected void changeState(string newState)
	{
		this.numStateChanges += 1;
		this.behaviorState = newState;
		this.lastBehaviorChangeTime = Game.gameTime();

		if (this.debugEnabled)
		{
			Debug.Log("State: " + this.behaviorState + " (Num " + this.numStateChanges + ")");
		}
	}
}