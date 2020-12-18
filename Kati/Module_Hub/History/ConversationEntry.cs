using System.Collections.Generic;

namespace Kati.Module_Hub.History {
    /// <summary>
    /// encapsulates a new conversation; used for short term history
    /// </summary>
    public class ConversationEntry {

        private int timeStamp;
        private string storyNode;
        private string locationNode;
        private (string, string) characterNodes;
        //module name -> module topic -> module type -> (tone, dialogue)
        List<List<string>> entry;

        public int TimeStamp { get => timeStamp; set => timeStamp = value; }
        public string StoryNode { get => storyNode; set => storyNode = value; }
        public string LocationNode { get => locationNode; set => locationNode = value; }
        public (string, string) CharacterNodes { get => characterNodes; set => characterNodes = value; }
        public List<List<string>> Entry { get => entry; set => entry = value; }

        public ConversationEntry(int time, string story, string location, string cha1, string cha2) {
            TimeStamp = time;
            StoryNode = story;
            LocationNode = location;
            CharacterNodes = (cha1, cha2);
            Entry = new List<List<string>>();
        }

        public void AddConversationEntry(string moduleName, string topic, string type, string tone, string dialogue) {
            Entry.Add(new List<string>() { moduleName, topic, type, tone, dialogue });
        }

        public Dictionary<string, int> GetToneBucket() {
            Dictionary<string, int> bucket = new Dictionary<string, int>();
            for (int i = 0; i < Entry.Count; i++) {
                if (bucket.ContainsKey(Entry[i][3])) {
                    bucket[Entry[i][3]]++;
                } else {
                    bucket[Entry[i][3]] = 1;
                }
            }
            return bucket;
        }

    }
}
