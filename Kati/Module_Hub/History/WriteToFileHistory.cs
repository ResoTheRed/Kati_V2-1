using System;
using System.Collections.Generic;

namespace Kati.Module_Hub.History {
    public class WriteToFileHistory {

        private string path;

        //path starts at Kati
        public string GetPathToFile(string pathFromKati, string fileName) {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] directories = path.Split("\\");
            path = "";
            for (int i = 0; i < directories.Length - 3; i++) {
                path += directories[i] + "\\";
            }
            path += pathFromKati + fileName;
            this.path = path;
            return path;
        }

        public void WriteShortTermHistoryToFile(LinkedList<ConversationEntry> history, bool append) {
            if (!append) {
                System.IO.File.Delete(path);
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true)) {
                int count = 1;
                foreach (ConversationEntry entry in history) {
                    string line = $"Story Node: {entry.StoryNode}\n Location: {entry.LocationNode}\n ";
                    line += $"Characters: {entry.CharacterNodes.Item1} && {entry.CharacterNodes.Item2}\n";
                    file.WriteLine(line);
                    for (int j = 0; j < entry.Entry.Count; j++) {
                        line += $"{j + 1}. Module: {entry.Entry[0]}, Topic: {entry.Entry[1]}, Type: {entry.Entry[2]}, Tone: {entry.Entry[3]}\n Dialogue: {entry.Entry[4]}";
                        file.WriteLine(line);
                    }
                    file.WriteLine($"End Conversation {count}\n\n");
                    count++;
                }
            }
        }

        public void WriteLongTermHistoryToFile(List<ConversationOverview> history, bool append) {
            if (!append) {
                System.IO.File.Delete(path);
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true)) {
                int count = 1;
                foreach (ConversationOverview entry in history) {
                    string line = $"Story Node: {entry.StoryNode}\n Location: {entry.LocationNode}\n ";
                    line += $"Characters: {entry.CharacterNodes.Item1} && {entry.CharacterNodes.Item2}\n";
                    line += $"Conversation Tone: {entry.ToneAverage}\n";
                    file.WriteLine(line);
                    for (int j = 0; j < entry.Topics.Count; j++) {
                        line += $"{j + 1}. Topics: {entry.Topics[j]}";
                        file.WriteLine(line);
                    }
                    file.WriteLine($"End Conversation {count}\n\n");
                    count++;
                }
            }
        }
    }
}
