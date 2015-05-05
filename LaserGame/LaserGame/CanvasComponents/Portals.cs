using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Collections;
using Client.LaserComponents;

namespace Client.CanvasBehavior {
    class Portals {

        private static Portals instance;

        public Rectangle FirstPortal { get; set; }
        public Rectangle SecondPortal { get; set; }

        public Hashtable OtherPortals = new Hashtable();

        public Portals() {
            instance = this;
        }

        public void BuildLaserIfIntersect() {
            if(Portals.getInstance().FirstPortal == null || Portals.getInstance().SecondPortal == null) {
                return;
            }
            string temp = "";
            temp = LaserBehavior.IntersectionLinePortals( Laser.getInstance().GetLastLine() );
            switch(temp) {
                case "first":
                    LaserBehavior.BuildPortalLine( Portals.getInstance().SecondPortal );
                    break;
                case "second":
                    LaserBehavior.BuildPortalLine( Portals.getInstance().FirstPortal );
                    break;
            }
        }

        public static Portals getInstance() {
            if(instance == null) {
                instance = new Portals();
            }
            return instance;
        }

    }
}
