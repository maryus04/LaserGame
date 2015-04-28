using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Client.CanvasBehavior;
using System.Windows;

namespace ClientTest {
    [TestClass]
    public class MechanicTest {
        /// <summary>
        /// X-axis first parameter from left to right
        /// Y-axis second parameter from top to bottom
        /// </summary>

        /// <summary>
        ///A test for GetIntersectionPointTwoLinesNoIntersection
        ///the result is Point(-1,-1) cause the line never intersect
        ///</summary>
        [TestMethod()]
        public void GetIntersectionPointTwoLinesNoIntersectionTest() {
            Point lp1 = new Point( 56.8322662316, 345.8379028764 );
            Point lp2 = new Point( 1243.8351994198, 324.4418393593 );
            Point rp1 = new Point( 40.8809675263, 234.6070460782 );
            Point rp2 = new Point( 65.5285091118, 47.3629395537 );
            Point expected = new Point(-1,-1);
            Point actual;
            actual = Mechanic.GetIntersectionPointTwoLines( lp1, lp2, rp1, rp2 );
            Assert.AreEqual( expected, actual );
        }

        /// <summary>
        ///A test for GetIntersectionPointTwoLines
        ///the result is Point(20,20) center points
        ///</summary>
        [TestMethod()]
        public void GetIntersectionPointTwoLinesCenterTest() {
            Point lp1 = new Point( 0, 20 );
            Point lp2 = new Point( 40, 20 );
            Point rp1 = new Point( 20, 0 );
            Point rp2 = new Point( 20, 40 );
            Point expected = new Point( 20, 20 );
            Point actual;
            actual = Mechanic.GetIntersectionPointTwoLines( lp1, lp2, rp1, rp2 );
            Assert.AreEqual( expected, actual );
        }

        /// <summary>
        ///A test for GetIntersectionPointTwoLines
        ///the result is Point(20,20) center points
        ///</summary>
        [TestMethod()]
        public void GetIntersectionPointTwoLinesTest() {
            Point lp1 = new Point( 206.8322662316, 68.8379028764 );
            Point lp2 = new Point( 1130.8351994198, 459.4418393593 );
            Point rp1 = new Point( 93.8809675263, 466.6070460782 );
            Point rp2 = new Point( 1131.5285091118, 22.3629395537 );
            Point expected = new Point( 617.491142085183, 242.43580792480 );
            Point actual;
            actual = Mechanic.GetIntersectionPointTwoLines( lp1, lp2, rp1, rp2 );
            Assert.AreEqual( expected, actual );
        }

        /// <summary>
        ///A test for IsOnLine
        ///</summary>
        [TestMethod()]
        public void IsOnLineTest() {
            Point lp1 = new Point(); // TODO: Initialize to an appropriate value
            Point lp2 = new Point(); // TODO: Initialize to an appropriate value
            Point pt = new Point(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Mechanic.IsOnLine( lp1, lp2, pt );
            //Assert.AreEqual( expected, actual );
        }
    }
}
