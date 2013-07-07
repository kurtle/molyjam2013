using UnityEngine;
using System.Collections;

public class Game
{
	public enum Direction
	{
		NONE = -1,

		UP = 0,
		RIGHT,
		DOWN,
		LEFT
	}

	public static Direction reverseDirection(Direction dir)
	{
		if (dir == Direction.UP)
		{
			return Direction.DOWN;
		}
		else if (dir == Direction.DOWN)
		{
			return Direction.UP;
		}
		else if (dir == Direction.LEFT)
		{
			return Direction.RIGHT;
		}
		else if (dir == Direction.RIGHT)
		{
			return Direction.LEFT;
		}

		return Direction.NONE;
	}

	public static Vector3 directionVector(Direction dir)
	{
		if (dir == Direction.UP)
		{
			return new Vector3(0, 0, 1);
		}
		else if (dir == Direction.DOWN)
		{
			return new Vector3(0, 0, -1);
		}
		else if (dir == Direction.LEFT)
		{
			return new Vector3(-1, 0, 0);
		}
		else if (dir == Direction.RIGHT)
		{
			return new Vector3(1, 0, 0);
		}

		return Vector3.zero;
	}

	public static int time()
	{
		return (int)(Time.fixedTime * 1000);
	}
}