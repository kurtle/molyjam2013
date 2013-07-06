package  {
	/**
	 * @author ajaffe
	 */
	public class Vec {
		
		public var x:Number;
		public var y:Number;
		
		public function Vec(X:Number=0, Y:Number=0)
		{
			x = X;
			y = Y;
		}
		
		public function magnitude():Number
		{
			return Math.sqrt(x*x + y*y);
		}
	}
}
