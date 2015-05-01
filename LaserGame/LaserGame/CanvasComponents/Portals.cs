using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Collections;

namespace Client.CanvasBehavior {
    class Portals {

        private static Portals instance;

        public Rectangle FirstPortal { get; set; }
        public Rectangle SecondPortal { get; set; }

        public Hashtable OtherPortals = new Hashtable();

        public Portals() {
            instance = this;
        }

        public static Portals getInstance() {
            if(instance == null) {
                instance = new Portals();
            }
            return instance;
        }

    }
}
