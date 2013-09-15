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

	public static Direction randomDirection()
	{
		return (Game.Direction)Random.Range(0,4);
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

	public static int deltaTime()
	{
		return (int)(Time.deltaTime * 1000);
	}
	public static int gameTime()
	{
		return (int)(Time.fixedTime * 1000);
	}
	public static int renderTime()
	{
		return (int)(Time.time * 1000);
	}

	public static Vector3 getRandomGroundPosition(Rect bounds)
	{
		return new Vector3(Random.Range(bounds.xMin, bounds.xMax), 0, Random.Range(bounds.yMin, bounds.yMax));
	}
}