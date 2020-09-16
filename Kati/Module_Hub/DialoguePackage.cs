using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        //****************Dialogue data********************//
        //the purpose for the entire Module System
        private string module;
        
        private string dialogue;//keys for req and leadTo
        private List<string> response;
        private Dictionary<string, List<string>> req;
        private Dictionary<string, List<string>> leadTo;
        private string topic;
        private string type;
        //character names
        private string speaker;
        private string responder;

        //**************module hub data****************//
        private ModuleStatus status;
        //points to next module directed by parser: "lead to"
        private string nextModule;

        //**************controller data****************//
        //dream : 200
        private Dictionary<string, double> forcedTopic;
        private Dictionary<string, double> forcedType;

        //**************history data******************//
        //holds running total of stat effects on game and character relationships
        private double impact;
        //attribute branchs effect format -> {attribute : scalarEffectValue}
        private Dictionary<string,double> impactType;

        public DialoguePackage() {
            Reset();
        }

        public void Reset() {
            status = ModuleStatus.CONTINUE;
            ForcedTopic = new Dictionary<string, double>();
            ForcedType = new Dictionary<string, double>();
            ImpactType = new Dictionary<string, double>();
            impact = 0;
            Req = new Dictionary<string, List<string>>();
            LeadTo = new Dictionary<string, List<string>>();
            Dialogue = "";
            Module = Topic = Type = Speaker = Responder = "";
        }

        public string Dialogue { get => dialogue; set => dialogue = value; }
        public ModuleStatus Status { get => status; set => status = value; }
        public string NextModule { get => nextModule; set => nextModule = value; }
        public Dictionary<string, double> ForcedTopic { get => forcedTopic; set => forcedTopic = value; }
        public Dictionary<string, double> ForcedType { get => forcedType; set => forcedType = value; }
        public Dictionary<string, List<string>> Req { get => req; set => req = value; }
        public Dictionary<string, List<string>> LeadTo { get => leadTo; set => leadTo = value; }
        public string Topic { get => topic; set => topic = value; }
        public string Type { get => type; set => type = value; }
        public string Speaker { get => speaker; set => speaker = value; }
        public double Impact { get => impact; set => impact = value; }
        public Dictionary<string, double> ImpactType { get => impactType; set => impactType = value; }
        public string Responder { get => responder; set => responder = value; }
        public string Module { get => module; set => module = value; }
        public List<string> Response { get => response; set => response = value; }

        public void AddForcedTopic(string topic, double weight) {
            forcedTopic[topic] = weight;
        }        
        
        public void AddForcedType(string topic, double weight) {
            forcedType[topic] = weight;
        }
    }
    
}
