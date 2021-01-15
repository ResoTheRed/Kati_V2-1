using Kati.Module_Hub;
using System.Collections.Generic;

namespace Kati.GenericModule {
    
    /// <summary>
    /// Links all elements of an individual module to a single 
    /// container class Module. Unique Models will extend this
    /// class becoming type module.
    /// </summary>
    
    public abstract class Module {

        private string moduleKey;
        private string path;
        private Controller ctrl;

        public string ModuleKey { get => moduleKey;}
        public string Path { get => path; set => path = value; }
        protected Controller Ctrl { get => ctrl; set => ctrl = value; }

        public Module(string name, string path) {
            moduleKey = name;
            Path = path;
            Ctrl = new Controller(Path);
        }

        public abstract DialoguePackage Run();

        
       
    }
}
