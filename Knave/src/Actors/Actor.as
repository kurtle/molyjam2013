package Actors
{
	import org.flixel.*;
	
	 
	
	public class Actor extends FlxSprite
	{
		[Embed(source = "../../img/knaveTest.png")]public var knaveTest:Class;
		
		protected var speed:Number = 2;
		
		public function Actor(X:Number=0, Y:Number=0, SimpleGraphic:Class=null)
		{
			super(X, Y, SimpleGraphic);
			
			loadGraphic(knaveTest, true, true, 20, 20);
			addAnimation("walking", [0, 1, 2], 30, true);
			play("walking")
		}
		
		public function moveDelta(x:int, y:int):void
		{
			this.x += x;
			this.y += y;
		}
		
	}
}