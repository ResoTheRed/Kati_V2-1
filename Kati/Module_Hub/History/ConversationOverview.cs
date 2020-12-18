using System.Collections.Generic;

namespace Kati.Module_Hub.History {
    /// <summary>
    /// Encapsulate Longterm history
    /// </summary>

    public class ConversationOverview {
        //tone average values
        public const string ROMANTIC = "Romantic"; //romance
        public const string FRIENDLY = "Friendly"; //friend
        public const string RESPECTFUL = "Respectful"; //respect or professional
        public const string ADMIRATION = "Admiration"; //affinity
        public const string HATEFUL = "Hateful"; //hate
        public const string DISDAIN = "Disdain"; //disgust
        public const string COMPETITIVE = "Competitive"; //rivalry
        public const string SUGGESTIVE = "Suggestive"; //Romance && Friend
        public const string CHEERFUL = "Cheerful"; //friend and respect or professional
        public const string LOATHING = "Loathing"; //Hate and Disgust
        public const string POSITIVE = "Positive"; //any positve tie not mentioned yet
        public const string NEGATIVE = "Negative"; //any negative tie not mentioned yet
        public const string CAUSAL = "Casual"; //neutral tone
        public const string EMOTIONAL = "Emotional"; //negative and positive tie

        private string storyNode;
        private string locationNode;
        private (string, string) characterNodes;
        private string toneAverage;
        private List<string> topics;

        public string StoryNode { get => storyNode; set => storyNode = value; }
        public string LocationNode { get => locationNode; set => locationNode = value; }
        public (string, string) CharacterNodes { get => characterNodes; set => characterNodes = value; }
        public string ToneAverage { get => toneAverage; set => toneAverage = value; }
        public List<string> Topics { get => topics; set => topics = value; }

        public ConversationOverview(string story, string location, string character1, string character2) {
            StoryNode = story;
            LocationNode = location;
            CharacterNodes = (character1, character2);
        }

        //@param: dict { tone : times it appears in conversation} 
        public void EstablishToneAverage(Dictionary<string, int> bucket) {
            List<string> top = ParseToneBucket(bucket);
            top = GetAllPossibleToneAverageValues(top);
            ToneAverage = GetToneAverage(top);
        }

        //get all tones that occur the most in the toneBucket
        virtual protected List<string> ParseToneBucket(Dictionary<string, int> bucket) {
            int max = 0;
            List<string> top = new List<string>();
            foreach (KeyValuePair<string, int> item in bucket) {
                if (item.Value > max) {
                    max = item.Value;
                }
            }
            foreach (KeyValuePair<string, int> item in bucket) {
                if (item.Value == max) {
                    top.Add(item.Key);
                }
            }
            return top;
        }

        virtual protected List<string> GetAllPossibleToneAverageValues(List<string> tones) {
            List<string> avg = new List<string>();
            if (tones.Count == 0) {
                avg.Add(CAUSAL);
                return avg;
            }
            if (tones.Count == 8) {
                avg.Add(EMOTIONAL);
                return avg;
            }
            if (tones.Contains(Constants.ROMANCE))
                avg.Add(ROMANTIC);
            if (tones.Contains(Constants.FRIEND))
                avg.Add(FRIENDLY);
            if (tones.Contains(Constants.RESPECT) || tones.Contains(Constants.PROFESSIONAL))
                avg.Add(RESPECTFUL);
            if (tones.Contains(Constants.AFFINITY))
                avg.Add(ADMIRATION);
            if (tones.Contains(Constants.HATE))
                avg.Add(HATEFUL);
            if (tones.Contains(Constants.DISGUST))
                avg.Add(DISDAIN);
            if (tones.Contains(Constants.RIVALRY))
                avg.Add(COMPETITIVE);
            if (tones.Contains(Constants.ROMANCE) && tones.Contains(Constants.FRIEND) || tones.Contains(Constants.AFFINITY))
                avg.Add(SUGGESTIVE);
            if (tones.Contains(Constants.FRIEND) && tones.Contains(Constants.RESPECT) || tones.Contains(Constants.PROFESSIONAL))
                avg.Add(CHEERFUL);
            if (tones.Contains(Constants.HATE) && tones.Contains(Constants.DISGUST))
                avg.Add(LOATHING);
            if (tones.Contains(Constants.HATE) && tones.Contains(Constants.DISGUST) && tones.Contains(Constants.RIVALRY))
                avg.Add(NEGATIVE);
            if (tones.Contains(Constants.ROMANCE) && tones.Contains(Constants.FRIEND) && tones.Contains(Constants.PROFESSIONAL) || tones.Contains(Constants.RESPECT) || tones.Contains(Constants.AFFINITY))
                avg.Add(POSITIVE);
            return avg;
        }

        virtual protected string GetToneAverage(List<string> avgs) {
            if (avgs.Count == 1)
                return avgs[0];
            if (avgs.Contains(NEGATIVE) && avgs.Contains(POSITIVE))
                return EMOTIONAL;
            if (avgs.Contains(POSITIVE))
                return POSITIVE;
            if (avgs.Contains(NEGATIVE))
                return NEGATIVE;
            if (avgs.Contains(SUGGESTIVE))
                return SUGGESTIVE;
            if (avgs.Contains(CHEERFUL))
                return CHEERFUL;
            if (avgs.Contains(LOATHING))
                return LOATHING;
            if (avgs.Contains(ROMANTIC))
                return ROMANTIC;
            if (avgs.Contains(FRIENDLY))
                return FRIENDLY;
            if (avgs.Contains(RESPECTFUL))
                return RESPECTFUL;
            if (avgs.Contains(ADMIRATION))
                return ADMIRATION;
            if (avgs.Contains(DISDAIN))
                return DISDAIN;
            if (avgs.Contains(HATEFUL))
                return HATEFUL;
            return CAUSAL;
        }

    }

}
