using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Characters {

    struct ModuleNames {
        public const string AROUND_TOWN = "Around_Town";
        public const string LERIN = "Lerin";
        public const string QUESTING = "Questing";
    }

    struct Areas {
        public const string TOWN = "Village of Biggs";
        public const string FOREST = "Gringor's Forest";
        public const string CAVE = "Crystal Cave";
    }

    struct Cast {
        public const string COLLIN = "Collin";
        public const string BENJAMIN = "Benjamin";
        public const string DAN = "Dan";
        public const string ALBRECHT = "Albrecht";
        public const string ROMERO = "Romero";
        public const string MICIAH = "Miciah";
        public const string SIVIAN = "Sivian";

        public const string LERIN = "Lerin";
        public const string LAFITTE = "Lafitte";
        public const string TETA = "Teta";
        public const string QUINN = "Quinn";
        public const string HELENA = "Helena";
        public const string CHRISTINA = "Christina";
        public const string CRUMPLES = "Crumples";

        public const string PLAYER = "player";
    }

    struct Personal {
        //career type attributes
        public const string ADVENTURER = "Adventurer";
        public const string BLACKSMITH = "Blacksmith";
        public const string MERCHANT = "Merchant";
        public const string FARMER = "Farmer";
        public const string BEGGAR = "Beggar";
        public const string PEASANT = "Peasant";
        public const string MAGICIAN = "Magician";
        public const string KNIGHT = "Knight";
        public const string HUNTER = "Hunter";
        public const string ROGUE = "Rogue";
        public const string ELDER = "Elder";

        public const string ROMANTIC = "Romantic";
        public const string SHY = "Shy";
        public const string LONER = "Loner";
        public const string NON_ROMANTIC = "Non Romantic";
        public const string FRIENDLY = "Friendly";
        public const string UNFRIENDLY = "Unfriendly";
        public const string SPOILED = "Spoiled";

    }

    struct Social {
        public const int VERY_HIGH_THRESHOLD = 700;
        public const int HIGH_THRESHOLD = 550;
        public const int MID_THRESHOLD = 400;
        public const int LOW_THRESHOLD = 250;
        public const int VERY_LOW_THRESHOLD = 100;

        public const string DATING = "Dating";
        public const string MARRIED = "Married";
        public const string CLOSE_TO = "Close To";
        public const string STRANGER = "Stranger";
        public const string ENEMIES = "Enemies";
        public const string IS_OWED = "Is Owed";
        public const string COLLEGUE = "Collegue";

        public const string ROMANCE = Kati.SourceFiles.Constants.ROMANCE;
        public const string FRIEND = Kati.SourceFiles.Constants.FRIEND;
        public const string PROFESSIONAL = Kati.SourceFiles.Constants.PROFESSIONAL;
        public const string AFFINITY = Kati.SourceFiles.Constants.AFFINITY;
        public const string RESPECT = Kati.SourceFiles.Constants.RESPECT;
        public const string DISGUST = Kati.SourceFiles.Constants.DISGUST;
        public const string HATE = Kati.SourceFiles.Constants.HATE;
        public const string RIVALRY = Kati.SourceFiles.Constants.RIVALRY;
    }

    public class CharacterLib {

        private Dictionary<string, Character> lib;

        public Dictionary<string, Character> Lib { get => lib; set => lib = value; }

        public CharacterLib(Player player) {
            InstantiateCharacters(player);
            SetupCharacters();
        }

        private void InstantiateCharacters(Player player) {
            Lib = new Dictionary<string, Character>();
            Lib[Cast.BENJAMIN] = new Character(Cast.BENJAMIN, true);
            Lib[Cast.COLLIN] = new Character(Cast.COLLIN, true);
            Lib[Cast.DAN] = new Character(Cast.DAN, true);
            Lib[Cast.ALBRECHT] = new Character(Cast.ALBRECHT, true);
            Lib[Cast.MICIAH] = new Character(Cast.MICIAH, true);
            Lib[Cast.ROMERO] = new Character(Cast.ROMERO, true);
            Lib[Cast.SIVIAN] = new Character(Cast.SIVIAN, true);

            Lib[Cast.LERIN] = new Character(Cast.LERIN, false);
            Lib[Cast.LAFITTE] = new Character(Cast.LAFITTE, false);
            Lib[Cast.TETA] = new Character(Cast.TETA, false);
            Lib[Cast.QUINN] = new Character(Cast.QUINN, false);
            Lib[Cast.HELENA] = new Character(Cast.HELENA, false);
            Lib[Cast.CHRISTINA] = new Character(Cast.CHRISTINA, false);

            Lib[Cast.PLAYER] = player;
        }

        private void SetupCharacters() {
            Ben(Cast.BENJAMIN);
            Collin(Cast.COLLIN);
            Dan(Cast.DAN);
            Albrecht(Cast.ALBRECHT);
            Michiah(Cast.MICIAH);
            Romero(Cast.ROMERO);
            Sivian(Cast.SIVIAN);
            Lerin(Cast.LERIN);
            Lafitte(Cast.LAFITTE);
            Teta(Cast.TETA);
            Quinn(Cast.QUINN);
            Helena(Cast.HELENA);
            Christina(Cast.CHRISTINA);
            Player(Cast.PLAYER);
        }

        private void Christina(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.ADVENTURER);
            Lib[name].PersonalAttributes.Add(Personal.FRIENDLY);
            Lib[name].PersonalAttributes.Add(Personal.PEASANT);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);
            Lib[name].Locations = new CharacterLocations();
            
            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN, Areas.FOREST }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() { 
                Location.Town.TOWN_CENTER
            });
            Lib[name].Locations.SetAreaRooms(Areas.FOREST, new List<string>() {
                Location.Forest.ENTRANCE, Location.Forest.PATH
            });
            Lib[name].Locations.ChangeRooms();

            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
        }

        private void Helena(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.LONER);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);
            
            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.STORE
            });

            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
        }

        private void Quinn(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.SPOILED);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);
            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.STORE, Location.Town.GATE
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
        }

        private void Teta(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.FARMER);
            Lib[name].PersonalAttributes.Add(Personal.PEASANT);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);

            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.STORE, Location.Town.PEASANT
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
            Lib[name].ModuleNames.Add(ModuleNames.QUESTING);
        }

        private void Lafitte(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.ROMANTIC);
            Lib[name].PersonalAttributes.Add(Personal.HUNTER);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);

            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.LAFITTE
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
        }

        private void Lerin(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.SHY);
            Lib[name].PersonalAttributes.Add(Personal.NON_ROMANTIC);
            Lib[name].PersonalAttributes.Add(Personal.MAGICIAN);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);
            
            Lib[name].Locations.SetAreaKeys(
               new List<string>() { Areas.TOWN, Areas.CAVE }
               );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.LERIN, Location.Town.SMITHY
            });

            Lib[name].Locations.SetAreaRooms(Areas.CAVE, new List<string>() {
                Location.Cave.LARGE_CHAMBER, Location.Cave.SMALL_CHAMBER
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.LERIN);
            Lib[name].ModuleNames.Add(ModuleNames.QUESTING);
        }

        private void Romero(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.UNFRIENDLY);
            Lib[name].PersonalAttributes.Add(Personal.PEASANT);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);

            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.GATE, Location.Town.PEASANT
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
        }

        private void Michiah(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.PEASANT);
            Lib[name].PersonalAttributes.Add(Personal.BEGGAR);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);

            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.GATE
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
        }

        private void Albrecht(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.MERCHANT);
            Lib[name].PersonalAttributes.Add(Personal.BLACKSMITH);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);
            
            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.ALBRECHT, Location.Town.LERIN
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
            Lib[name].ModuleNames.Add(ModuleNames.QUESTING);
        }

        private void Dan(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.MERCHANT);
            Lib[name].PersonalAttributes.Add(Personal.FRIENDLY);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);
            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.GATE, Location.Town.STORE
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
            Lib[name].ModuleNames.Add(ModuleNames.QUESTING);
        }

        private void Collin(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.ROGUE);
            Lib[name].PersonalAttributes.Add(Personal.LONER);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0, 0, 0, 0, 0, 0, 0, 0);

            Lib[name].Locations.SetAreaKeys(
                new List<string>() { Areas.TOWN, Areas.FOREST }
                );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_CENTER, Location.Town.GATE, Location.Town.PEASANT
            });

            Lib[name].Locations.SetAreaRooms(Areas.FOREST, new List<string>() {
                Location.Forest.CLEARING, Location.Forest.PATH
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
        }

        private void Ben(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.ADVENTURER);
            Lib[name].PersonalAttributes.Add(Personal.KNIGHT);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0,0,0,0,0,0,0,0 );

            Lib[name].Locations.SetAreaKeys(
               new List<string>() { Areas.TOWN, Areas.FOREST, Areas.CAVE }
               );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.GATE, Location.Town.BEN
            });

            Lib[name].Locations.SetAreaRooms(Areas.FOREST, new List<string>() {
                Location.Forest.ENTRANCE, Location.Forest.PATH
            });

            Lib[name].Locations.SetAreaRooms(Areas.CAVE, new List<string>() {
                Location.Forest.ENTRANCE
            });
            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.AROUND_TOWN);
        } 
        
        private void Sivian(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.ELDER);
            Lib[name].PersonalAttributes.Add(Personal.FRIENDLY);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].SocialAttributes[Cast.PLAYER] = new List<string>() { Social.STRANGER };
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.PLAYER, 0,0,0,0,0,0,0,0 );

            Lib[name].Locations.SetAreaKeys(
               new List<string>() { Areas.TOWN }
               );
            Lib[name].Locations.SetAreaRooms(Areas.TOWN, new List<string>() {
                Location.Town.TOWN_HALL
            });

            Lib[name].Locations.ChangeRooms();
            //Modules
            Lib[name].ModuleNames.Add(ModuleNames.QUESTING);
        }

        private void Player(string name) {
            Lib[name].PersonalAttributes = new List<string>();
            Lib[name].PersonalAttributes.Add(Personal.ADVENTURER);

            Lib[name].SocialAttributes = new Dictionary<string, List<string>>();
            Lib[name].BranchAttributes = new Dictionary<string, Dictionary<string, int>>();
            SetBranchAttributes(name, Cast.BENJAMIN, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.ALBRECHT, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.DAN, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.ROMERO, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.MICIAH, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.COLLIN, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.LERIN, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.LAFITTE, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.CHRISTINA, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.QUINN, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.TETA, 0, 0, 0, 0, 0, 0, 0, 0);
            SetBranchAttributes(name, Cast.HELENA, 0, 0, 0, 0, 0, 0, 0, 0);
            //set players current location to the village of Biggs and Town Center
            Lib[name].Locations.CurrentArea = Location.Town.NAME;
            Lib[name].Locations.CurrentRoom = Location.Town.TOWN_CENTER;

        }

        public void SetBranchAttributes(string me, string other, int romance, int friend, int prof,
            int affinity, int respect, int disgust, int hate, int rival) {
            var att = new Dictionary<string, int>();
            att[Social.ROMANCE] = romance;
            att[Social.FRIEND] = friend;
            att[Social.PROFESSIONAL] = prof;
            att[Social.AFFINITY] = affinity;
            att[Social.RESPECT] = respect;
            att[Social.DISGUST] = disgust;
            att[Social.HATE] = hate;
            att[Social.RIVALRY] = rival;
            Lib[me].BranchAttributes[other] = att;
        }

        //changes the locations of each character calls when day change happens
        public void ChangeLocations() {
            foreach (KeyValuePair<string, Character> item in Lib) {
                item.Value.Locations.ChangeRooms();
                (string area, string room) = item.Value.Locations.getLocation();
                Console.WriteLine(item.Key+" "+area + " " + room);
            }
        }
    }
}
