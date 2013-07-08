using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Registry : MonoBehaviour
{
	private static Registry _instance;

	public Player player;

	public King king;
	
	public Police police1;
	public Police police2;
	public Police police3;
	
	public List<Police> policeList = new List<Police>();
	
	public Townsfolk townsfolk;
	public Drunk drunk;

	public List<Agent> citizenList = new List<Agent>();

	public MeshRenderer winScreen;

	private int showWinScreenTime = int.MaxValue;

	private void Awake()
	{
		_instance = this;
		policeList.Add(police1);
		policeList.Add(police2);
		policeList.Add(police3);

		foreach (Police p in policeList)
		{
			citizenList.Add(p);
		}
		citizenList.Add(townsfolk);
	}

	public static Registry Instance
	{
		get
		{
			return _instance;
		}
	}

	public void endGame()
	{
		if (this.showWinScreenTime == int.MaxValue)
		{
			this.showWinScreenTime = Game.gameTime() + 2000;
		}
	}

	private void Update()
	{
		if (Game.gameTime() > this.showWinScreenTime)
		{
			this.winScreen.enabled = true;
		}
	}
}