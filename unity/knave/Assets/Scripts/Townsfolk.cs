using UnityEngine;
using System.Collections.Generic;

public class Townsfolk : Agent
{
	public const string TOWNSF_BEHAVIOR_MILL = "MILL";
	public const string TOWNSF_BEHAVIOR_DOZE = "DOZE";

	public const string TOWNSF_BEHAVIOR_AGHAST = "AGHAST";
	public const string TOWNSF_BEHAVIOR_SEEKPOLICE = "SEEKPOLICE";
	public const string TOWNSF_BEHAVIOR_FLEE = "FLEE";

	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	private const uint EMOTE_NONE = 999;
	private const uint EMOTE_MARKED = 0;

	public Popup emotePopup;
	public GameObject coinEffectPrefab;

	public int minMillTime, maxMillTime;
	public int minDozeTime, maxDozeTime;

	public Rect millBounds;

	public int scareDistance; // how close to player i can be before running
	public int informPoliceDistance; // how close to police i must be to inform them

	public float aghastSpeed;
	
	private Vector3 playerLastSeen;
	private int aghastTime;
	private int dozeTime;
	private int millTime;
	
	private Actor whoStoleFrom;

	private Police policeToSeek;
	private float baseSpeed;
	
	protected override void Awake()
	{
		this.playerLastSeen = this.transform.position;

		this.baseSpeed = this.speed;
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
		else if (this.behaviorState == TOWNSF_BEHAVIOR_SEEKPOLICE)
		{
			updateSeekPoliceState();
		}
		else if (this.behaviorState == TOWNSF_BEHAVIOR_FLEE)
		{
			updateFleeState();
		}

		base.FixedUpdate();
	}

	private void OnCollisionEnter(Collision info)
	{
		if (info.gameObject == Registry.Instance.player.gameObject)
		{
			this.whoStoleFrom = Registry.Instance.player;
			this.stealFrom();
		}
		
		if (Drunk.USE_DRUNK)
		{
			if (info.gameObject == Registry.Instance.drunk.gameObject)
			{
				this.whoStoleFrom = Registry.Instance.drunk;
				this.stealFrom();
			}
		}
		
	}

	private void updateFleeState()
	{
		emote(EMOTE_MARKED);

		Vector3 myPos = this.transform.position;
		Vector3 playerPos = Registry.Instance.player.transform.position;
		float distToPlayer = Vector3.Distance(playerPos, myPos);

		if (seesEntity(Registry.Instance.player))
		{
			playerLastSeen = Registry.Instance.player.transform.position;
		}

		if (this.isDestinationReached() || (distToPlayer > 2 * this.scareDistance))
		{
			startAghast();
		}
	}

	private void updateSeekPoliceState()
	{
		Vector3 myPos = this.transform.position;
		Vector3 playerPos = Registry.Instance.player.transform.position;
		Vector3 policePos = this.policeToSeek.transform.position;
		float distToPlayer = Vector3.Distance(playerPos, myPos);

		emote(EMOTE_MARKED);

		if (seesEntity(Registry.Instance.player))
		{
			playerLastSeen = Registry.Instance.player.transform.position;

			if (distToPlayer < this.scareDistance)
			{
				startFlee();

				return;
			}
		}

		if (Vector3.Distance(policePos, myPos) < this.informPoliceDistance)
		{
			this.policeToSeek.informActorPosition(playerLastSeen,this.whoStoleFrom);
			startMill();

			return;
		}
		else if (this.isDestinationReached())
		{
			this.setDestination(policePos);
		}
	}

	private void updateAghastState()
	{
		Vector3 myPos = this.transform.position;
		Vector3 playerPos = Registry.Instance.player.transform.position;
		float distToPlayer = Vector3.Distance(playerPos, myPos);

		emote(EMOTE_MARKED);

		if (seesEntity(Registry.Instance.player) && distToPlayer < this.scareDistance)
		{
			startFlee();

			return;
		}

		List<Police> visibleList = new List<Police>();
		foreach (Police p in Registry.Instance.policeList)
		{
			if (this.seesEntity(p))
			{
				visibleList.Add(p);
			}
		}
		if (visibleList.Count > 0)
		{
			int index = Random.Range(0, visibleList.Count);
			startSeekPolice(visibleList[index]);
			return;
		}
		
		if (Game.gameTime() > this.aghastTime)
		{
			startMill();

			return;
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

	public override bool isAngry()
	{
		return (this.behaviorState == TOWNSF_BEHAVIOR_AGHAST ||
				this.behaviorState == TOWNSF_BEHAVIOR_FLEE ||
				this.behaviorState == TOWNSF_BEHAVIOR_SEEKPOLICE);
	}

	private void startDoze()
	{
		this.changeState(TOWNSF_BEHAVIOR_DOZE);
		this.setPathfindingEnabled(false);
		this.spriteAnimation.play(ANIM_IDLE);
		this.dozeTime = Game.gameTime() + Random.Range(minDozeTime, maxDozeTime);
		this.speed = this.baseSpeed;
	}

	private void startMill()
	{
		this.changeState(TOWNSF_BEHAVIOR_MILL);
		this.setPathfindingEnabled(true);
		this.setDestination(Game.getRandomGroundPosition(this.millBounds));
		this.spriteAnimation.play(ANIM_WALK, true);
		this.millTime = Game.gameTime() + Random.Range(minMillTime, maxMillTime);
		this.speed = this.baseSpeed;
	}

	private void startAghast()
	{
		this.changeState(TOWNSF_BEHAVIOR_AGHAST);
		this.setPathfindingEnabled(false);
		this.spriteAnimation.play(ANIM_IDLE, true);
		this.aghastTime = Game.gameTime() + 10000;
		this.speed = this.aghastSpeed;
	}

	private void startFlee()
	{
		this.changeState(TOWNSF_BEHAVIOR_FLEE);
		this.setPathfindingEnabled(true);
		this.spriteAnimation.play(ANIM_WALK, true);

		Vector3 myPos = this.transform.position;
		Vector3 playerPos = Registry.Instance.player.transform.position;

		this.setDestination(myPos + 5 * (myPos - playerPos).normalized);
		this.speed = this.aghastSpeed;
	}

	private void startSeekPolice(Police p)
	{
		this.changeState(TOWNSF_BEHAVIOR_SEEKPOLICE);
		this.setPathfindingEnabled(true);
		this.spriteAnimation.play(ANIM_WALK, true);

		this.policeToSeek = p;
		this.setDestination(p.transform.position);
		this.speed = this.aghastSpeed;
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