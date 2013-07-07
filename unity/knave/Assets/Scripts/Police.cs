using UnityEngine;
using System.Collections;

public class Police : Agent
{
	public const string POL_BEHAVIOR_PATROL = "PATROL";
	//public const string POL_BEHAVIOR_CHASING = "CHASING";
	public const string POL_BEHAVIOR_ALERT = "ALERT";
	public const string POL_BEHAVIOR_DESTINATION = "DESTINATION";
	public const string POL_BEHAVIOR_EMBARRASSED = "EMBARASSED";
	
	public Popup emotePopup;

	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	private const uint EMOTE_NONE = 999;
	private const uint EMOTE_MARKED = 0;
	private const uint EMOTE_ALERT = 1;
	private const uint EMOTE_EMBARRASSED = 2;
	
	private Vector3 playerLastSeen;

	private Game.Direction patrolDirection;

	private bool justCollided;
	private Collider justCollidedWith;
	private float baseSpeed;
	
	private bool isPlayerMarked;
	
	private int updateCount;

	protected void Start()
	{
		this.spriteAnimation.addClip(ANIM_IDLE, new SpriteAnimation.Clip(0, 1, 150, WrapMode.Loop));
		this.spriteAnimation.addClip(ANIM_WALK, new SpriteAnimation.Clip(1, 6, 150, WrapMode.Loop));
		this.spriteAnimation.play(ANIM_IDLE);

		this.changeState(POL_BEHAVIOR_PATROL);
		this.patrolDirection = Game.Direction.LEFT;
		
		this.baseSpeed = speed;
		this.setPathfindingEnabled(false);
		this.isPlayerMarked = false;
		this.updateCount = 0;
		//this.destination = this.transform.position;
	}

	protected void OnCollisionEnter(Collision collision)
	{
		this.justCollided = true;
		this.justCollidedWith = collision.collider;
	}

	protected override void FixedUpdate()
	{

		base.FixedUpdate();
		
		if (seesEntity(Registry.Instance.player))
		{
			playerLastSeen = Registry.Instance.player.transform.position;
		}
		
		if (this.behaviorState == POL_BEHAVIOR_PATROL)
		{
			updatePatrolState();
		}
		//else if (this.behaviorState == POL_BEHAVIOR_CHASING)
		//{
		//	updateChasingState();
		//}
		else if (this.behaviorState == POL_BEHAVIOR_ALERT)
		{
			updateAlertState();
		}
		else if (this.behaviorState == POL_BEHAVIOR_DESTINATION)
		{
			updateDestinationState();
		}
		else if (this.behaviorState == POL_BEHAVIOR_EMBARRASSED)
		{
			updateEmbarrassedState();
		}


		this.justCollided = false;
		this.updateCount++;


	}

	private void updatePatrolState()
	{
		emote(EMOTE_NONE);

		if (seesEntity(Registry.Instance.player) && this.isPlayerMarked)
		{
			informPlayerPosition(playerLastSeen);
			//changeState(POL_BEHAVIOR_CHASING);
		} else
		{
			
		
			if (this.justCollided)
			{
				this.patrolDirection = Game.reverseDirection(this.patrolDirection);
			}
			this.moveDelta(Game.directionVector(this.patrolDirection));
	
			this.spriteAnimation.play(ANIM_WALK, true);
		}
	}
	
	/*
	private void updateChasingState()
	{
		emote(EMOTE_MARKED);
		
		if (this.justCollided && this.justCollidedWith == Registry.Instance.player.collider)
		{
			//caught player
			changeState(POL_BEHAVIOR_PATROL);
			this.isPlayerMarked = false;
		} else if (seesEntity(Registry.Instance.player))
		{
			//changeState(POL_BEHAVIOR_CHASING);
			
			this.speed = baseSpeed * 1.3f;

			Vector3 playerPos = Registry.Instance.player.transform.position;
			Vector3 myPos = this.transform.position;

			this.moveDelta((playerPos - myPos).normalized);

			this.spriteAnimation.play(ANIM_WALK, true);
		} else 
		{
			this.informPlayerPosition(playerLastSeen);
		}
	}
	*/

	private void updateAlertState()
	{
		emote(EMOTE_ALERT);

		if (seesEntity(Registry.Instance.player))
		{
			//changeState(POL_BEHAVIOR_CHASING);
			informPlayerPosition(this.playerLastSeen);
		}
		else if (Game.gameTime() > this.lastBehaviorChangeTime + 3000)
		{
			this.speed = baseSpeed;
			changeState(POL_BEHAVIOR_PATROL);
		}
		else
		{
			this.moveDelta(Game.directionVector(Game.randomDirection()));

			this.spriteAnimation.play(ANIM_WALK, true);
		}
	}
	
	private void updateDestinationState()
	{
		this.speed = baseSpeed * 1.3f;
		emote(EMOTE_MARKED);
		if (this.justCollided && this.justCollidedWith == Registry.Instance.player.collider)
		{
			//caught player
			changeState(POL_BEHAVIOR_EMBARRASSED);
			this.setPathfindingEnabled(false);
			this.isPlayerMarked = false;
		} else if (seesEntity(Registry.Instance.player))
		{
			//changeState(POL_BEHAVIOR_CHASING);
			//this.setPathfindingEnabled(false);
			if (updateCount % 15 == 0)
			{
				this.setDestination(playerLastSeen);
			}
		} else if (this.isDestinationReached())
		{
			changeState(POL_BEHAVIOR_PATROL);
			this.setPathfindingEnabled(false);
		}
	}
	
	private void updateEmbarrassedState()
	{
		emote(EMOTE_EMBARRASSED);
		if (Game.gameTime() > this.lastBehaviorChangeTime + 3000)
		{
			changeState(POL_BEHAVIOR_PATROL);
		}
	}

	public void informPlayerPosition(Vector3 lastPos)
	{
		this.isPlayerMarked = true;
		changeState(POL_BEHAVIOR_DESTINATION);
		this.setPathfindingEnabled(true);
		this.setDestination(lastPos);
	}

	public override bool isAngry()
	{
		return (this.behaviorState == POL_BEHAVIOR_DESTINATION);
	}

	private void emote(uint emoteIndex)
	{
		switch (emoteIndex)
		{
			case EMOTE_ALERT:
			case EMOTE_MARKED:
			case EMOTE_EMBARRASSED:
				this.emotePopup.gameObject.SetActive(true);
				this.emotePopup.playClip(emoteIndex);				
				break;

			default:
				this.emotePopup.gameObject.SetActive(false);
				break;
		}
	}	
}