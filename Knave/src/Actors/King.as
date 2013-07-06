package Actors
{
	import org.flixel.*;

	public class King extends Agent
	{
		private var dest:FlxPoint;
		private var pointsOfInterest:Array = [new FlxPoint(260, 45),
											  new FlxPoint(585, 135),
											  new FlxPoint(500, 450),
											  new FlxPoint(130, 260),
											  new FlxPoint(100, 90)];
		
		public function King(X:Number=0, Y:Number=0, SimpleGraphic:Class=null)
		{
			super(X, Y, SimpleGraphic);
			
			this.speed = 12;
		}
		
		override public function update():void
		{
			super.update();
			
			if (dest == null)
			{
				// pick new destination
				dest = this.pointsOfInterest[Math.floor(Math.random()*(this.pointsOfInterest.length))];
				var path:FlxPath = Registry.map.findPath(this.getMidpoint(), dest);
				this.followPath(path, this.speed);
			}
			
			var currentPos:FlxPoint = this.getMidpoint();
			if (currentPos.x - dest.x < 0.1 && currentPos.y - dest.y < 0.1)
			{
				dest = null;
				this.stopFollowingPath();
			}
		}
	}
}