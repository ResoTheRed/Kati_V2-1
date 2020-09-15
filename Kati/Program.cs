using System;
using System.Collections.Generic;
using Kati.GenericModule;
using Kati.JobInterview;
using Kati.Module_Hub;
using Kati.SourceFiles;

namespace Kati{

    public class Program{

        static Game game = new Game();
        public static void Main(string[] args) {
            Console.WriteLine("Hello World");
        }


        

    }

    class Game {
        
        JobInterviewModule mod = new JobInterviewModule();
        DialoguePackage package = new DialoguePackage();

        public Game() {
            StartingDialoguePackage();
        }

        //figure out where what needs to happen next
        public void ParseLeadTo() {
            SetDefaultPackage();
            //set next topic
            //set next type
            //set requirements
        }

        private void SetDefaultPackage() {
            package.Reset();
            package.Module = "JobInterviewModule";
            package.Speaker = "Hiring Manager";
            package.Responder = "Player";
        }

        private void StartingDialoguePackage() {
            SetDefaultPackage();
            package.Topic = "welcome";
            package.Type = Constants.STATEMENT;
            package.Dialogue.Add("start");
            package.Req["start"].Add("start");
            package.LeadTo["start"].Add("start");
        }




    }
}
