using UnityEngine;
using System.Collections;

public class Drunk : Agent
{	
	public const string DRUNK_BEHAVIOR_MILL = "MILL";
	public const string DRUNK_BEHAVIOR_DOZE = "DOZE";
	public const string DRUNK_BEHAVIOR_SEEKTOWNSF = "SEEKTOWNSF";
	
	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	private const uint EMOTE_NONE = 999;
	private const uint EMOTE_MARKED = 0;
	
	public int minMillTime, maxMillTime;
	public int minDozeTime, maxDozeTime;
	
	public Popup emotePopup;
	public Rect millBounds;

	
	private int dozeTime;
	private int millTime;
	private Townsfolk townsfToSeek;
	private float baseSpeed;
	
	private bool justCollided;
 	private Collider justCollidedWith;
	
	protected override void Awake()
	{
		this.baseSpeed = this.speed;
	}
	
	protected void Start()
	{
		this.spriteAnimation.addClip(ANIM_IDLE, new SpriteAnimation.Clip(0, 1, 150, WrapMode.Loop));
		this.spriteAnimation.addClip(ANIM_WALK, new SpriteAnimation.Clip(1, 5, 150, WrapMode.Loop));
		this.spriteAnimation.play(ANIM_IDLE);

		startSeekTownsf(Registry.Instance.townsfolk);
	}
	
	
	protected void OnCollisionEnter(Collision collision)
	{
		this.justCollided = true;
		this.justCollidedWith = collision.collider;
	}
	
	protected override void FixedUpdate()
	{
		if (this.behaviorState == DRUNK_BEHAVIOR_MILL)
		{
			updateMillState();
		}
		else if (this.behaviorState == DRUNK_BEHAVIOR_DOZE)
		{
			updateDozeState();
		}
		else if (this.behaviorState == DRUNK_BEHAVIOR_SEEKTOWNSF)
		{
			updateSeekTownsfState();
		}

		base.FixedUpdate();
	}
	
	private void updateSeekTownsfState()
	{
		Vector3 townsfPos = this.townsfToSeek.transform.position;

		emote(EMOTE_MARKED);
		
		if (this.justCollided && this.justCollidedWith == Registry.Instance.drunk.collider)
		{
			this.startMill();
		} else if (this.isDestinationReached())
		{
			this.setDestination(townsfPos);
		} 

	}
	
	private void updateMillState()
	{
		emote(EMOTE_NONE);

		if (this.isDestinationReached())
		{
			this.setDestination(Game.getRandomGroundPosition(this.millBounds));
		}

		if (Game.gameTime() > millTime)
		{
			startDoze();
		}
	}

	private void updateDozeState()
	{
		emote(EMOTE_NONE);

		if (Game.gameTime() > dozeTime)
		{
			startMill();
		}
	}
	
	private void startDoze()
	{
		this.changeState(DRUNK_BEHAVIOR_DOZE);
		this.setPathfindingEnabled(false);
		this.spriteAnimation.play(ANIM_IDLE);
		this.dozeTime = Game.gameTime() + Random.Range(minDozeTime, maxDozeTime);
		this.speed = this.baseSpeed;
	}

	private void startMill()
	{
		this.changeState(DRUNK_BEHAVIOR_MILL);
		this.setPathfindingEnabled(true);
		this.setDestination(Game.getRandomGroundPosition(this.millBounds));
		this.spriteAnimation.play(ANIM_WALK, true);
		this.millTime = Game.gameTime() + Random.Range(minMillTime, maxMillTime);
		this.speed = this.baseSpeed;
	}
	
	private void emote(uint emoteIndex)
	{
		switch (emoteIndex)
		{
			case EMOTE_MARKED:
				this.emotePopup.gameObject.SetActive(true);
				this.emotePopup.playClip(emoteIndex);
				break;

			default:
				this.emotePopup.gameObject.SetActive(false);
				break;
		}
	}
	
	private void startSeekTownsf(Townsfolk tf)
	{
		this.changeState(DRUNK_BEHAVIOR_SEEKTOWNSF);
		this.setPathfindingEnabled(true);
		this.spriteAnimation.play(ANIM_WALK, true);

		this.townsfToSeek = tf;
		this.setDestination(tf.transform.position);
	}

}

