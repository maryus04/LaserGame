using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Server.Handlers {
    class PortalHandler {

        private void UpdatePortals() {
            //SetCurrentPortal( rect );
        }

        public static void SetCurrentPortal( ServerClient client, Rectangle rect ) {
            if(client.FirstClick) {
                client.FirstPortal = rect;
                client.FirstClick = false;
            } else {
                client.SecondPortal = rect;
                client.FirstClick = true;
            }
        }

        public static Tuple<int, int> GetPointsFromMessage( string message ) {
            message = message.Substring( message.IndexOf( "COORD:" ) + 6, (message.IndexOf( "ENDCOORD" )) - (message.IndexOf( "COORD:" ) + 6) );
            string x = message.Substring( 0, (message.IndexOf( "," )) );
            message = message.Replace( x + ",", "" );
            string y = message.Substring( 0, message.Length );

            return Tuple.Create( Int32.Parse( x ), Int32.Parse( y ) );
        }

        public static bool IsPortalIntersectingPortal(Rectangle portal) {
            foreach(ServerClient client in Server._nickName.Values){
                if(portal.IntersectsWith( client.FirstPortal ) || portal.IntersectsWith( client.FirstPortal )) {
                    
                    return true;
                }
            }
            return false;
        }

    }
}
