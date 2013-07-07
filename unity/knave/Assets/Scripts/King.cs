using UnityEngine;
using System.Collections;

public class King : Agent
{
	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	public int testVar = 3;
	public Vector3 destination = new Vector3(17, 0, -13);
	
	private void Start()
	{
		this.spriteAnimation.addClip(ANIM_IDLE, new SpriteAnimation.Clip(0, 1, 150, WrapMode.Loop));
		this.spriteAnimation.addClip(ANIM_WALK, new SpriteAnimation.Clip(1, 4, 150, WrapMode.Loop));
		this.spriteAnimation.play(ANIM_WALK);

		this.setPathfindingEnabled(true);
		this.setDestination(destination);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}