using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Characters {

    public class Character {

        private string name;
        private string description;
        private bool isMale;

        private List<string> personalAttributes;
        private Dictionary<string, List<string>> socialAttributes;
        private Dictionary<string, Dictionary<string, int>> branchAttributes;
        private CharacterLocations locations;

        

        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public List<string> PersonalAttributes { get => personalAttributes; set => personalAttributes = value; }
        public Dictionary<string, List<string>> SocialAttributes { get => socialAttributes; set => socialAttributes = value; }
        public bool IsMale { get => isMale; }
        public Dictionary<string, Dictionary<string, int>> BranchAttributes { get => branchAttributes; set => branchAttributes = value; }
        public CharacterLocations Locations { get => locations; set => locations = value; }

        public Character(string name, bool isMale) {
            Name = name;
            this.isMale = isMale;
            Locations = new CharacterLocations();
        }

        public void Talk() {
            Console.WriteLine("Hello from " + name);
        }


        
    }

    


}
