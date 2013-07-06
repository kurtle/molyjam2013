package
{
	import org.flixel.*;
	import Actors.Player;
	
	public class PlayState extends FlxState
	{
		override public function create():void
		{
			add(new FlxText(0,0,100,"Knave!")); //adds a 100px wide text field at position 0,0 (top left)
			
			// spawn players
			var p:Player = new Player(100, 50);
			p.setPlayer(0);
			add(p);
			p = new Player(200, 50);
			p.setPlayer(1);
			add(p);
			
		}
	}
}