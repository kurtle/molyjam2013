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
			this.currentPath = null;

			this.pathfindingEnabled = value;
		}
	}

	public bool seesEntity(Actor entity)
	{
		Vector3 agentPos = this.transform.position;
		Vector3 entityPos = entity.transform.position;

		RaycastHit hitInfo;
		if (Physics.Raycast(agentPos, entityPos - agentPos, out hitInfo))
		{
			if (hitInfo.rigidbody == null)
			{
				// wall collision!
				return false;
			}
			else if (hitInfo.collider.gameObject == entity.gameObject)
			{
				return true;
			}
		}

		return false;
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