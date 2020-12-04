using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game {

    struct QuestTitles {
        //Lerin Quests
        public const string MUSHROOM = "mushroom";
        public const string YELLOW_CRYSTAL = "yellow crystal";
        //Sivian Quests
        public const string SCOUT_FOREST_RADIANT = "Scout Forest Radiant";
        public const string DEFEAT_SPIDERS = "Defeat Spiders";
        public const string SCOUT_MOUNTAINS = "Scout Mountains";
        public const string HUNT_GAME_RADIANT = "Hunt Game";
        public const string PROVIDE_FOOD_RADIANT = "Prvide Food";
        //Albrect Quests
        public const string RETRIEVE_COAL_RADIANT = "Coal";
        public const string RETRIEVE_IRON_ORE_RADIANT = "Iron Ore";
        //Machiah
        public const string GIVE_ALMS_RADIANT = "Give Alms";
        //Dan
        public const string GET_DELIVERY_RADIANT = "Get Deleivery";
        //Romero
        public const string FIND_HOE = "Find Hoe";
        //Teta
        public const string FIND_GOAT = "Find Goat";
        public const string MONSTER_KILLER = "Monster Killer"; 
        public const string IN_NEED = "In Need";
        public const string IN_NEED_2 = "In Need 2";
        public const string IN_NEED_3 = "In Need 3";
        //Christina
        public const string GET_WEAPON = "Get Weapon";
    }
    
    public class QuestTracker {

        public static QuestTracker tracker;

        public static QuestTracker Tracker() {
            if (tracker == null) {
                tracker = new QuestTracker();
            }
            return tracker;
        }

        private Dictionary<string, bool> questIsComplete;
        private Dictionary<string, bool> questIsActive;

        public Dictionary<string, bool> QuestIsComplete { get => questIsComplete; set => questIsComplete = value; }
        public Dictionary<string, bool> QuestIsActive { get => questIsActive; set => questIsActive = value; }

        private QuestTracker() {
            QuestIsComplete = new Dictionary<string, bool>();
            QuestIsActive = new Dictionary<string, bool>();
            LoadQuestComplete();
            LoadQuestActive();
        }

        private void LoadQuestComplete() {
            QuestIsComplete[QuestTitles.MUSHROOM] = false;
            QuestIsComplete[QuestTitles.YELLOW_CRYSTAL] = false;
            QuestIsComplete[QuestTitles.SCOUT_FOREST_RADIANT] = false;
            QuestIsComplete[QuestTitles.DEFEAT_SPIDERS] = false;
            QuestIsComplete[QuestTitles.SCOUT_MOUNTAINS] = false;
            QuestIsComplete[QuestTitles.HUNT_GAME_RADIANT] = false;
            QuestIsComplete[QuestTitles.PROVIDE_FOOD_RADIANT] = false;
            QuestIsComplete[QuestTitles.RETRIEVE_COAL_RADIANT] = false;
            QuestIsComplete[QuestTitles.RETRIEVE_IRON_ORE_RADIANT] = false;
            QuestIsComplete[QuestTitles.GIVE_ALMS_RADIANT] = false;
            QuestIsComplete[QuestTitles.GET_DELIVERY_RADIANT] = false;
            QuestIsComplete[QuestTitles.FIND_HOE] = false;
            QuestIsComplete[QuestTitles.FIND_GOAT] = false;
            QuestIsComplete[QuestTitles.MONSTER_KILLER] = false;
            QuestIsComplete[QuestTitles.IN_NEED] = false;
            QuestIsComplete[QuestTitles.IN_NEED_2] = false;
            QuestIsComplete[QuestTitles.IN_NEED_3] = false;
            QuestIsComplete[QuestTitles.GET_WEAPON] = false; 
        }
        
        private void LoadQuestActive() {
            QuestIsActive[QuestTitles.MUSHROOM] = false;
            QuestIsActive[QuestTitles.YELLOW_CRYSTAL] = false;
            QuestIsActive[QuestTitles.SCOUT_FOREST_RADIANT] = false;
            QuestIsActive[QuestTitles.DEFEAT_SPIDERS] = false;
            QuestIsActive[QuestTitles.SCOUT_MOUNTAINS] = false;
            QuestIsActive[QuestTitles.HUNT_GAME_RADIANT] = false;
            QuestIsActive[QuestTitles.PROVIDE_FOOD_RADIANT] = false;
            QuestIsActive[QuestTitles.RETRIEVE_COAL_RADIANT] = false;
            QuestIsActive[QuestTitles.RETRIEVE_IRON_ORE_RADIANT] = false;
            QuestIsActive[QuestTitles.GIVE_ALMS_RADIANT] = false;
            QuestIsActive[QuestTitles.GET_DELIVERY_RADIANT] = false;
            QuestIsActive[QuestTitles.FIND_HOE] = false;
            QuestIsActive[QuestTitles.FIND_GOAT] = false;
            QuestIsActive[QuestTitles.MONSTER_KILLER] = false;
            QuestIsActive[QuestTitles.IN_NEED] = false;
            QuestIsActive[QuestTitles.IN_NEED_2] = false;
            QuestIsActive[QuestTitles.IN_NEED_3] = false;
            QuestIsActive[QuestTitles.GET_WEAPON] = false; 
        }

    }
}
