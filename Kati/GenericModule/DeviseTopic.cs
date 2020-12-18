using System;
using System.Collections.Generic;

namespace Kati.GenericModule {

    /// <summary>
    /// Class decides which topic to use for the conversation
    /// </summary>
    public class DeviseTopic {

        private Dictionary<string, double> topicWeights;
        private string topic;
        private readonly double baseValue = 20;

        public DeviseTopic(List<string> topicKeys) {
            TopicWeights = new Dictionary<string, double>();
            foreach (string s in topicKeys) {
                TopicWeights[s] = baseValue;
            }
        }

        public Dictionary<string, double> TopicWeights { get => topicWeights; set => topicWeights = value; }
        public string Topic { get => topic; set => topic = value; }

        //consider specifics from the Module hub
        //consider specifics from the Module itself
        /**
         * n number of topics with statement, question response
         * n        
         */

        /*****************************Weighted Probablitiy Decision************************************/
        public void ResetWeights() {
            foreach (KeyValuePair<string, double> item in topicWeights) {
                topicWeights[item.Key] = baseValue;
            }
        }

        public void SetMultiWeights(Dictionary<string, double> delta) {
            foreach (KeyValuePair<string, double> item in delta) {
                if(topicWeights.ContainsKey(item.Key))
                    topicWeights[item.Key] = delta[item.Key];
            }
        }

        public void SetSingleTopicWeight(string topic, double weight) {
            if (topicWeights.ContainsKey(topic))
                TopicWeights[topic] = weight;
        }

        //optional forcedTopics: limit possible topics to choose from
        public string GetTopic(List<string> forcedTopics = null) {
            double total = 0;
            List<Dictionary<string, double>> calc = new List<Dictionary<string, double>>();
            total = GetProbabilityTotal(forcedTopics, total, ref calc);
            double option = Controller.dice.NextDouble() * total;
            for (int i = 0; i < calc.Count; i++) {
                foreach (KeyValuePair<string, double> item in calc[i]) {
                    if (option <= calc[i][item.Key]) {
                        Topic = item.Key;
                        return topic;
                    }
                }
            }
            return GetTopic();
        }

        protected double GetProbabilityTotal
            (List<string> forcedTopics, double total, ref List<Dictionary<string, double>> calc) {
            if (forcedTopics == null) {
                total = GetStandardTotal(total, calc);
            } else {
                total = GetForcedTotal(forcedTopics, total, calc);
            }
            return total;
        }

        protected double GetForcedTotal
            (List<string> forcedTopics, double total, List<Dictionary<string, double>> calc) {
            int i = 0;
            foreach (string s in forcedTopics) {
                if (TopicWeights.ContainsKey(s)) {
                    total += TopicWeights[s];
                    calc.Add(new Dictionary<string, double>());
                    calc[i][s] = total;
                    i++;
                }
            }
            return total;
        }

        protected double GetStandardTotal(double total, List<Dictionary<string, double>> calc) {
            int i = 0;
            foreach (KeyValuePair<string, double> item in topicWeights) {
                total += topicWeights[item.Key];
                calc.Add(new Dictionary<string, double>());
                calc[0][item.Key] = total;
                i++;
            }
            return total;
        }

        /**************************************Forced Decision****************************************/
        public void ForcedTopic(string topic) {
            if (TopicWeights.ContainsKey(topic))
                Topic = topic;
            else
                Topic = GetTopic();
        }
    }
}
