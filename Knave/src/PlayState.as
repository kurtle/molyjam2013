package {
	import Actors.Agent;
	import Actors.Player;
	
	import org.flixel.*;
	
	public class PlayState extends FlxState

	{
		[Embed(source="data/map.png")] private var ImgMap:Class;
		[Embed(source="data/tiles.png")] private var ImgTiles:Class;
		
		protected var map:FlxTilemap;
		
		override public function create():void
		{
			add(new FlxText(0,0,100,"Knave!")); //adds a 100px wide text field at position 0,0 (top left)
			
			// generate map
			this.map = new FlxTilemap();
			this.map.loadMap(FlxTilemap.imageToCSV(this.ImgMap, false, 8), this.ImgTiles, 0, 0, FlxTilemap.OFF, 0, 0, 1);
			add(this.map);
			
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
		
		override public function update():void
		{
			super.update();
			FlxG.collide();
		}
	}
}