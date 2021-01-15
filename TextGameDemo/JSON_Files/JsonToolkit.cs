using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.JSON_Files {
    public class JsonToolkit {
        public const string AROUND_TOWN = "AroundTown";
        public const string BENJAMIN = "Benjamin";
        public const string FIGHTING_WORDS = "FightingWords";
        public const string FOREST_TALK = "ForestTalk";
        public const string LAFFITE = "Lafitte";
        public const string LERIN = "Lerin";
        public const string QUESTING = "Questing";
        public const string SHOPPING = "Shopping";
        public const string YOUNG_LOVE = "YoungLove";

        private static string _path_ = "JSON_Files";

        public static string Get(string moduleName) {
            string path = "";
            switch (moduleName) {
                case AROUND_TOWN : { path = "\\" + AROUND_TOWN + ".json"; }break;
                case BENJAMIN : path = "\\"+BENJAMIN+".json"; break;
                case FIGHTING_WORDS : path = "\\"+FIGHTING_WORDS+".json"; break;
                case FOREST_TALK : path = "\\"+FOREST_TALK+".json"; break;
                case LAFFITE: path = "\\"+LAFFITE+".json"; break;
                case LERIN : path = "\\"+LERIN+".json"; break;
                case QUESTING : path = "\\"+QUESTING+".json"; break;
                case SHOPPING : path = "\\"+SHOPPING+".json"; break;
                case YOUNG_LOVE : path = "\\"+YOUNG_LOVE+".json"; break;
            }
            return GetPath(_path_ +path);
        }

        public static string GetPath(string fileName) {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] directories = path.Split("\\");
            path = "";
            for (int i = 0; i < directories.Length - 4; i++) {
                path += directories[i] + "\\";
            }
            path += fileName;
            return path;
        }

    
    }
}
