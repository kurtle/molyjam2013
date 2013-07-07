using UnityEngine;
using System.Collections;

public class Agent : Actor
{
	public bool debugEnabled = false;

	public NavMeshAgent navMeshAgent;

	private NavMeshPath currentPath;
	private int currentCornerIndex;

	private void Awake()
	{
		if (this.navMeshAgent == null)
		{
			Debug.LogError("NO NAVMESHAGENT assigned to " + this);
		}
	}

	private void FixedUpdate()
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

	public void setDestination(Vector3 dest)
	{
		this.currentPath = null;

		this.navMeshAgent.SetDestination(dest);
	}
}