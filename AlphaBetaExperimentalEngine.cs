using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourInARow
{
	class AlphaBetaExperimentalEngine : AlphaBetaEngine
	{
		private bool isMinLast;
		private int lastAlpha;
		private int lastBeta;
		private int nextToLastGameValue;
		private const int ChangeThreshold = 50;
		private bool isAlreadyQuintEssence;
		public AlphaBetaExperimentalEngine(int sizeX, int sizeY, int depth, bool isFirstPlayer) : base(sizeX, sizeY, depth, isFirstPlayer)
		{
		}

		protected override int eval(Game g)
		{
			//Zde nahraďte vlastním kódem
			var expectedGameResult = base.eval(g);
			var change = Math.Abs(nextToLastGameValue - expectedGameResult);
			if (change> ChangeThreshold && !isAlreadyQuintEssence)
			{
				isAlreadyQuintEssence = true;
				if (!isMinLast)
				{
					var gameValue =abSearchMax(3, lastAlpha, lastBeta);
					isAlreadyQuintEssence = false;
					return gameValue;
				}
				else
				{
					var gameValue =abSearchMin(3, lastAlpha, lastBeta);
					isAlreadyQuintEssence = false;
					return gameValue; 
				}
			}
			return expectedGameResult;
		}

		public override string getName()
		{
			//zde můžete napsat jméno svého enginu
			return "Werit " + base.getName();
		}
		private void SetState(bool isMInSearch,int alpha, int beta)
		{
			isMinLast = isMInSearch;
			lastAlpha = alpha;
			lastBeta = beta;
		}
		protected override int abSearchMax(int depth, int alpha, int beta)
        {
            if (g.hasFinished)
            {
                switch (g.winner)
                {
                    case true:
                        return 1000 + depth;
                    case false: 
                        return -1000 - depth;
                    case null:
                        return 0;
                }
            }

            int a = alpha;
            if (depth == 1 && !isAlreadyQuintEssence)
	            nextToLastGameValue = base.eval(g);
            
            if (depth <= 0)
            {
	            SetState(false, alpha, beta);
	            return eval(g);
            }

            List<int> moves = new List<int>(g.possibleMoves);
            int value, bestMove = -1, bestValue = -2000;
            foreach (var item in moves)
            {
                g.playMove(true, item);
                value = abSearchMin(depth - 1, a, beta);
                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = item;
                }
                g.undoMove(item);
                if (bestValue >= beta)
                    return bestValue;
                a = Math.Max(a, value);
            }
            return bestValue;
        }

        protected override int abSearchMin(int depth, int alpha, int beta)
        {
            if (g.hasFinished)
            {
                switch (g.winner)
                {
                    case true:
                        return 1000 + depth;
                    case false:
                        return -1000 - depth;
                    case null:
                        return 0;
                }
            }

            int b = beta;

            if (depth == 1 && !isAlreadyQuintEssence)
	            nextToLastGameValue = base.eval(g);
            
            if (depth <= 0)
            {
	            SetState(true, alpha, beta);
	            return eval(g);
            }

            List<int> moves = new List<int>(g.possibleMoves);
            int value, bestMove = -1, bestValue = 2000;
            foreach (var item in moves)
            {
                g.playMove(false, item);
                value = abSearchMax(depth - 1, alpha, b);
                if (value < bestValue)
                {
                    bestValue = value;
                    bestMove = item;
                }
                g.undoMove(item);
                if (bestValue <= alpha)
                    return bestValue;
                b = Math.Min(b, value);
            }
            return bestValue;
        }
    }
}
