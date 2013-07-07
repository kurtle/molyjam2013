using UnityEngine;
using System.Collections;

public class Police : Agent
{
	public const string POL_BEHAVIOR_PATROL = "PATROL";
	public const string POL_BEHAVIOR_MARKED = "MARKED";
	public const string POL_BEHAVIOR_ALERT = "ALERT";
	public const string POL_BEHAVIOR_DESTINATION = "DESTINATION";
	
	//private Vector3 destination;
	private Game.Direction patrolDirection;

	private bool justCollided;

	protected override void Awake()
	{
		this.changeState(POL_BEHAVIOR_PATROL);
		this.patrolDirection = Game.Direction.LEFT;
		//this.destination = this.transform.position;
	}

	protected void OnCollisionEnter()
	{
		this.justCollided = true;
	}

	protected override void FixedUpdate()
	{
		if (this.behaviorState == POL_BEHAVIOR_PATROL)
		{
			updatePatrolState();
		}
		else if (this.behaviorState == POL_BEHAVIOR_MARKED)
		{
			updateMarkedState();
		}
		else if (this.behaviorState == POL_BEHAVIOR_ALERT)
		{
			updateAlertState();
		}
		else if (this.behaviorState == POL_BEHAVIOR_DESTINATION)
		{
			updateDestinationState();
		}


		this.justCollided = false;
	}

	private void updatePatrolState()
	{
		if (seesEntity(Registry.Instance.player))
		{
			changeState(POL_BEHAVIOR_MARKED);
		}
		else
		{
			if (this.justCollided)
			{
				this.patrolDirection = Game.reverseDirection(this.patrolDirection);
			}

			this.moveDelta(Game.directionVector(this.patrolDirection));
		}
	}

	private void updateMarkedState()
	{
		if (seesEntity(Registry.Instance.player))
		{
			changeState(POL_BEHAVIOR_MARKED);

			Vector3 playerPos = Registry.Instance.player.transform.position;
			Vector3 myPos = this.transform.position;

			this.moveDelta((playerPos - myPos).normalized);
		}
		else
		{
			changeState(POL_BEHAVIOR_ALERT);
		}
	}

	private void updateAlertState()
	{
		if (seesEntity(Registry.Instance.player))
		{
			changeState(POL_BEHAVIOR_MARKED);
		}
		else if (Game.time() > this.lastBehaviorChangeTime + 3000)
		{
			changeState(POL_BEHAVIOR_PATROL);
		}
		else
		{
			this.moveDelta(Game.directionVector(Game.randomDirection()));
		}
	}
	
	private void updateDestinationState()
	{
		base.FixedUpdate();
		if (seesEntity(Registry.Instance.player))
		{
			changeState(POL_BEHAVIOR_MARKED);
			this.navMeshAgent.ResetPath();
		} else if (this.isDestinationReached())
		{
			changeState(POL_BEHAVIOR_PATROL);
		}
	}
	
	public void informPlayerPosition(Vector3 lastPos)
	{
		changeState(POL_BEHAVIOR_DESTINATION);
		//destination = lastPos;
		this.setDestination(lastPos);
	}
	
}