using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Collections;

namespace Client.CanvasComponents {
    class Portals {

        private static Portals instance;

        public Rectangle FirstPortal { get; set; }
        public Rectangle SecondPortal { get; set; }

        public Hashtable OtherPortals = new Hashtable( 100 );

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
