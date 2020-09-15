using Kati.SourceFiles;

namespace Kati.GenericModule {
    /// <summary>
    /// decides if the topic will be a statement or a question
    /// </summary>
    public class DeviseType {
        
        private const double STATEMENT_WEIGHT = 80;
        private const double QUESTION_WEIGHT = 20;
        private double currentStatementWeight = STATEMENT_WEIGHT;
        private double currentQuestionWeight = QUESTION_WEIGHT;
        private string type;


        public DeviseType() {
            Type = Constants.STATEMENT;
        }

        public string Type { get => type; set => type = value; }
        public double CurrentStatementWeight { get => currentStatementWeight; set => currentStatementWeight = value; }
        public double CurrentQuestionWeight { get => currentQuestionWeight; set => currentQuestionWeight = value; }

        /****************************Type based on Weighted Probability******************************/

        
        public void SetWeights(double? statement, double? question) {
            if(statement != null)
                CurrentStatementWeight = (double)(statement);
            if(question != null)
                CurrentQuestionWeight = (double)(question);
        }

        public string GetTopicType() {
            double total = CurrentStatementWeight + CurrentQuestionWeight;
            double choice = Controller.dice.NextDouble() * total;
            if (choice <= CurrentStatementWeight) {
                Type = Constants.STATEMENT;
            } else {
                Type = Constants.QUESTION;
            }
            return Type;
        }

        public void ResetWeights() {
            CurrentStatementWeight = STATEMENT_WEIGHT;
            CurrentQuestionWeight = QUESTION_WEIGHT;
        }

        /****************************************Force Type*******************************************/

        public void ForceType(string type) {
            Type = type;
        }
    }
}
