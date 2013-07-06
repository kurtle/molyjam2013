package Actors
{
	import org.flixel.*;
	
	 
	
	public class Actor extends FlxSprite
	{
		
		
		protected var speed:Number = 2;
		
		public function Actor(X:Number=0, Y:Number=0, SimpleGraphic:Class=null)
		{
			super(X, Y, SimpleGraphic);
			
			
		}
		
		public function moveDelta(x:int, y:int):void
		{
			this.x += x;
			this.y += y;
		}
		
	}
}