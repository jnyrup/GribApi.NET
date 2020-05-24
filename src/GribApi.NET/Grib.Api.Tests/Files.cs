using System;
using System.Linq;
using NUnit.Framework;
using System.IO;
using Grib.Api.Interop;
using System.Threading;


namespace Grib.Api.Tests
{
    [TestFixture]
    public class Files
    {
        [Test, Timeout(2000)]
        public void TestEnableMultField()
        {
            Assert.IsTrue(GribContext.Default.EnableMultipleFieldMessages);

            using (GribFile file = new GribFile(Settings.MULTI))
            {
                Assert.AreEqual(file.MessageCount, 24);
            }

            GribContext.Default.EnableMultipleFieldMessages = false;
            Assert.IsFalse(GribContext.Default.EnableMultipleFieldMessages);

            using (GribFile file = new GribFile(Settings.MULTI))
            {
                Assert.AreEqual(file.MessageCount, 1);
            }

            GribContext.Default.EnableMultipleFieldMessages = true;
            Assert.IsTrue(GribContext.Default.EnableMultipleFieldMessages);
        }

        [Test, Timeout(2000)]
        public void TestBadFile()
        {
            Assert.Throws<FileLoadException>(() =>
            {
                using GribFile file = new GribFile(Settings.BAD);
            });
        }

        [Test, Timeout(2000)]
        public void TestEmptyFile()
        {
            Assert.Throws<FileLoadException>(() =>
            {
                using GribFile file = new GribFile(Settings.EMPTY);
            });
        }

        [Test, Timeout(2000)]
        public void TestNullFile()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GribFile(null);
            });
        }

        [Test, Timeout(2000)]
        public void TestNonExistingFile()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                new GribFile(string.Empty);
            });
        }

        [Test, Timeout(5000)]
        public void TestSteographic()
        {
            using GribFile file = new GribFile(Settings.STEREO);
            GribMessage msg = file.First();
            Assert.Greater(msg.ValuesCount, 1);
            foreach (GridCoordinateValue gs in msg.GridCoordinateValues)
            {

                Assert.Greater(gs.Latitude, 15);
            }
        }

        //[Test, Timeout(5000)]
        //public void TestBounds ()
        //{
        //    using (GribFile file = new GribFile(Settings.TIME))
        //    {

        //        GribMessage msg = file.First();
        //        msg["jDirectionIncrement"].AsDouble(0.25);
        //     //   for (var i = 1600; i < 1681; i++)
        //    //    Console.WriteLine("[{3}] Lat {0}, Lon {1}, Value {2}", msg.GridCoordinateValues.ElementAt(i).Latitude, msg.GridCoordinateValues.ElementAt(i).Longitude, msg.GridCoordinateValues.ElementAt(i).Value, i);

        //    }
        //}

        [Test, Timeout(5000)]
        public void TestBits()
        {
            var bytes = File.ReadAllBytes(Settings.CONSTANT2);
            using var msg = GribMessage.Create(bytes);
            Assert.Greater(msg.ValuesCount, 1);
            foreach (GridCoordinateValue gs in msg.GridCoordinateValues)
            {
                Assert.Greater(gs.Latitude, 15);
            }
        }

        [Ignore("GribStream still don't seem stable")]
        [Test, Timeout(50000)]
        public void TestStream()
        {
            //Stopwatch sw = new Stopwatch();

            using var fs = File.OpenRead(Settings.MIXED);
            //sw.Start();
            foreach (var msg in new GribStream(fs))
            {
                Assert.Greater(msg.ValuesCount, 1);
                foreach (GridCoordinateValue cv in msg.GridCoordinateValues)
                {
                    Assert.Greater(cv.Latitude, -180);
                    Assert.Less(cv.Latitude, 180);
                }
            }
            //sw.Stop();
            //Console.WriteLine("Elapsed={0}", sw.Elapsed);
            //sw.Reset();

            //sw.Start();
            //using (GribFile file = new GribFile(Settings.MIXED))
            //{
            //    foreach (var msg in file)
            //    {
            //        Assert.Greater(msg.ValuesCount, 1);
            //        foreach (GridCoordinateValue cv in msg.GridCoordinateValues)
            //        {
            //            Assert.Greater(cv.Latitude, -180);
            //            Assert.Less(cv.Latitude, 180);
            //        }
            //    }
            //}
            //sw.Stop();
            //Console.WriteLine("Elapsed={0}", sw.Elapsed);
        }

        [Test, Timeout(5000)]
        public void TestOpenPng()
        {
            using GribFile file = new GribFile(Settings.PNG_COMPRESSION);
            Assert.IsTrue(file.MessageCount > 0);

            using var msg = file.First();
            try
            {
                Assert.IsTrue(msg["packingType"].AsString().ToLower().EndsWith("_png"));
                Assert.IsTrue(msg.ValuesCount > 0);
                Assert.IsTrue(msg.GridCoordinateValues.Any());
                int i = 0;
                foreach (var v in msg.GridCoordinateValues)
                {
                    Assert.AreNotEqual(double.NaN, v.Value);
                    if (i++ > 1000)
                        break;
                }
            }
            catch (GribApiException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(msg.ParameterShortName);
                Console.WriteLine(msg.ToString());
                Assert.IsTrue(false);
            }
        }

        [Test, Timeout(2000)]
        public void TestOpenComplex()
        {

            using GribFile file = new GribFile(Settings.COMPLEX_GRID);
            Assert.IsTrue(file.MessageCount > 0);

            foreach (var msg in file)
            {
                try
                {
                    Assert.IsTrue(msg["packingType"].AsString().ToLower().Contains("complex"));
                    Assert.IsTrue(msg.ValuesCount > 0);
                    msg.Values(out double[] vals);
                    Assert.IsTrue(vals.Any());
                    foreach (var v in vals)
                    {
                        Assert.AreNotEqual(double.NaN, v);
                    }
                }
                catch (GribApiException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(msg.ParameterShortName);
                    Console.WriteLine(msg.ToString());
                    Assert.IsTrue(false);
                }
            }
        }

        [Test, Timeout(2000)]
        public void TestEnumDisposal()
        {
            using var file1 = new GribFile(Settings.COMPLEX_GRID);
            using var file2 = new GribFile(Settings.TIME);
            using var msg1 = file1.First();
            using var msg2 = file2.First();

            var msgEnumerator1 = msg1.GetEnumerator();
            var msgEnumerator2 = msg2.GetEnumerator();
            int i = 0;
            while (msgEnumerator1.MoveNext() && msgEnumerator2.MoveNext())
            {
                i++;
                Assert.IsNotEmpty(msgEnumerator1.Current.Key);
                Assert.IsNotEmpty(msgEnumerator2.Current.Key);
            }
            Assert.IsTrue(i > 3);
        }
    }
}
