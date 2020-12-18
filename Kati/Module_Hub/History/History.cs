using Kati.SourceFiles;
using System;
using System.Collections.Generic;

namespace Kati.Module_Hub.History {
    
    public class History {

        public const int ENTRY_DURATION = 14;
        public const string FILE_PATH = "SourceFiles\\";
        public const string LONG_TERM_HISTORY_FILE_NAME = "LongTermHistory.txt";
        public const string SHORT_TERM_HISTORY_FILE_NAME = "ShortTermHistory.txt";

        //short-term robust history
        private LinkedList<ConversationEntry> shortTermHistory;
        //longterm 
        private List<ConversationOverview> longTermHistory;
        private WriteToFileHistory write;
        private bool newConversation;

        public LinkedList<ConversationEntry> ShortTermHistory { get => shortTermHistory;}
        public List<ConversationOverview> LongTermHistory { get => longTermHistory;}
        public bool NewConversation { get => newConversation; set => newConversation = value; }

        public History() {
            shortTermHistory = new LinkedList<ConversationEntry>();
            longTermHistory = new List<ConversationOverview>();
            NewConversation = true;
            write = new WriteToFileHistory();
        }

        /*************************************Dialogue Package***************************************/
        public void UpdateConversationFromDialoguePackage(DialoguePackage package) {
            if (package.NewConversation) {
                CreateHistory(package.TimeStamp, package.StoryNode, package.LocationNode, package.Speaker, package.Responder);
                package.NewConversation = NewConversation = false;
            }
            AddToExistingHistory(package.Module, package.Topic, package.Type, package.Tone, package.Dialogue);
        }

        /************************************Short Term History**************************************/
        public void CreateHistory(int time, string story, string location, string character1, string character2) {
            NewConversation = false;
            ShortTermHistory.AddLast(new ConversationEntry(time, story, location, character1, character2));
        }

        public void AddToExistingHistory(string moduleName, string topic, string type, string tone, string dialogue) {
            ShortTermHistory.Last.Value.AddConversationEntry(moduleName,topic,type, tone,dialogue);
        }

        public void EndConversation() {
            NewConversation = true;
        }

        //dequeues short term history and adds it to long term history.
        public void DequeueShortTermHistory(int timeStamp) {
            if (ShortTermHistory.Count > 0) {
                var entry = ShortTermHistory.First.Value;
                if ((timeStamp - entry.TimeStamp) >= ENTRY_DURATION) {
                    ShortTermHistory.RemoveFirst();
                    var longTerm = new ConversationOverview(entry.StoryNode,entry.LocationNode,entry.CharacterNodes.Item1,entry.CharacterNodes.Item2);
                    LongTermHistory.Add(longTerm);
                    LongTermHistory[LongTermHistory.Count - 1].EstablishToneAverage(entry.GetToneBucket());
                    AddLongTermTopics(entry, longTerm);
                }
            }
        }

        /*********************************Long Term*********************************/
        private void AddLongTermTopics(ConversationEntry entry, ConversationOverview current) {
            List<string> topics = new List<string>();
            for (int i = 0; i < entry.Entry.Count; i++) {
                string topic = entry.Entry[i][1];
                if (!topics[topics.Count - 1].Equals(topic)) {
                    topics.Add(topic);
                }
            }
            current.Topics = topics;
        }

        /*****************************Write to File*********************************/
        public void WriteHistory() {
            WriteLongTermHistory();
            WriteShortTermHistory();
        }

        public void WriteShortTermHistory() {
            write.GetPathToFile(FILE_PATH,SHORT_TERM_HISTORY_FILE_NAME);
            write.WriteShortTermHistoryToFile(ShortTermHistory, true);
        }

        public void WriteLongTermHistory() {
            write.GetPathToFile(FILE_PATH,LONG_TERM_HISTORY_FILE_NAME);
            write.WriteLongTermHistoryToFile(LongTermHistory,true);
        }
    
    }

}
