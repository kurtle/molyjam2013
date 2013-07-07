using UnityEngine;
using System.Collections;

public class Townsfolk : Agent
{
	public const string TOWNSF_BEHAVIOR_MILL = "MILL";
	public const string TOWNSF_BEHAVIOR_DOZE = "DOZE";

	public const string TOWNSF_BEHAVIOR_AGHAST = "AGHAST";

	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	private const uint EMOTE_NONE = 999;
	private const uint EMOTE_MARKED = 0;

	public Popup emotePopup;
	public GameObject coinEffectPrefab;

	public int minMillTime, maxMillTime;
	public int minDozeTime, maxDozeTime;

	public Rect millBounds;
	
	private Vector3 playerLastSeen;
	private int aghastTime;
	private int dozeTime;
	private int millTime;
	
	protected override void Awake()
	{
		this.playerLastSeen = this.transform.position;
	}

	protected void Start()
	{
		this.spriteAnimation.addClip(ANIM_IDLE, new SpriteAnimation.Clip(0, 1, 150, WrapMode.Loop));
		this.spriteAnimation.addClip(ANIM_WALK, new SpriteAnimation.Clip(1, 5, 150, WrapMode.Loop));
		this.spriteAnimation.play(ANIM_IDLE);

		startDoze();
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
		else if (this.behaviorState == TOWNSF_BEHAVIOR_DOZE)
		{
			updateDozeState();
		}

		base.FixedUpdate();
	}

	private void OnCollisionEnter(Collision info)
	{
		if (info.gameObject == Registry.Instance.player.gameObject)
		{
			this.stealFrom();
		}
	}

	private void updateAghastState()
	{
		emote(EMOTE_MARKED);

		if (seesEntity(Registry.Instance.player))
		{
			playerLastSeen = Registry.Instance.player.transform.position;
		}

		/*foreach (Police p in Registry.Instance.policeList)
		{
			if (this.seesEntity(p)) //&& p.isDestinationReached())
			{
				p.informPlayerPosition(playerLastSeen);
				this.changeState(TOWNSF_BEHAVIOR_MILL);
				this.spriteAnimation.play(ANIM_WALK, true);
			}
		}*/
		
		if (Game.gameTime() > this.aghastTime)
		{
			startMill();
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
	
	public void stealFrom()
	{
		GameObject.Instantiate(this.coinEffectPrefab, this.transform.position, Quaternion.identity);

		startAghast();
	}

	private void startDoze()
	{
		this.changeState(TOWNSF_BEHAVIOR_DOZE);
		this.setPathfindingEnabled(false);
		this.spriteAnimation.play(ANIM_IDLE);
		this.dozeTime = Game.gameTime() + Random.Range(minDozeTime, maxDozeTime);
	}

	private void startMill()
	{
		this.changeState(TOWNSF_BEHAVIOR_MILL);
		this.setPathfindingEnabled(true);
		this.setDestination(Game.getRandomGroundPosition(this.millBounds));
		this.spriteAnimation.play(ANIM_WALK, true);
		this.millTime = Game.gameTime() + Random.Range(minMillTime, maxMillTime);
	}

	private void startAghast()
	{
		this.changeState(TOWNSF_BEHAVIOR_AGHAST);
		this.setPathfindingEnabled(true);
		this.spriteAnimation.play(ANIM_IDLE, true);
		this.aghastTime = Game.gameTime() + 10000;
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
}