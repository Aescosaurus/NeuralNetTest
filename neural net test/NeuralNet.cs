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
				var rand = new Random();

				for( int i = 0; i < width; ++i )
				{
					weights.Add( new List<float>() );
					for( int j = 0; j < height; ++j )
					{
						weights[i].Add( ( ( float )rand.NextDouble() ) * ( max - min ) + min );
					}
					biases.Add( ( ( float )rand.NextDouble() ) * ( max - min ) + min );
				}
			}

			// Copy weights & biases & deviate each one.
			private Layer( List<List<float>> weights,List<float> biases,float dev )
			{
				var rand = new Random();
				foreach( var weightList in weights )
				{
					this.weights.Add( new List<float>() );
					foreach( var weight in weightList )
					{
						this.weights[this.weights.Count - 1].Add( weight + ( ( float )( rand.NextDouble() - 0.5 ) * dev ) );
					}
				}

				foreach( var bias in biases )
				{
					this.biases.Add( bias + ( float )( rand.NextDouble() - 0.5 ) * dev );
				}
			}

			public List<float> ForwardProp( List<float> data )
			{
				var results = new List<float>();

				var nOutputs = weights[0].Count;
				for( int i = 0; i < nOutputs; ++i )
				{
					float total = 0.0f;
					for( int j = 0; j < data.Count; ++j )
					{
						total += weights[j][i];
					}
					total += biases[i];
					results.Add( total / ( 1 + Math.Abs( total ) ) ); // fake sigmoid
				}

				Debug.Assert( results.Count == nOutputs );

				return( results );
			}

			public Layer GenerateChild( float dev )
			{
				return( new Layer( weights,biases,dev ) );
			}

			public List<List<float>> weights = new List<List<float>>();
			public List<float> biases = new List<float>();
		}

		public void Init()
		{
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

		public NeuralNet GenerateChild()
		{
			var child = new NeuralNet();
			foreach( var layer in layers )
			{
				child.layers.Add( layer.GenerateChild( variation ) );
			}
			return( child );
		}

		const int nLayers = 3; // includes input & output layers
		const int nHidden = 2; // how many hidden layers, applies to each hidden layer, like vertically how many
		const int nInputs = 2;
		const int nOutputs = 1;
		List<Layer> layers = new List<Layer>();
		
		const float variation = 0.1f; // Genetic variation between generations.
	}
}
