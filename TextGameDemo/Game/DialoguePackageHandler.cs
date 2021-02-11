using System;
using System.Collections.Generic;
using Kati.Module_Hub;

namespace TextGameDemo.Game {
    /// <summary>
    /// Disect the dialogue package and make it managable
    /// </summary>
    public class DialoguePackageHandler {

        public static DialoguePackage Get() {
            if (package == null)
                package = (new DialoguePackageHandler()).Package;
            return package;
        }

        private static DialoguePackage package;

        public DialoguePackage Package { get => package; set => package = value; }

        public DialoguePackageHandler() {
            Package = DialoguePackage.Package();
        }

        //reset package after a dialogue has ended
        public void Reset_for_next() { 
        
        }

    }
}
