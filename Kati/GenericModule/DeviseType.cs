using Kati.SourceFiles;

namespace Kati.GenericModule {
    /// <summary>
    /// decides if the topic will be a statement or a question
    /// </summary>
    public class DeviseType {

        private double defaultStatementWeight = 80;
        private double defaultQuestionWeight = 20;
        private string type;


        public DeviseType() {
            Type = Constants.STATEMENT;
        }

        public string Type { get => type; set => type = value; }

        /****************************Type based on Weighted Probability******************************/

        public void SetWeights(double statement, double question) {
            defaultStatementWeight = statement;
            defaultQuestionWeight = question;
        }

        public string GetTopicType() {
            double total = defaultStatementWeight + defaultQuestionWeight;
            double choice = Controller.dice.NextDouble() * total;
            if (choice <= defaultStatementWeight) {
                Type = Constants.STATEMENT;
            } else {
                Type = Constants.QUESTION;
            }
            return Type;
        }

        /****************************************Force Type*******************************************/

        public void ForceType(string type) {
            Type = type;
        }
    }
}
