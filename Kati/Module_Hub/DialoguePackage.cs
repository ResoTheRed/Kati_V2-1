using System;
using System.Collections.Generic;
using System.Text;

namespace Kati.Module_Hub {
    /// <summary>
    /// Stat that the current module is at after each dialogue retrieval
    /// RETURN: must return to the active module
    /// CONTINUE: can return or exit active module
    /// EXIT: must leave current Module
    /// </summary>
    public enum ModuleStatus{ 
        RETURN,
        CONTINUE,
        EXIT
    }
    /// <summary>
    /// Class provides a way to package data from each module to the hub and untimately to the game
    /// </summary>
    public class DialoguePackage {

        private static DialoguePackage package;

        public static DialoguePackage Package() {
            if (package == null)
                package = new DialoguePackage();
            return package;
        }

        public static void Destroy() {
            package = null;
        }

        //****************Game data********************//
        //the purpose for the entire Module System
        private string dialogue;

        //**************module hub data****************//
        private ModuleStatus status;
        //points to next module directed by parser: "lead to"
        private string nextModule;

        //**************controller data****************//
        //dream : 200
        private Dictionary<string, double> forcedTopic;
        private Dictionary<string, double> forcedType;

        public DialoguePackage() {
            status = ModuleStatus.CONTINUE;
            ForcedTopic = new Dictionary<string, double>();
            ForcedType = new Dictionary<string, double>();
        }

        public string Dialogue { get => dialogue; set => dialogue = value; }
        public ModuleStatus Status { get => status; set => status = value; }
        public string NextModule { get => nextModule; set => nextModule = value; }
        public Dictionary<string, double> ForcedTopic { get => forcedTopic; set => forcedTopic = value; }
        public Dictionary<string, double> ForcedType { get => forcedType; set => forcedType = value; }

        public void AddForcedTopic(string topic, double weight) {
            forcedTopic[topic] = weight;
        }        
        
        public void AddForcedType(string topic, double weight) {
            forcedType[topic] = weight;
        }
    }



}
