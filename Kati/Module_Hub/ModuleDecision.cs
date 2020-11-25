using System;
using System.Collections.Generic;
using System.Text;

namespace Kati.Module_Hub {
    
    /// <summary>
    /// Decides on which module to use base on module list,
    /// wieghts, and dialogue package data
    /// 
    /// requires Dialogue Package Status and NextModule to 
    /// be set or null
    /// 
    /// </summary>

    public class ModuleDecision {

        private static DialoguePackage package;
        private static List<string> modules;

        public static string PickModule(DialoguePackage pack, List<string> mod) {
            package = pack;
            modules = mod;
            //check if dialogue package is null
            //check status of dialogue package
            //check if there is a next module and if is exists in the modules list
            var chosenModule = CheckDialoguePackage();
            //if package points to value then return it
            if (chosenModule == null && mod.Count >= 1) {
                //change to consult history here
                var temp = new Random();
                var rand = new Random(temp.Next(100000));
                chosenModule = mod[rand.Next(mod.Count)];
            }    
            //else return random value from
            return chosenModule;
        }

        private static string CheckDialoguePackage() {
            if (package != null) {
                if (package.Status == ModuleStatus.RETURN) {
                    return package.Module;
                } else if (package.Status == ModuleStatus.CONTINUE && package.NextModule != null) {
                    return package.NextModule;
                }
            }
            return null;
        }
    }
}
