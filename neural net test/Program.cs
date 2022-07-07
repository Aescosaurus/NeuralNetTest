using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace neural_net_test
{
	class Program
	{
		static void Main( string[] args )
		{
			var data = new List<List<float>>
			{
				new List<float> { 0.0f,0.0f },
				new List<float> { 1.0f,0.0f },
				new List<float> { 0.0f,1.0f },
				new List<float> { 1.0f,1.0f }
			};
			var answers = new List<float>()
			{
				0.0f,1.0f,1.0f,0.0f
			};

			Console.WriteLine( "running tests" );

			var curGen = new Generation();
			const int nTests = 150000;
			var rand = new Random();
			NeuralNet winningNet = null;
			for( int i = 0; i < nTests; ++i )
			{
				var best = curGen.ForwardPropTest( data,answers );
				if( i == nTests - 1 ) winningNet = curGen.nets[best];
				else curGen = curGen.CreateNextGen( best );
			}

			Debug.Assert( winningNet != null );

			Console.WriteLine( "testing results" );
			
			for( int i = 0; i < data.Count; ++i )
			{
				var results = winningNet.ForwardProp( data[i] );
			
				var score = Math.Abs( results[0] - answers[i] );
				Console.WriteLine( "prediction: " + results[0] + " actual: " + answers[i] +
					" score: " + ( ( int )( score * 100.0f ) ) );
			}
			
			Console.WriteLine( "print layers" );
			
			winningNet.PrintLayers();

			Console.WriteLine( "done" );

			Console.Read();
			Console.Read();
		}
	}
}
