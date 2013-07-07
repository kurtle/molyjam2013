using UnityEngine;
using System.Collections;

public class Police : Agent
{
	public const string POL_BEHAVIOR_PATROL = "PATROL";
	public const string POL_BEHAVIOR_MARKED = "MARKED";
	public const string POL_BEHAVIOR_ALERT = "ALERT";

	public const string POL_BEHAVIOR_DESTINATION = "DESTINATION";

	public Popup emotePopup;

	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	private const uint EMOTE_NONE = 999;
	private const uint EMOTE_MARKED = 0;
	private const uint EMOTE_ALERT = 1;
	
	private Vector3 playerLastSeen;

	private Game.Direction patrolDirection;

	private bool justCollided;
	private float baseSpeed;

	protected void Start()
	{
		this.spriteAnimation.addClip(ANIM_IDLE, new SpriteAnimation.Clip(0, 1, 150, WrapMode.Loop));
		this.spriteAnimation.addClip(ANIM_WALK, new SpriteAnimation.Clip(1, 6, 150, WrapMode.Loop));
		this.spriteAnimation.play(ANIM_IDLE);

		this.changeState(POL_BEHAVIOR_PATROL);
		this.patrolDirection = Game.Direction.LEFT;
		
		this.baseSpeed = speed;
		this.setPathfindingEnabled(false);
		//this.destination = this.transform.position;
	}

	protected void OnCollisionEnter()
	{
		this.justCollided = true;
	}

	protected override void FixedUpdate()
	{
		if (seesEntity(Registry.Instance.player))
		{
			playerLastSeen = Registry.Instance.player.transform.position;
		}
		
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

		base.FixedUpdate();
	}

	private void updatePatrolState()
	{
		emote(EMOTE_NONE);

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

			this.spriteAnimation.play(ANIM_WALK, true);
		}
	}

	private void updateMarkedState()
	{
		emote(EMOTE_MARKED);

		if (seesEntity(Registry.Instance.player))
		{
			changeState(POL_BEHAVIOR_MARKED);
			
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

	private void updateAlertState()
	{
		emote(EMOTE_ALERT);

		if (seesEntity(Registry.Instance.player))
		{
			changeState(POL_BEHAVIOR_MARKED);
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
		emote(EMOTE_NONE);
		if (seesEntity(Registry.Instance.player))
		{
			changeState(POL_BEHAVIOR_MARKED);
			this.setPathfindingEnabled(false);
		} else if (this.isDestinationReached())
		{
			changeState(POL_BEHAVIOR_PATROL);
			this.setPathfindingEnabled(false);
		}
	}
	
	public void informPlayerPosition(Vector3 lastPos)
	{
		changeState(POL_BEHAVIOR_DESTINATION);
		//destination = lastPos;
		this.setPathfindingEnabled(true);
		this.setDestination(lastPos);
	}

	private void emote(uint emoteIndex)
	{
		switch (emoteIndex)
		{
			case EMOTE_ALERT:
			case EMOTE_MARKED:
				this.emotePopup.gameObject.SetActive(true);
				this.emotePopup.playClip(emoteIndex);				
				break;

			default:
				this.emotePopup.gameObject.SetActive(false);
				break;
		}
	}	
}