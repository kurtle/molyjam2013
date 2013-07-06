package
{
	import org.flixel.*; //Allows you to refer to flixel objects in your code
	[SWF(width="640", height="480", backgroundColor="#222222")] //Set the size and color of the Flash file
	
	public class Knave extends FlxGame
	{
		public function Knave()
		{
			super(640, 480, PlayState, 1);
		}
	}
}