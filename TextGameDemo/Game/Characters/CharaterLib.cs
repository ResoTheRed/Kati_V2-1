using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.Characters {
    public class CharacterLib {

        public const string COLLIN = "Collin";
        public const string DAN = "Dan";
        public const string ALBRECHT = "Albrecht";
        public const string BENJAMIN = "Benjamin";
        public const string TETA = "Teta";
        public const string LERIN = "Lerin";
        public const string LAFITTE = "Lafitte";
        public const string QUINN = "Quinn";

        private Dictionary<string, Character> lib;

        public Dictionary<string, Character> Lib { get => lib; set => lib = value; }

        public CharacterLib() {
            Lib = new Dictionary<string, Character>();
            Lib[QUINN] = new Quinn();
            Lib[COLLIN] = new Collin();
            Lib[TETA] = new Teta();
            Lib[DAN] = new Dan();
            Lib[ALBRECHT] = new Albrecht();
            Lib[LERIN] = new Lerin();
            Lib[LAFITTE] = new Lafitte();
            Lib[BENJAMIN] = new Benjamin();
        }

        
    }
}
