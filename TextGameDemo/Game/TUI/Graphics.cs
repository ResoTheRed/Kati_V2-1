using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameDemo.Game.TUI {
    public class Graphics {

        public static string[] NiceDay() {
            string[] graphic = new string[7];
            graphic[0] = "        .";
            graphic[1] = "      \\ | /";
            graphic[2] = "    '-.;;;.-'";
            graphic[3] = "   -==;;;;;==-";
            graphic[4] = "    .-';;;'-.";
            graphic[5] = "      / | \\";
            graphic[6] = "        '";
             return graphic;
        }

        public static string[] NiceNight() {
            string[] graphics = new string[7];
            graphics[0] = "         _.._";
            graphics[1] = "        .' .-'`";
            graphics[2] = "       /  /";
            graphics[3] = "       |  |";
            graphics[4] = "       \\  '.___.;";
            graphics[5] = "        '._  _.'";
            graphics[6] = "           ``";
            return graphics;
        }

        public static string[] OvercastDay() {
            string[] graphics = new string[7];
            graphics[0] = "            . . -;.";
            graphics[1] = "         \\ |(      ).";
            graphics[2] = "        '-.;;(       '`.";
            graphics[3] = "       -==;;(      .    )";
            graphics[4] = "       (( `(  ..__.:'-'";
            graphics[5] = "       `(` ____._: \")";
            graphics[6] = "";
            return graphics;
        }

        public static string[] OvercastNight() {
            string[] graphics = new string[7];
            graphics[0] = "          _.._ . -;.";
            graphics[1] = "         .'  .(      ).";
            graphics[2] = "        /   /(       '`.";
            graphics[3] = "       |   |(      .    )";
            graphics[4] = "       (( `(  ..__.:'-'";
            graphics[5] = "       `(` ____._: \")";
            graphics[6] = "";
            return graphics;
        }

        public static string[] Rain() {
            string[] graphics = new string[7];
            graphics[0] = "          __   _";
            graphics[1] = "        _(  )_( )_";
            graphics[2] = "       (_   _    _)";
            graphics[3] = "      / /(_) (__)";
            graphics[4] = "     / / / / / /";
            graphics[5] = "    / / / / / /";
            graphics[6] = "";
            return graphics;
        }

        public static string[] Fighter() {
            string[] graphics = new string[16];
            graphics[0] = "   |\\                     /)";
            graphics[1] = " /\\_\\\\__               (_//";
            graphics[2] = "|   `>\\-`     _._       //`)";
            graphics[3] = " \\ /` \\\\  _.-`:::`-._  //";
            graphics[4] = "  `    \\|`    :::    `|/";
            graphics[5] = "        |     :::     |";
            graphics[6] = "        |.....:::.....|";
            graphics[7] = "        |:::::::::::::|";
            graphics[8] = "        |     :::     |";
            graphics[9] = "        \\     :::     /";
            graphics[10] = "         \\    :::    /";
            graphics[11] = "          `-. ::: .-'";
            graphics[12] = "           //`:::`\\\\";
            graphics[13] = "          //   '   \\\\";
            graphics[14] = "         |/         \\\\ ";
            graphics[15] = "";
            return graphics;
        }

    }
}
