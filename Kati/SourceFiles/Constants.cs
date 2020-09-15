

using System.Threading;

namespace Kati.SourceFiles{
    /// <summary>
    /// Class contains a collection of constant values.  It may go to json in the future
    /// the constants will be used by other classes.  so chnages made here will not break
    /// the classes that use them.
    /// </summary>
    public class Constants{
        /*collection of paths for GlobalKeys*/
        public const string pronoun = "C:/Users/User/Documents/Kati_V2-1/Kati/SourceFiles/json_files/pronoun.json";
        public const string time = "C:/Users/User/Documents/Kati_V2-1/Kati/SourceFiles/json_files/time.json";
        public const string smallTalk = "C:/Users/User/Documents/Kati_V2-1/Kati/SourceFiles/json_files/smallTalk.json";
        public const string DayDreamWonder = "C:/Users/User/Documents/Kati_V2-1/Kati/SourceFiles/json_files/DayDreamWonder.json";
        public const string TestJson = "C:/Users/User/Documents/Kati_V2-1/KatiUnitTest/Module_Tests/GlobalModuleTest/RuleTester.json";
        //Keywords
        public const string REQ = "req";
        public const string LEAD_TO = "lead to";
        public const string NOT = "not";
        public const string STATEMENT = "statement";
        public const string QUESTION = "question";
        public const string RESPONSE = "response";
        public const string INITIATOR = "initiator";
        public const string RESPONDER = "responder";
        //SocialCharacterRules
        public const string SOCIAL = "social";
        public const string NPC = "npc";
        public const string PLAYER = "player";
        public const string ATTRIBUTE = "attribute";
        public const string RELATIONSHIP = "relationship";
        public const string DIRECTED_STATUS = "directed";
        //PersonalCharacterRules 
        public const string PERSONAL = "personal";
        public const string TRAIT = "trait";
        public const string STATUS = "status";
        public const string INTEREST = "interest";
        public const string PHYSICAL_FEATURES = "physicalFeature";
        public const string SCALAR_TRAIT = "scalarTrait";
        //GameRules 
        public const string GAME = "game";
        public const string WEATHER = "weather";
        public const string SECTOR = "sector";
        public const string TIME_OF_DAY = "time";
        public const string DAY_OF_WEEK = "day";
        public const string SEASON = "season";
        public const string PUBLIC_EVENT = "publicEvent";
        public const string TRIGGER_EVENT = "trigger";
        public const string NEXT_EVENT = "next";
        public const string MON = "mon";
        public const string TUE = "tues";
        public const string WED = "weds";
        public const string THUR = "thurs";
        public const string FRI = "fri";
        public const string SAT = "sat";
        public const string SUN = "sun";
        //BranchDecision
        public const string ROMANCE = "romance";
        public const string DISGUST = "disgust";
        public const string FRIEND = "friend";
        public const string HATE = "hate";
        public const string PROFESSIONAL = "professional";
        public const string RIVALRY = "rivalry";
        public const string AFFINITY = "affinity";
        public const string RESPECT = "respect";
        public const string NEUTRAL = "neutral";
        //response values
        public const int RESPONSE_PLUS_THRESHOLD = 500;
        public const int RESPONSE_NEUTRAL_THRESHOLD = 100;
        public const string POSITIVE_PLUS = "positive+";
        public const string POSITIVE = "positive";
        public const string NEGATIVE_PLUS = "negative+";
        public const string NEGATIVE = "negative";
        public const string RESPONSE_TAG = "response_tag";

    }
}
