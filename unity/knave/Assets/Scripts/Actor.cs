using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	public bool debugEnabled = false;
	public SpriteAnimation spriteAnimation;
	public bool flipFacing;
	
	public float speed;

	private Vector3 lastPosition;

	public void moveDelta(Vector3 delta)
	{
		this.transform.localPosition += this.speed * delta;
	}

	public void setFacing(Game.Direction dir)
	{
		if (this.spriteAnimation == null) return;

		if (!this.flipFacing && dir == Game.Direction.LEFT ||
			this.flipFacing && dir == Game.Direction.RIGHT)
		{
			Vector3 scale = this.spriteAnimation.gameObject.transform.localScale;
			scale = new Vector3(-1 * Mathf.Abs(scale.x), scale.y, scale.z);
			this.spriteAnimation.gameObject.transform.localScale = scale;
		}
		else if (!this.flipFacing && dir == Game.Direction.RIGHT ||
				 this.flipFacing && dir == Game.Direction.LEFT)
		{
			Vector3 scale = this.spriteAnimation.gameObject.transform.localScale;
			scale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
			this.spriteAnimation.gameObject.transform.localScale = scale;
		}
	}

	protected virtual void FixedUpdate()
	{
		Vector3 position = this.transform.position;

		if (position.x > this.lastPosition.x)
		{
			setFacing(Game.Direction.RIGHT);
		}
		else if (position.x < this.lastPosition.x)
		{
			setFacing(Game.Direction.LEFT);
		}

		this.lastPosition = position;
	}
}