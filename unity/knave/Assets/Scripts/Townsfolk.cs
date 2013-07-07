using UnityEngine;
using System.Collections;

public class Townsfolk : Agent
{
	public const string TOWNSF_BEHAVIOR_MILL = "MILL";
	public const string TOWNSF_BEHAVIOR_AGHAST = "AGHAST";
	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";
	
	private Vector3 playerLastSeen;


	private bool justCollided;
	
	protected void Start()
	{
		this.spriteAnimation.addClip(ANIM_IDLE, new SpriteAnimation.Clip(0, 1, 150, WrapMode.Loop));
		this.spriteAnimation.addClip(ANIM_WALK, new SpriteAnimation.Clip(1, 6, 150, WrapMode.Loop));
		this.spriteAnimation.play(ANIM_WALK);
	}
	
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
		}
//		} else {		
		foreach (Police p in Registry.Instance.policeList)
		{
			if (this.seesEntity(p)) //&& p.isDestinationReached())
			{
				p.informPlayerPosition(playerLastSeen);
				this.changeState(TOWNSF_BEHAVIOR_MILL);
				this.spriteAnimation.play(ANIM_WALK, true);
			}
			}
		//}
		
		//if (Game.gameTime() > this.lastBehaviorChangeTime + 10000) {
		//	this.changeState(TOWNSF_BEHAVIOR_MILL);
		//}
		
		
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
		this.spriteAnimation.play(ANIM_IDLE, true);
	}
}