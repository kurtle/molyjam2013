package Actors {
	import flash.geom.Point;
	import org.flixel.FlxObject;
	import flash.utils.getTimer;
	import org.flixel.FlxG;
	import org.flixel.FlxPoint;
	import org.flixel.FlxTilemap;
	import Actors.Actor;

	/**
	 * @author ajaffe
	 */
	public class Agent extends Actor {
		
		protected var behaviorState:String;
		protected var numStateChanges:int = 0;
		protected var lastBehaviorChangeTime:int = 0;
		
		
		public function Agent(X : Number = 0, Y : Number = 0, SimpleGraphic : Class = null) {
			super(X, Y, SimpleGraphic);
		}
		
		public function seesPlayer():Boolean {
			//If line of sight was interrupted, collisionPoint contains that location. We won't use it for now.
			var collisionPoint:FlxPoint = new FlxPoint(-1000000,-1000000);
			
			//Try to see player
			var myPos:FlxPoint = new FlxPoint(x,y);
			var playerPos:FlxPoint = new FlxPoint(Registry.p1.x,Registry.p1.y);
			return Registry.map.ray(myPos,playerPos,collisionPoint,100);
		}
		
				
		protected function changeState(newState:String):void
		{
			numStateChanges += 1;
			behaviorState = newState;
			lastBehaviorChangeTime = getTimer();
			FlxG.log("State: " + behaviorState + " (Num " + numStateChanges + ")");
		}
		
		public static function reverseDirection(dir:uint):uint
		{
			if (dir == FlxObject.UP)
			{
				return FlxObject.DOWN;
			} else if (dir == FlxObject.DOWN)
			{
				return FlxObject.UP;
			} else if (dir == FlxObject.LEFT)
			{
				return FlxObject.RIGHT;
			} else if (dir == FlxObject.RIGHT)
			{
				return FlxObject.LEFT;
			}
			return 99999;
		}
		
		public static function directionVector(dir:uint):Point
		{
			if (dir == FlxObject.UP)
			{
				return new Point(0,-1);
			} else if (dir == FlxObject.DOWN)
			{
				return new Point(0,1);
			} else if (dir == FlxObject.LEFT)
			{
				return new Point(-1,0);
			} else if (dir == FlxObject.RIGHT)
			{
				return new Point(1,0);
			} else {
				return null;
			}
		}
	
	}
	
}
