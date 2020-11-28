using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Characters {
    public class Player :Character {


        public Player() : base("Player", true) { }

        public void UpdateBranchAttributes(string character, Dictionary<string, int> attributes) {
            //reflects how a person feels about the player
            foreach (KeyValuePair<string, int> item in attributes) {
                BranchAttributes[character][item.Key] = attributes[item.Key];
            }
        }

        public string BranchAttributesToString() {
            string border =  "-------------------------------------------------------------------------------------------------\n";
            string heading = "Character Name | Romance | Friend | Professional | Affinity | Respect | Disgust | Hate | Rivalry \n";
            string printout = border + heading + border;
            foreach (KeyValuePair<string, Dictionary<string, int>> item in BranchAttributes) {
                string romance = BranchAttributes[item.Key][Social.ROMANCE].ToString();
                string friend = BranchAttributes[item.Key][Social.FRIEND].ToString();
                string prof = BranchAttributes[item.Key][Social.PROFESSIONAL].ToString();
                string affinity = BranchAttributes[item.Key][Social.AFFINITY].ToString();
                string respect = BranchAttributes[item.Key][Social.RESPECT].ToString();
                string disgust = BranchAttributes[item.Key][Social.DISGUST].ToString();
                string hate = BranchAttributes[item.Key][Social.HATE].ToString();
                string rivalry = BranchAttributes[item.Key][Social.RIVALRY].ToString();
                string body = $"{item.Key,-15}| {romance,-8}| {friend,-7}| {prof,-13}| {affinity,-9}| {respect,-8}| {disgust,-8}| {hate,-5}| {rivalry,-7}\n"; 
                printout += body;
            }
            printout += border;
            return printout;
        }
        
    }
}
