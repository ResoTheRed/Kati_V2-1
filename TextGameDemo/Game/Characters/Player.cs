using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Characters {
    public class Player :Character {


        public Player() : base("Player", true) {}

        

        public void UpdateBranchAttributes(string character, Dictionary<string, int> attributes) {
            //reflects how a person feels about the player
            foreach (KeyValuePair<string, int> item in attributes) {
                BranchAttributes[character][item.Key] = attributes[item.Key];
            }
        }

        
        
    }
}
