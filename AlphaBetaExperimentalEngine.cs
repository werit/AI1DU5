using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourInARow
{
	class AlphaBetaExperimentalEngine : AlphaBetaEngine
	{
		public AlphaBetaExperimentalEngine(int sizeX, int sizeY, int depth, bool isFirstPlayer) : base(sizeX, sizeY, depth, isFirstPlayer)
		{
		}

		protected override int eval(Game g)
		{
			//Zde nahraďte vlastním kódem
			return base.eval(g);
		}

		public override string getName()
		{
			//zde můžete napsat jméno svého enginu
			return "Experimental " + base.getName();
		}
    }
}
