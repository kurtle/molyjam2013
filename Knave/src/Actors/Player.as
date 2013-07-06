package Actors
{
	import org.flixel.*;
	
	public class Player extends Actor
	{
		public var playerIndex:int;
		
		public function Player(X:Number=0, Y:Number=0, SimpleGraphic:Class=null)
		{
			super(X, Y, SimpleGraphic);
		}
		
		public function setPlayer(index:int):void
		{
			this.playerIndex = index;
		}
		
		override public function update():void
		{
			var deltaX:Number = 0;
			var deltaY:Number = 0;
			if (this.playerIndex == 0)
			{
				if (FlxG.keys.W)
				{
					deltaY -= 1;
				}
				else if (FlxG.keys.S)
				{
					deltaY += 1;
				}
				else if (FlxG.keys.A)
				{
					deltaX -= 1;
				}
				else if (FlxG.keys.D)
				{
					deltaX += 1;
				}
			}
			else if (this.playerIndex == 1)
			{
				if (FlxG.keys.UP)
				{
					deltaY -= 1;	
				}
				else if (FlxG.keys.DOWN)
				{
					deltaY += 1;
				}
				else if (FlxG.keys.LEFT)
				{
					deltaX -= 1;
				}
				else if (FlxG.keys.RIGHT)
				{
					deltaX += 1;
				}					
			}
			
			this.moveDelta(this.speed * deltaX, this.speed * deltaY);
		}
	}
}