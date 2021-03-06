using UnityEngine;
using System.Collections;

public class Player : Actor 
{
	private const string ANIM_IDLE = "idle";
	private const string ANIM_WALK = "walk";

	protected void Start()
	{
		this.spriteAnimation.addClip(ANIM_IDLE, new SpriteAnimation.Clip(0, 1, 150, WrapMode.Loop));
		this.spriteAnimation.addClip(ANIM_WALK, new SpriteAnimation.Clip(1, 6, 150, WrapMode.Loop));

		this.spriteAnimation.play(ANIM_IDLE);
	}

	protected override void FixedUpdate()
	{
		Vector3 inputDelta = Vector3.zero;

		if (Input.GetKey(KeyCode.W))
		{
			inputDelta.z += 1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			inputDelta.x -= 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			inputDelta.z -= 1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			inputDelta.x += 1;
		}

		if (inputDelta == Vector3.zero)
		{
			this.spriteAnimation.play(ANIM_IDLE);
		}
		else
		{
			this.spriteAnimation.play(ANIM_WALK);
		}
		
		//temp
		if (Input.GetKey(KeyCode.E))
		{
			foreach (Police p in Registry.Instance.policeList)
			{
				if(p.seesEntity(this))
				{
					p.informActorPosition(this.transform.position, this);
				}
			}
		}

		this.moveDelta(inputDelta.normalized);

		base.FixedUpdate();
	}
}