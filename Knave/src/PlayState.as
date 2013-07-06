package {
	import Actors.Agent;
	import org.flixel.*;
	import Actors.Player;
	
	public class PlayState extends FlxState
	{	
		
		override public function create():void
		{
			add(new FlxText(0,0,100,"Knave!")); //adds a 100px wide text field at position 0,0 (top left)
			
			// spawn players
			Registry.p1 = new Player(100,50);
			Registry.p1.setPlayer(0);
			add(Registry.p1);
			Registry.p2 = new Player(200,50);
			Registry.p2.setPlayer(1);
			add(Registry.p2);
			
			Registry.police1 = new Agent(300,100);
			add(Registry.police1);
			
			Registry.police1.seesPlayer();
			
		}
	}
}