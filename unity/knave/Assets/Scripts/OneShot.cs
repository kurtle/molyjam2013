using UnityEngine;
using System.Collections;

public class OneShot : MonoBehaviour
{
	public int duration;
	private int expiryTime;

	private void Awake()
	{
		this.expiryTime = Game.renderTime() + this.duration;
	}

	private void Update()
	{
		if (Game.renderTime() > this.expiryTime)
		{
			Destroy(this.gameObject);
		}
	}
}