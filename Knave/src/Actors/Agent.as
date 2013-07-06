package Actors {
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
			return Registry.map.ray(this.last,Registry.p1.last,collisionPoint,100);
		}
		
				
		protected function changeState(newState:String):void
		{
			numStateChanges += 1;
			behaviorState = newState;
			lastBehaviorChangeTime = getTimer();
			FlxG.log("State: " + behaviorState + " (Num " + numStateChanges + ")");
		}
		
		
		

	}
	
}
