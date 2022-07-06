using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neural_net_test
{
	class Program
	{
		static void Main( string[] args )
		{
			List<float> data = new List<float>
			{
				0.0f,1.0f
			};
			NeuralNet test = new NeuralNet();
			test.Init();
			var results = test.ForwardProp( data );
			foreach( var result in results )
			{
				Console.Write( result );
			}

			Console.Write( "\n\ndone" );

			Console.Read();
			Console.Read();
		}
	}
}
