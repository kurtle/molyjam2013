using UnityEngine;
using System.Collections;

public class King : Agent
{
	private const string KNG_BEHAVIOR_DAWDLE = "DAWDLE";
	private const string KNG_BEHAVIOR_LOITER = "LOITER";

	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	public int loiterMinTime = 1500;
	public int loiterMaxTime = 7500;

	public int dawdleMinTime = 2500;
	public int dawdleMaxTime = 9000;

	public Rect dawdleBounds;

	public float endGameDistance = 7f;
	public int requiredAngryCitizens = 4;

	private int changeStateAt;
	
	private void Start()
	{
		this.spriteAnimation.addClip(ANIM_IDLE, new SpriteAnimation.Clip(0, 1, 150, WrapMode.Loop));
		this.spriteAnimation.addClip(ANIM_WALK, new SpriteAnimation.Clip(1, 4, 150, WrapMode.Loop));
		this.spriteAnimation.play(ANIM_IDLE);

		this.setPathfindingEnabled(true);

		this.changeState(KNG_BEHAVIOR_LOITER);
		this.changeStateAt = Game.gameTime() + Random.Range(loiterMinTime, loiterMaxTime);
	}

	private void OnCollisionEnter(Collision info)
	{
		if (info.gameObject == Registry.Instance.player.gameObject)
		{
			Vector3 myPos = this.transform.position;

			int angryCount = 0;
			foreach (Agent citizen in Registry.Instance.citizenList)
			{
				if (citizen.isAngry() && Vector3.Distance(myPos, citizen.transform.position) < this.endGameDistance)
				{
					++angryCount;
				}
			}

			if (angryCount >= this.requiredAngryCitizens)
			{
				Registry.Instance.endGame();
			}
		}
	}

	protected override void FixedUpdate()
	{
		if (Game.gameTime() > this.changeStateAt)
		{
			if (this.behaviorState == KNG_BEHAVIOR_LOITER)
			{
				startDawdle();
			}
			else if (this.behaviorState == KNG_BEHAVIOR_DAWDLE)
			{
				startLoiter();
			}
		}

		base.FixedUpdate();
	}

	private void startDawdle()
	{
		this.setPathfindingEnabled(true);
		this.setDestination(Game.getRandomGroundPosition(this.dawdleBounds));
		this.spriteAnimation.play(ANIM_WALK);

		changeState(KNG_BEHAVIOR_DAWDLE);
		this.changeStateAt = Game.gameTime() + Random.Range(dawdleMinTime, dawdleMaxTime);
	}

	private void startLoiter()
	{
		this.setPathfindingEnabled(false);
		this.spriteAnimation.play(ANIM_IDLE);

		changeState(KNG_BEHAVIOR_LOITER);
		this.changeStateAt = Game.gameTime() + Random.Range(loiterMinTime, loiterMaxTime);
	}
}