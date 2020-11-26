using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Characters {
    public class Character {

        private string name;
        private string description;
        private bool isMale;

        private List<string> personalAttributes;
        private List<string> socialAttributes;

        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public List<string> PersonalAttributes { get => personalAttributes; set => personalAttributes = value; }
        public List<string> SocialAttributes { get => socialAttributes; set => socialAttributes = value; }
        public bool IsMale { get => isMale; }

        public Character(string name, bool isMale) {
            Name = name;
            isMale = isMale;
        }
    }
}
