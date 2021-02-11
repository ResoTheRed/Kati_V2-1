using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.TUI {

    /// <summary>
    /// Class holds static methods that display text info important to the 
    /// player.
    /// </summary>

    public class Menus {

        public const int TEXTBOXS_CAP = 35;
        public const int LARGE_TEXTBOX = 84;
        public const int SMALL_TEXTBOX = 54;
        public const int MARGINS = 1;
        public const string TEXT_BORDER_SYMBOL = "#";

        private static Menus menus;

        public static void SetupMenus(GameData gameData, Characters.CharacterLib lib) {
            if (menus == null) {
                menus = new Menus(gameData, lib);
            }
        }

        public static Menus Get() {
            return menus;
        }

        private GameData gameData;
        private Characters.CharacterLib lib;

        private Menus(GameData gameData, Characters.CharacterLib lib) {
            this.gameData = gameData;
            this.lib = lib;
        }

        public void InventoryLoop() {
             
            int option = 5;
            do {
                InventoryMenu();
                try {
                    option = Int32.Parse(Console.ReadLine());
                } catch (Exception) {
                    Console.WriteLine("Enter a number 1 - 4.");
                    continue;
                }
                if (option == 1) {
                    ItemDisplay();
                } else if (option == 2) {
                    CharactersStatsDisplay();
                } else if (option == 3) {
                    WeatherAndTimeDisplay();
                } else if (option == 4) {
                    PlayersBranchAttributesDisplay();
                } else if (option != 5) {
                    Console.WriteLine("Enter a number 1 - 4.");
                }
            } while (option != 5);
        }

        public string WeatherAndTimeDisplay() {
            string timeOfDay = gameData.CheckTimeOfDay(Timer.Get().Moves) ? "Night" : "Day";
            (string area, string room) = lib.Lib[Characters.Cast.PLAYER].Locations.GetLocation();
            string money = Characters.PlayersStats.Get().Money.ToString();
            string[] graphics = GetWeatherAndTimeGraphics();
            string border = "=============================================================\n";
            string line1 =  "Moves Today | " + $"{Timer.Get().Moves,-25} |"+graphics[0]+"\n";
            string line2 =  "Total Moves | " + $"{Timer.Get().TotalMoves,-25} |" + graphics[1] + "\n";
            string line3 =  "Days Past   | " + $"{Timer.Get().DaysPast,-25} |" + graphics[2] + "\n";
            string line4 =  "Time of Day | " + $"{timeOfDay, -25} |" + graphics[3] + "\n";
            string line5 =  "Weather     | " + $"{gameData.Weather,-25} |" + graphics[4] + "\n";
            string line6 =  "Area        | " + $"{area,-25} |" + graphics[5] + "\n";
            string line7 =  "Room        | " + $"{room,-25} |" + graphics[6] + "\n";
            string body =  border+line1+line2+line3+line4+line5+line6+line7+border;
            Console.WriteLine(body);
            return body;
        }

        public string CharactersStatsDisplay() {
            var stats = Characters.PlayersStats.Get();
            string aa = "|---| ";
            string[] graphics = Graphics.Fighter();
            string border = "=========================================================\n";
            string l1 = aa + "Name    | " + $"{stats.Name,-10} |" + graphics[0]+"\n";
            string l2 = aa + "Job     | " + $"{stats.Job,-10} |" + graphics[1] + "\n";
            string b3 = aa + "=====================|" + graphics[2] + "\n";
            string l4 = aa + "HP      | " + $"{stats.MaxHp,-3}:{stats.Hp,-3}    |" + graphics[3] + "\n";
            string l5 = aa + "Mana    | " + $"{stats.MaxMana,-3}:{stats.Mana,-3}    |" + graphics[4] + "\n";
            string l6 = aa + "Attack  | " + $"{stats.Attack,-10} |" + graphics[5] + "\n";
            string l7 = aa + "Magic   | " + $"{stats.Magic,-10} |" + graphics[6] + "\n";
            string l8 = aa + "Dodge   | " + $"{stats.Dodge,-10} |" + graphics[7] + "\n";
            string l9 = aa + "Accuracy| " + $"{stats.Accuracy,-10} |" + graphics[8] + "\n";
            string l10 = aa + "Stamina | " + $"{stats.Stamina,-10} |" + graphics[9] + "\n";
            string b11 = aa + "=====================|" + graphics[10] + "\n";
            string l12 = aa + "Level   | " + $"{stats.Level,-10} |" + graphics[11] + "\n";
            string l13 = aa + "XP      | " + $"{stats.Xp,-10} |" + graphics[12] + "\n";
            string l14 = aa + "To Next | " + $"{stats.GetXpToNextLevel(),-10} |" + graphics[13] + "\n";
            string b15 = aa + "=====================|" + graphics[14] + "\n";
            string l16 = aa + "Gold    | " + $"{stats.Money,-10} |" + graphics[15] + "\n";
            string body = border + l1 + l2 + b3 + l4 + l5 + l6 + l7 + l8 + l9 + l10 + b11 + l12 + l13 + l14 + b15 + l16 + border;
            Console.WriteLine(body);
            return body;
        }
        
        //returns a string of the players branch relationships to the screen
        public string PlayersBranchAttributesDisplay() {
            string border = "-------------------------------------------------------------------------------------------------\n";
            string heading = "Character Name | Romance | Friend | Professional | Affinity | Respect | Disgust | Hate | Rivalry \n";
            string printout = border + heading + border;
            var BranchAttributes = lib.Lib[Characters.Cast.PLAYER].BranchAttributes;
            foreach (KeyValuePair<string, Dictionary<string, int>> item in BranchAttributes) {
                string romance = BranchAttributes[item.Key][Characters.Social.ROMANCE].ToString();
                string friend = BranchAttributes[item.Key][Characters.Social.FRIEND].ToString();
                string prof = BranchAttributes[item.Key][Characters.Social.PROFESSIONAL].ToString();
                string affinity = BranchAttributes[item.Key][Characters.Social.AFFINITY].ToString();
                string respect = BranchAttributes[item.Key][Characters.Social.RESPECT].ToString();
                string disgust = BranchAttributes[item.Key][Characters.Social.DISGUST].ToString();
                string hate = BranchAttributes[item.Key][Characters.Social.HATE].ToString();
                string rivalry = BranchAttributes[item.Key][Characters.Social.RIVALRY].ToString();
                string body = $"{item.Key,-15}| {romance,-8}| {friend,-7}| {prof,-13}| {affinity,-9}| {respect,-8}| {disgust,-8}| {hate,-5}| {rivalry,-7}\n";
                printout += body;
            }
            printout += border;
            Console.WriteLine(printout);
            return printout;
        }

        public void InventoryMenu() {
            string body = "What would you like to do?\n";
            body += "1. Use Item\n";
            body += "2. Character Stats\n";
            body += "3. Game Stats\n";
            body += "4. Relationship Status\n";
            body += "5. Exit Menu\n";
            Console.WriteLine(body);
        }

        public string ItemDisplay() {
            string body = "There is no inventory yet";
            Console.WriteLine(body);
            return body;
        }

        public string[] GetWeatherAndTimeGraphics() {
            string[] graphics = null;
            if (gameData.Weather.Equals("Nice")) {
                if (gameData.CheckTimeOfDay(Timer.Get().Moves)) {
                    graphics = Graphics.NiceNight();
                } else {
                    graphics = Graphics.NiceDay();
                }
            } else if (gameData.Weather.Equals("Overcast")) {
                if (gameData.CheckTimeOfDay(Timer.Get().Moves)) {
                    graphics = Graphics.OvercastNight();
                } else {
                    graphics = Graphics.OvercastDay();
                }
            } else {
                graphics = Graphics.Rain();
            }
            return graphics;
        }


        /***********************************Dialogue Textbox Methods*****************************************/
        public void TextBox(string speaker, string phrase) {
            Console.WriteLine($"\n{speaker}");
            string[] words = phrase.Split(" ");
            int size = GetTextBoxWidth(words.Length);
            List<string> text = BreakDialoguePhraseIntoSizedLines(words, size);
            string textbox = GetTextboxToString(size,text);
            Console.WriteLine($"{textbox}\n");
        }

        private string GetTextboxToString(int size, List<string> text) {
            string border = GenerateBorder(size);
            string margin = GenerateMargin();
            string textbox = border+"\n";
            foreach (string line in text) {
                string rightMargin = GenerateRightMargin(line,size);
                textbox += $"{TEXT_BORDER_SYMBOL}{margin}{line}{rightMargin}{TEXT_BORDER_SYMBOL}\n";
            }
            textbox += border;
            return textbox;
        }

        private string GenerateBorder(int size) { 
            string border = "";
            for (int i = 0; i < size + (MARGINS * 2) + 2; i++) {
                border += TEXT_BORDER_SYMBOL;
            }
            return border;
        }

        private string GenerateRightMargin(string text, int size) {
            int start = text.Length + MARGINS + 2 - 4;
            string spaces = "";
            for (int i = start; i < size; i++) {
                spaces += " ";
            }
            return spaces;
        }

        private string GenerateMargin() {
            string margin = "";
            for (int i = 0; i < MARGINS; i++) {
                margin += " ";
            }
            return margin;
        }

        private List<string> BreakDialoguePhraseIntoSizedLines(string[] words, int size) {
            List<string> lines = new List<string>();
            int charCount = 0;
            string line = "";
            foreach (string word in words) {
                charCount += word.Length + 1;
                if (charCount <= size) {
                    line += word + " ";
                } else {
                    lines.Add(line);
                    charCount = word.Length + 1;
                    line = word + " ";
                }
            }
            if (line.Length > 0) {
                lines.Add(line);
            }
            return lines;
        }

        private int GetTextBoxWidth(int size) {
            return size > TEXTBOXS_CAP ? LARGE_TEXTBOX : SMALL_TEXTBOX; 
        }
        

    }
}
