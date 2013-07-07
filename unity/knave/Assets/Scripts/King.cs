using UnityEngine;
using System.Collections;

public class King : Agent
{
	private void Start()
	{
		this.setDestination(new Vector3(5, 0, 7));
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}