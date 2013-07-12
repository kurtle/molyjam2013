using UnityEngine;
using System.Collections;

public class Drunk : Agent
{	
	public const bool USE_DRUNK = false;
	public const string DRUNK_BEHAVIOR_MILL = "MILL";
	public const string DRUNK_BEHAVIOR_DOZE = "DOZE";
	public const string DRUNK_BEHAVIOR_SEEKTOWNSF = "SEEKTOWNSF";
	public const string DRUNK_BEHAVIOR_SEEKBEER = "SEEKBEER";
	
	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	private const uint EMOTE_NONE = 999;
	private const uint EMOTE_MARKED = 0;
	private const uint EMOTE_BEER = 5;
	
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
		
		this.startSeekBeer();
		//this.startSeekTownsf(Registry.Instance.townsfolk);
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
		else if (this.behaviorState == DRUNK_BEHAVIOR_SEEKBEER)
		{
			updateSeekBeerState();
		}

		base.FixedUpdate();
	}
	
	public bool hasBeer()
	{
		return this.behaviorState == DRUNK_BEHAVIOR_SEEKTOWNSF;
	}
	
	private void updateSeekTownsfState()
	{
		Vector3 townsfPos = this.townsfToSeek.transform.position;

		emote(EMOTE_BEER);
		
		//if (this.justCollided && this.justCollidedWith == Registry.Instance.drunk.collider)
		//{
		//	this.startMill();
		//} 
		//else 
		if (this.justCollided)
		{
			foreach(Police p in Registry.Instance.policeList)
			{
				if (this.justCollidedWith == p.collider)
				{
					this.startSeekBeer();
					return;
				}
			}
		}	
		
		if (this.isDestinationReached())
		{
			this.setDestination(townsfPos);
		} 

	}

	private void updateSeekBeerState()
	{
		emote(EMOTE_MARKED);
		if (this.isDestinationReached() && Game.gameTime() > this.lastBehaviorChangeTime + 500)
		{
			this.startSeekTownsf(Registry.Instance.townsfolk);
		}
	}
	
	private void updateMillState()
	{
		//emote(EMOTE_NONE);

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
		//emote(EMOTE_NONE);

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
			case EMOTE_BEER:
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

	private void startSeekBeer()
	{
		this.changeState(DRUNK_BEHAVIOR_SEEKBEER);
		this.setPathfindingEnabled(true);
		this.spriteAnimation.play(ANIM_WALK, true);
		this.setDestination(new Vector3(0f,0f,-13.5f));
	}

}

