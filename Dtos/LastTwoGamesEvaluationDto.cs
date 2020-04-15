namespace FourInARow.Dtos
{
    public class LastTwoGamesEvaluationDto
    {
        private readonly int lastEval;
        private readonly int penultimateEval;
        public LastTwoGamesEvaluationDto(int lastEval, int penultimateEval)
        {
            this.lastEval = lastEval;
            this.penultimateEval = penultimateEval;
        }

        public LastTwoGamesEvaluationDto(LastTwoGamesEvaluationDto lastTwoGamesEvaluationDto,int lastEval)
        {
            this.lastEval = lastEval;
            penultimateEval = lastTwoGamesEvaluationDto.lastEval;
        }

        public int GetPenultimateGameEvaluation()
        {
            return penultimateEval;
        }
        public int GetLastGameEvaluation()
        {
            return lastEval;
        }
    }
}