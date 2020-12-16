using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Characters {
    public class PlayersStats {
        
        private static PlayersStats stats;
        private static int levelIncrease = 30;

        public static PlayersStats Get() {
            if (stats == null) {
                stats = new PlayersStats();
            }
            return stats;
        }
        private string name;
        private string job;

        private int attack;
        private int magic;
        private int dodge;
        private int accuracy;
        private int stamina;

        private int maxHp;
        private int hp;
        private int maxMana;
        private int mana;

        private int xp;
        private int level;
        private int money;

        //leveling attributes
        private int lvl_atk;
        private int lvl_mag;
        private int lvl_dodge;
        private int lvl_acc;
        private int lvl_stam;
        private int lvl_hp;
        private int lvl_mana;

        public PlayersStats() {
            attack = 15;
            magic = 2;
            dodge = 5;
            accuracy = 15;
            stamina = 15;
            maxHp = 100;
            hp = 100;
            maxMana = 5;
            mana = 5;
            xp = 0;
            level = 1;
            money = 100;
            name = Cast.PLAYER;
            job = Personal.ADVENTURER;

            lvl_atk = 5;
            lvl_mag = 1;
            lvl_dodge = 1;
            lvl_acc = 2;
            lvl_stam = 5;
            lvl_hp = 7;
            lvl_mana = 1;
        }

        public int Attack { get => attack;}
        public int Magic { get => magic;}
        public int Dodge { get => dodge;}
        public int Accuracy { get => accuracy;}
        public int Stamina { get => stamina;}
        public int MaxHp { get => maxHp;}
        public int MaxMana { get => maxMana;}
        public int Level { get => level;}
        public int Xp { get => xp; }

        public int Hp { get => hp; set => hp = value; }
        public int Mana { get => mana; set => mana = value; }
        public int Money { get => money; set => money = value; }

        public string Name { get => name; set => name = value; }
        public string Job { get => job; set => job = value; }

        public void GainXp(int xp) {
            this.xp += xp;
            if (this.xp > GetXpToNextLevel()) {
                LevelUp();
            }
        }

        public int GetXpToNextLevel() {
            int nextLevel = 0;
            for (int i = 0; i <= level; i++) {
                nextLevel += level * levelIncrease;
            }
            return nextLevel;
        }

        public void LevelUp() {
            Console.WriteLine("You've gained a level");
            attack += lvl_atk;
            magic += lvl_mag;
            dodge += lvl_dodge;
            accuracy += lvl_acc;
            stamina += lvl_stam;
            maxHp += lvl_hp;
            maxMana += lvl_mana;
            hp = maxHp;
            mana = maxMana;
        }
    }
}
