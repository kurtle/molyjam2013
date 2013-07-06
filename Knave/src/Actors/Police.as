package Actors {
	import org.flixel.FlxG;
	import org.flixel.FlxObject;
	import flash.geom.Point;
	import flash.utils.getTimer;
	import Actors.Agent;

	/**
	 * @author ajaffe
	 */
	public class Police extends Agent 
	{

		[Embed(source = "../../img/police.png")]public var policeGraphic:Class;

		public static const POL_BEHAVIOR_PATROL:String = "PATROL";
		public static const POL_BEHAVIOR_MARKED:String = "MARKED";
		public static const POL_BEHAVIOR_ALERT:String = "ALERT";
		
		private var patrolDirection:uint;
		
		public function Police(X : Number = 0, Y : Number = 0, SimpleGraphic : Class = null) {
			super(X, Y, SimpleGraphic);
			
			loadGraphic(policeGraphic, true, true, 20, 20);
			changeState(POL_BEHAVIOR_PATROL);
			patrolDirection = FlxObject.UP;
			this.speed = 1;
		}
		
		override public function update():void {
			if (behaviorState == POL_BEHAVIOR_PATROL)
			{
				updatePatrolState();
			}
			
			if (behaviorState == POL_BEHAVIOR_MARKED)
			{
				updateMarkedState();
			}
			
			if (behaviorState == POL_BEHAVIOR_ALERT)
			{
				updateAlertState();
			}
		}
		
		private function updatePatrolState():void
		{
			if (seesPlayer())
				{
					changeState(POL_BEHAVIOR_MARKED);
				} else {
					if (justTouched(patrolDirection))
					{
						patrolDirection = reverseDirection(patrolDirection);
					}
					var deltaDir:Point = directionVector(patrolDirection);
					this.moveDelta(this.speed * deltaDir.x, this.speed * deltaDir.y);
				}
		}
		
		private function updateMarkedState():void
		{
			if (seesPlayer())
			{
				//re-up the marked behavior
				changeState(POL_BEHAVIOR_MARKED);
				var playerDir:Vec = new Vec(Registry.p1.x - x, Registry.p1.y - y);
				this.moveDelta(this.speed*playerDir.x/playerDir.magnitude(),this.speed*playerDir.y/playerDir.magnitude());
			} else //if (getTimer() > lastBehaviorChangeTime + 3000)
			{
				changeState(POL_BEHAVIOR_ALERT);
			} 
		}

		private function updateAlertState():void
		{			
			if (seesPlayer())
			{
				//return to the marked behavior
				changeState(POL_BEHAVIOR_MARKED);
			} else if (getTimer() > lastBehaviorChangeTime + 3000)
			{
				changeState(POL_BEHAVIOR_PATROL);
			} else {
				this.moveDelta(this.speed * (2*FlxG.random() - 1), this.speed * (2*FlxG.random()-1));
			}
		}
		
	}
}
