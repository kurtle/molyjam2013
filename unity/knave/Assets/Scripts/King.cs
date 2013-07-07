using UnityEngine;
using System.Collections;

public class King : Agent
{
	private const string KNG_BEHAVIOR_DAWDLE = "DAWDLE";
	private const string KNG_BEHAVIOR_LOITER = "LOITER";

	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	public int testVar = 3;
	public Vector3 destination = new Vector3(17, 0, -13);

	private int changeStateAt;
	
	private void Start()
	{
		this.spriteAnimation.addClip(ANIM_IDLE, new SpriteAnimation.Clip(0, 1, 150, WrapMode.Loop));
		this.spriteAnimation.addClip(ANIM_WALK, new SpriteAnimation.Clip(1, 4, 150, WrapMode.Loop));
		this.spriteAnimation.play(ANIM_WALK);

		this.setPathfindingEnabled(true);

		this.changeState(KNG_BEHAVIOR_LOITER);
		this.changeStateAt = Game.gameTime() + Random.Range(1500, 7500);
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
		this.setDestination(new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f)));
		changeState(KNG_BEHAVIOR_DAWDLE);
		this.changeStateAt = Game.gameTime() + Random.Range(2500, 9000);
	}

	private void startLoiter()
	{
		changeState(KNG_BEHAVIOR_LOITER);
		this.changeStateAt = Game.gameTime() + Random.Range(1500, 7500);
	}
}