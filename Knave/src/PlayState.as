package {
	import Actors.Police;
	import Actors.Agent;
	import Actors.Player;
	
	import org.flixel.*;
	
	public class PlayState extends FlxState

	{
		[Embed(source="data/map.png")] private var ImgMap:Class;
		[Embed(source="data/tiles.png")] private var ImgTiles:Class;
		
	
		
		override public function create():void
		{
			add(new FlxText(0,0,100,"Knave!")); //adds a 100px wide text field at position 0,0 (top left)
			
			// generate map
			Registry.map = new FlxTilemap();
			Registry.map.loadMap(FlxTilemap.imageToCSV(this.ImgMap, false, 8), this.ImgTiles, 0, 0, FlxTilemap.OFF, 0, 0, 1);
			add(Registry.map);
			
			// spawn players
			Registry.p1 = new Player(260,260);
			Registry.p1.setPlayer(0);
			add(Registry.p1);
			
			//Registry.p2 = new Player(300,200);
			//Registry.p2.setPlayer(1);
			//add(Registry.p2);
			
			Registry.police1 = new Police(360,260);
			add(Registry.police1);
		
		}
		
		override public function update():void
		{
			super.update();
			FlxG.collide();
		}
	}
}