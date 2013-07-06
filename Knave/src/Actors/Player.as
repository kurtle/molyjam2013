package Actors
{
	import org.flixel.*;
	
	public class Player extends Actor
	{
		[Embed(source = "../../img/knaveTest.png")]public var knaveTest:Class;
		
		public var playerIndex:int;
		
		public function Player(X:Number=0, Y:Number=0, SimpleGraphic:Class=null)
		{
			super(X, Y, SimpleGraphic);
			
			loadGraphic(knaveTest, true, true, 20, 20);
			addAnimation("walking", [0, 1, 2], 10, true);
			addAnimation("idle", [0], 1, true);
			play("idle");
			
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
					play("walking");
				}
				else if (FlxG.keys.S)
				{
					deltaY += 1;
					play("walking");
				}
				else if (FlxG.keys.A)
				{
					deltaX -= 1;
					facing = LEFT;
					play("walking");
				}
				else if (FlxG.keys.D)
				{
					deltaX += 1;
					facing = RIGHT;
					play("walking");
				}
				else {
					play("idle");
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