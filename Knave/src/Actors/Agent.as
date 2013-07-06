package Actors {
	import org.flixel.FlxG;
	import org.flixel.FlxPoint;
	import org.flixel.FlxTilemap;
	import Actors.Actor;

	/**
	 * @author ajaffe
	 */
	public class Agent extends Actor {
		
		public static const BEHAVIOR_PATROL:int = 1;
		public static const BEHAVIOR_MARKED:int = 2;
		public static const BEHAVIOR_ALERT:int = 3;
		
		private var behaviorState:int;
		
		public function Agent(X : Number = 0, Y : Number = 0, SimpleGraphic : Class = null) {
			super(X, Y, SimpleGraphic);
			
			behaviorState = BEHAVIOR_PATROL;
		}
		
		public function seesPlayer():Boolean {
			
			var collisionPoint:FlxPoint = new FlxPoint(-1000000,-1000000);
			
			//Try to see player
			if(Registry.map.ray(this.last,Registry.p1.last,collisionPoint,100))
			{
				//If line of sight was interrupted, collisionPoint contains that location. We won't use it for now.
				behaviorState = BEHAVIOR_MARKED;
				FlxG.log("MARKED");
			}
			return false;
		}
		
		override public function update():void {
			seesPlayer();
		}
		
	}
	
}
