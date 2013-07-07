using UnityEngine;
using System.Collections;

public class King : Agent
{
	public int testVar = 3;
	public Vector3 destination = new Vector3(17, 0, -13);
	
	private void Start()
	{
		this.setDestination(destination);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}