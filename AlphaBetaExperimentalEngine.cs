using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FourInARow.Dtos;

namespace FourInARow
{
	class AlphaBetaExperimentalEngine : AlphaBetaEngine
	{
		private bool isMinLast;
		private int lastAlpha;
		private int lastBeta;
		private const int ChangeThreshold = 45;
		private const int HighThreshold = 100000;
		private bool isAlreadyQuintEssence;
		public AlphaBetaExperimentalEngine(int sizeX, int sizeY, int depth, bool isFirstPlayer) : base(sizeX, sizeY, depth, isFirstPlayer)
		{
		}
		public override int selectMove()
		{
			int move = checkWinningMoves();
			if (move >= 0) return move;

			List<int> moves = new List<int>(g.possibleMoves);
			int value, bestMove = -1, bestValue = (isFirstPlayer ? -2000 : 2000);
			if (isFirstPlayer)
			{
				foreach (var item in moves)
				{
					g.playMove(true, item);
					value = abSearchMin(depth - 1, -2000, 2000,new LastTwoGamesEvaluationDto(HighThreshold,HighThreshold));
					if (value > bestValue)
					{
						bestValue = value;
						bestMove = item;
					}
					g.undoMove(item);
				}
				return bestMove;
			}
			else
			{
				foreach (var item in moves)
				{
					g.playMove(false, item);
					value = abSearchMax(depth - 1, -2000, 2000,new LastTwoGamesEvaluationDto(HighThreshold,HighThreshold));
					if (value < bestValue)
					{
						bestValue = value;
						bestMove = item;
					}
					g.undoMove(item);
				}
				return bestMove;
			}
		}
		protected int eval(Game g,LastTwoGamesEvaluationDto gamesEvaluation)
		{
			if (isAlreadyQuintEssence) return gamesEvaluation.GetLastGameEvaluation();

			isAlreadyQuintEssence = true;
			if (!isMinLast)
			{
				var gameValue =abSearchMax(7, lastAlpha, lastBeta,gamesEvaluation);
				isAlreadyQuintEssence = false;
				return gameValue;
			}
			else
			{
				var gameValue =abSearchMin(7, lastAlpha, lastBeta,gamesEvaluation);
				isAlreadyQuintEssence = false;
				return gameValue;
			}
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
		protected int abSearchMax(int depth, int alpha, int beta,LastTwoGamesEvaluationDto lastTwoGamesEvaluationDto)
        {
	        Console.WriteLine("searching depth :{0}, alpha: {1}, beta: {2}",depth,alpha,beta);
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
            LastTwoGamesEvaluationDto thisGameEvaluation;
            if (isAlreadyQuintEssence)
            {

	            var expectedGameResult = base.eval(g);

	            var change =
		            Math.Min(
			            Math.Abs(lastTwoGamesEvaluationDto.GetPenultimateGameEvaluation() - expectedGameResult),
			            Math.Abs(lastTwoGamesEvaluationDto.GetLastGameEvaluation() - expectedGameResult)
		            );
	            if (change >= ChangeThreshold)
	            {
		            Console.WriteLine("unstable state found with change {0}",change);
		            thisGameEvaluation = new LastTwoGamesEvaluationDto(lastTwoGamesEvaluationDto,expectedGameResult);
	            }
	            else
	            {
		            return expectedGameResult;
	            }
            }
            else
            {
	            if (depth == 1 || depth == 2)
	            {
		            thisGameEvaluation = new LastTwoGamesEvaluationDto(lastTwoGamesEvaluationDto,base.eval(g));
	            }
	            else
	            {
		            thisGameEvaluation = new LastTwoGamesEvaluationDto(lastTwoGamesEvaluationDto,HighThreshold);
	            }
            }

            if (depth <= 0)
            {
	            SetState(false, alpha, beta);
	            return eval(g,thisGameEvaluation);
            }

            List<int> moves = new List<int>(g.possibleMoves);
            int value, bestMove = -1, bestValue = -2000;
            foreach (var item in moves)
            {
                g.playMove(true, item);
                value = abSearchMin(depth - 1, a, beta,thisGameEvaluation);
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

        protected int abSearchMin(int depth, int alpha, int beta,LastTwoGamesEvaluationDto lastTwoGamesEvaluationDto)
        {
	        Console.WriteLine("searching depth :{0}, alpha: {1}, beta: {2}",depth,alpha,beta);
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

            LastTwoGamesEvaluationDto thisGameEvaluation;
            if (isAlreadyQuintEssence)
            {

	            var expectedGameResult = base.eval(g);

	            var change =
		            Math.Min(
			            Math.Abs(lastTwoGamesEvaluationDto.GetPenultimateGameEvaluation() - expectedGameResult),
			            Math.Abs(lastTwoGamesEvaluationDto.GetLastGameEvaluation() - expectedGameResult)
			            );
	            if (change >= ChangeThreshold)
	            {
		            Console.WriteLine("unstable state found with change {0}",change);
		            thisGameEvaluation = new LastTwoGamesEvaluationDto(lastTwoGamesEvaluationDto,expectedGameResult);
	            }
	            else
	            {
		            return expectedGameResult;
	            }
            }
            else
            {
	            if (depth == 1 || depth == 2)
	            {
		            thisGameEvaluation = new LastTwoGamesEvaluationDto(lastTwoGamesEvaluationDto,base.eval(g));
	            }
	            else
	            {
		            thisGameEvaluation = new LastTwoGamesEvaluationDto(lastTwoGamesEvaluationDto,HighThreshold);
	            }
            }

            if (depth <= 0)
            {
	            SetState(true, alpha, beta);
	            return eval(g,thisGameEvaluation);
            }

            List<int> moves = new List<int>(g.possibleMoves);
            int value, bestMove = -1, bestValue = 2000;

            foreach (var item in moves)
            {
                g.playMove(false, item);

                value = abSearchMax(depth - 1, alpha, b,thisGameEvaluation);
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
