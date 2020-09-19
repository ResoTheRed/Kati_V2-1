using System;
using System.Collections.Generic;
using System.Text;

namespace Kati.Module_Hub {

    /// <summary>
    /// Contains all of the character data from the game
    /// </summary>
    public class CharacterData {
        //singleton
        private static CharacterData c_data;

        private static void _CharacterData() {
            if (c_data == null) {
                c_data = new CharacterData();
            }
        }

        public static CharacterData GetCharacterData() {
            _CharacterData();
            return c_data;
        }

        //use every dialogue transition
        public static void SetInitiatorCharacterData
            (string initiatorsName, string initGender,
            Dictionary<string, double> initiatorsTone,
            Dictionary<string, string> initiatorPersonalList,
            Dictionary<string, Dictionary<string, string>> initiatorSocialList) {
            c_data.initiatorsName = initiatorsName;
            c_data.initialorsGender = initGender;
            c_data.initiatorsTone = initiatorsTone;
            c_data.initiatorPersonalList = initiatorPersonalList;
            c_data.initiatorSocialList = initiatorSocialList;
        }

        public static void SetResponderCharacterData
            (string name, string gender, Dictionary<string, string> personal) {
            c_data.respondersName = name;
            c_data.respondersGender = gender;
            c_data.responderpersonalList = personal;
        }

        private string initiatorsName;
        private string respondersName;
        private string initialorsGender;
        private string respondersGender;
        /*init/respond feelings toward or outlook on th econversation 8 attributes with 8 numbers 0-1000*/
        private Dictionary<string, double> initiatorsTone;
        /*Collections of character's boolean attributes-> format: "lucky" : "characterTrait"/or"120" */
        private Dictionary<string, string> initiatorPersonalList;
        private Dictionary<string, string> responderpersonalList;
        /*collection of every social trait Format: npc_name : {trait_name : trait_type or value}*/
        private Dictionary<string, Dictionary<string, string>> initiatorSocialList;

        private CharacterData() {
            initiatorsTone = new Dictionary<string, double>();
            initiatorPersonalList = new Dictionary<string, string>();
            responderpersonalList = new Dictionary<string, string>();
            initiatorSocialList = new Dictionary<string, Dictionary<string, string>>();
        }

        //Only getters, Must be set through the hub by the game itself
        //Modules should not beable to alter contents
        public string InitiatorsName { get => initiatorsName; }
        public string RespondersName { get => respondersName; }
        public string InitialorsGender { get => initialorsGender; }
        public string RespondersGender { get => respondersGender; }
        public Dictionary<string, double> InitiatorsTone { get => initiatorsTone; }
        public Dictionary<string, string> InitiatorPersonalList { get => initiatorPersonalList; }
        public Dictionary<string, string> ResponderAttributeList { get => responderpersonalList; }
        public Dictionary<string, Dictionary<string, string>> InitiatorSocialList { get => initiatorSocialList; }

    }

}
