package {
	import org.flixel.FlxTilemap;
	
	import Actors.Police;
	import Actors.King;
	import Actors.Player;
	/**
	 * @author ajaffe
	 */
	public class Registry {
		
		public static var p1:Player;
		public static var p2:Player;
		public static var king:King;
		public static var police1:Police;
		
		public static var map:FlxTilemap;
		
		public function Registry()
		{
		}
	}
}
