package
{
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
			var p:Player = new Player(200, 300);
			p.setPlayer(0);
			add(p);
			p = new Player(400, 300);
			p.setPlayer(1);
			add(p);
		}
		
		override public function update():void
		{
			super.update();
			FlxG.collide();
		}
	}
}