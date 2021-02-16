using System.Collections.Generic;

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
        private string tone;
        //character names
        private string speaker;
        private string responder;

        //**************controller data****************//
        //dream : 200
        private Dictionary<string, double> forcedTopic;
        private Dictionary<string, double> forcedType;

        //*************Dialogue Chaining***************//
        private bool isChain;
        private bool isReponse;
        private string nextModule;
        private string nextTopic;
        private string nextType;
        private string nextTone;
        private string nextReq;

        

        //**************module hub data****************//
        private ModuleStatus status;
        //points to next module directed by parser: "lead to"
        //private string nextModule;

        //**************history data******************//
        //True if a new conversation is started
        private bool newConversation;
        //current time stamp of when the conversation started
        private int timeStamp;
        //contains the story node used in conversation
        private string storyNode;
        //contains the location node used in conversation
        private string locationNode;

        public DialoguePackage() {
            Reset();
        }

        public void Reset() {
            status = ModuleStatus.CONTINUE;
            ForcedTopic = new Dictionary<string, double>();
            ForcedType = new Dictionary<string, double>();
            NewConversation = true;
            TimeStamp = 0;
            Req = new Dictionary<string, List<string>>();
            LeadTo = new Dictionary<string, List<string>>();
            Dialogue = "";
            Module = Topic = Type = Speaker = Responder = "";
        }

        public string Dialogue { get => dialogue; set => dialogue = value; }
        public ModuleStatus Status { get => status; set => status = value; }
        public Dictionary<string, double> ForcedTopic { get => forcedTopic; set => forcedTopic = value; }
        public Dictionary<string, double> ForcedType { get => forcedType; set => forcedType = value; }
        public Dictionary<string, List<string>> Req { get => req; set => req = value; }
        public Dictionary<string, List<string>> LeadTo { get => leadTo; set => leadTo = value; }
        public string Topic { get => topic; set => topic = value; }
        public string Type { get => type; set => type = value; }
        public string Speaker { get => speaker; set => speaker = value; }
        public string Responder { get => responder; set => responder = value; }
        public string Module { get => module; set => module = value; }
        public List<string> Response { get => response; set => response = value; }
        public bool NewConversation { get => newConversation; set => newConversation = value; }
        public int TimeStamp { get => timeStamp; set => timeStamp = value; }
        public string StoryNode { get => storyNode; set => storyNode = value; }
        public string LocationNode { get => locationNode; set => locationNode = value; }
        public string Tone { get => tone; set => tone = value; }


        public bool IsChain { get => isChain; set => isChain = value; }
        public string NextModule { get => nextModule; set => nextModule = value; }
        public string NextTopic { get => nextTopic; set => nextTopic = value; }
        public string NextType { get => nextType; set => nextType = value; }
        public string NextTone { get => nextTone; set => nextTone = value; }
        public string NextReq { get => nextReq; set => nextReq = value; }
        public bool IsResponse { get => isReponse; set => isReponse = value; }

        public void AddForcedTopic(string topic, double weight) {
            forcedTopic[topic] = weight;
        }        
        
        public void AddForcedType(string type, double weight) {
            forcedType[type] = weight;
        }

        public void SetForChain(string topic, string type, string tone, string req) {
            IsChain = true;
            IsResponse = false;
            NextTopic = topic;
            NextType = type;
            NextTone = tone; 
            NextReq = req;
        }

        public void NotAChain() {
            IsChain = false;
            NextModule = NextTopic= NextType = NextTone = NextReq ="";
        }
        
        public void SetForResponse(string topic, string type, string tone, string req) {
            IsChain = false;
            IsResponse = true;
            NextTopic = topic;
            NextType = type;
            NextTone = tone; 
            NextReq = req;
        }

        public void NotAResponse() {
            IsResponse = false;
            //NextModule = NextTopic= NextType = NextTone = NextReq ="";
        }
    }
    
}
