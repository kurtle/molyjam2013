using UnityEngine;
using System.Collections;

public class Townsfolk : Agent
{
	public const string TOWNSF_BEHAVIOR_MILL = "MILL";
	public const string TOWNSF_BEHAVIOR_AGHAST = "AGHAST";
	
	private Vector3 playerLastSeen;


	private bool justCollided;

	protected override void Awake()
	{
		this.changeState(TOWNSF_BEHAVIOR_MILL);
		this.playerLastSeen = this.transform.position;
	}

	protected override void FixedUpdate()
	{
		if (this.behaviorState == TOWNSF_BEHAVIOR_MILL)
		{
			updateMillState();
		}
		else if (this.behaviorState == TOWNSF_BEHAVIOR_AGHAST)
		{
			updateAghastState();
		}
	}

	private void updateAghastState()
	{
		if (seesEntity(Registry.Instance.player))
		{
			playerLastSeen = Registry.Instance.player.transform.position;	
		} else {		
			foreach (Police p in Registry.Instance.policeList)
			{
				if (this.seesEntity(p) && p.isDestinationReached())
				{
					p.informPlayerPosition(playerLastSeen);
					//this.changeState(TOWNSF_BEHAVIOR_MILL);
				}
			}
		}
		
		if (Game.time() > this.lastBehaviorChangeTime + 10000) {
			this.changeState(TOWNSF_BEHAVIOR_MILL);
		}
		
		
	}

	private void updateMillState()
	{
		/*
		if (seesPlayer())
		{
			changeState(POL_BEHAVIOR_MARKED);
		}
		else if (Game.time() > this.lastBehaviorChangeTime + 3000)
		{
			changeState(POL_BEHAVIOR_PATROL);
		}
		else
		{
		*/
		this.moveDelta(Game.directionVector(Game.randomDirection()));
		//}
	}
	
	public void stealFrom()
	{
		this.changeState(TOWNSF_BEHAVIOR_AGHAST);
	}
}