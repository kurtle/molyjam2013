package Actors {
	import flash.utils.getTimer;
	import Actors.Agent;

	/**
	 * @author ajaffe
	 */
	public class Police extends Agent {

		public static const POL_BEHAVIOR_PATROL:String = "PATROL";
		public static const POL_BEHAVIOR_MARKED:String = "MARKED";
		public static const POL_BEHAVIOR_ALERT:String = "ALERT";
		
		public function Police(X : Number = 0, Y : Number = 0, SimpleGraphic : Class = null) {
			super(X, Y, SimpleGraphic);
			
			changeState(POL_BEHAVIOR_PATROL);
		}
		
		override public function update():void {
			if (behaviorState == POL_BEHAVIOR_PATROL)
			{
				if (seesPlayer())
				{
					changeState(POL_BEHAVIOR_MARKED);
				}
			}
			
			if (behaviorState == POL_BEHAVIOR_MARKED)
			{
				if (seesPlayer())
				{
					//re-up the marked behavior
					changeState(POL_BEHAVIOR_MARKED);
				} 
				
				if (getTimer() > lastBehaviorChangeTime + 3000)
				{
					changeState(POL_BEHAVIOR_ALERT);
				}
			}
			
			if (behaviorState == POL_BEHAVIOR_ALERT)
			{
				
				if (seesPlayer())
				{
					//return to the marked behavior
					changeState(POL_BEHAVIOR_MARKED);
				} 
				
				if (getTimer() > lastBehaviorChangeTime + 3000)
				{
					changeState(POL_BEHAVIOR_PATROL);
				}
			}
		}

		
	}
}
