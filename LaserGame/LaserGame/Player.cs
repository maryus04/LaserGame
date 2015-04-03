using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Client {
    class Player {

        public static string Name { set; get; }
        public static Rectangle FirstPortal { get; set; }
        public static Rectangle SecondPortal { get; set; }

        public static bool firstClick;

        public Player() {

        }

    }
}
