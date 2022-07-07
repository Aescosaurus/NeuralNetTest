using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neural_net_test
{
	class FakeRand
	{
		public static float Rand()
		{
			if( !init )
			{
				init = true;
				var rand = new Random();
				seed = rand.Next();
			}
			var val = Math.Sin( seed++ ) * 10000;
			return( ( float )( val - Math.Floor( val ) ) );
		}

		public static int RangeI( int min,int max )
		{
			return( ( int )( RangeF( ( float )min,( float )max ) - 0.00001f ) );
		}

		public static float RangeF( float min,float max )
		{
			return ( ( Rand() * ( max - min ) ) + min );
		}
		
		static int seed = 0;
		static bool init = false;
	}
}
