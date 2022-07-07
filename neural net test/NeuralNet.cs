using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace neural_net_test
{
	class NeuralNet
	{
		class Layer
		{
			// create new random layer
			public Layer( int width,int height,float min = -0.5f,float max = 0.5f )
			{
				for( int i = 0; i < width; ++i )
				{
					weights.Add( new List<float>() );
					for( int j = 0; j < height; ++j )
					{
						weights[i].Add( FakeRand.RangeF( min,max ) );
					}
					biases.Add( FakeRand.RangeF( min,max ) );
				}
			}

			// Copy weights & biases & deviate each one.
			private Layer( List<List<float>> weights,List<float> biases,float dev )
			{
				foreach( var weightList in weights )
				{
					this.weights.Add( new List<float>() );
					foreach( var weight in weightList )
					{
						// this.weights[this.weights.Count - 1].Add( weight + ( FakeRand.Rand() - 0.5f ) * dev );
						this.weights[this.weights.Count - 1].Add( weight );
					}
				}

				foreach( var bias in biases )
				{
					// this.biases.Add( bias + ( FakeRand.Rand() - 0.5f ) * dev );
					this.biases.Add( bias );
				}

				// only randomize 1 of each
				weights[FakeRand.RangeI( 0,weights.Count )][FakeRand.RangeI( 0,weights[0].Count )] += ( FakeRand.Rand() - 0.5f ) * dev;
				biases[FakeRand.RangeI( 0,biases.Count )] += ( FakeRand.Rand() - 0.5f ) * dev;
			}

			public List<float> ForwardProp( List<float> data )
			{
				// Console.WriteLine( "forward prop" );
				var results = new List<float>();

				// Console.Write( "inputs " );
				// foreach( var x in data )
				// {
				// 	Console.Write( x + ", " );
				// }
				// Console.WriteLine();
				var nOutputs = weights[0].Count;
				for( int i = 0; i < nOutputs; ++i )
				{
					float total = 0.0f;
					for( int j = 0; j < data.Count; ++j )
					{
						total += weights[j][i] * data[j];
					}
					total += biases[i];
					results.Add( total / ( 1 + Math.Abs( total ) ) ); // fake sigmoid
				}

				Debug.Assert( results.Count == nOutputs );
				// Console.Write( "results " );
				// foreach( var x in results )
				// {
				// 	Console.Write( x + ", " );
				// }
				// Console.WriteLine();

				return( results );
			}

			public Layer GenerateChild( float dev )
			{
				return( new Layer( weights,biases,dev ) );
			}

			public List<List<float>> weights = new List<List<float>>();
			public List<float> biases = new List<float>();
		}

		public NeuralNet( bool generateLayers = true )
		{
			if( !generateLayers ) return;

			layers.Add( new Layer( nInputs,nHidden ) );
			for( int i = 0; i < nLayers - 2 - 1; ++i ) // adding input/output layer separately, 1 fewer than # of nodes
			{
				layers.Add( new Layer( nHidden,nHidden ) );
			}
			layers.Add( new Layer( nHidden,nOutputs ) );
		}

		// returns list with size of nOutputs
		public List<float> ForwardProp( List<float> data )
		{
			List<float> results = data;
			foreach( var layer in layers )
			{
				results = layer.ForwardProp( results );
			}
			return( results );
		}

		public NeuralNet GenerateChild( float variation )
		{
			var child = new NeuralNet( false );
			foreach( var layer in layers )
			{
				child.layers.Add( layer.GenerateChild( variation ) );
			}
			return( child );
		}

		public void PrintLayers()
		{
			// just print vertically 
			Console.WriteLine( layers.Count );
			foreach( var layer in layers )
			{
				foreach( var weights in layer.weights )
				{
					foreach( var weight in weights )
					{
						Console.Write( weight + ", " );
					}
				}
				Console.WriteLine();
			}
		}

		const int nLayers = 3; // includes input & output layers
		const int nHidden = 2; // how many hidden layers, applies to each hidden layer, like vertically how many
		const int nInputs = 2;
		const int nOutputs = 1;
		List<Layer> layers = new List<Layer>();
	}
}
