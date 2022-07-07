using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neural_net_test
{
	class Generation
	{
		public Generation( bool generateNets = true )
		{
			if( !generateNets ) return;

			for( int i = 0; i < genSize; ++i )
			{
				nets.Add( new NeuralNet() );
			}
		}

		// Create next gen, all are randomized but best stays the same.
		public Generation CreateNextGen( int best )
		{
			Generation next = new Generation( false );
			next.nets.Add( nets[best] );
			for( int i = 0; i < genSize - 1; ++i )
			{
				next.nets.Add( nets[best].GenerateChild( variation ) );
			}
			return( next );
		}

		public List<List<float>> ForwardProp( List<float> data )
		{
			var results = new List<List<float>>();
			foreach( var net in nets )
			{
				results.Add( net.ForwardProp( data ) );
			}
			return( results );
		}

		// return top performer
		public int ForwardPropTest( List<List<float>> data,List<float> answers )
		{
			var errors = new List<float>();
			for( int i = 0; i < nets.Count; ++i )
			{
				float totalError = 0.0f;
				for( int j = 0; j < data.Count; ++j )
				{
					var results = nets[i].ForwardProp( data[j] );
					totalError += Math.Abs( results[0] - answers[j] );
				}
				errors.Add( totalError );
			}

			int lowestSpot = 0;
			float lowestVal = 9999.0f;
			for( int i = 0; i < errors.Count; ++i )
			{
				if( errors[i] < lowestVal )
				{
					lowestVal = errors[i];
					lowestSpot = i;
				}
			}

			return( lowestSpot );
		}

		const int genSize = 15;
		const float variation = 0.1f; // Genetic variation between generations.
		public List<NeuralNet> nets = new List<NeuralNet>();
	}
}
