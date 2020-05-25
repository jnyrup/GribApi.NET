﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Grib.Api.Interop;
using System.Text.RegularExpressions;
using System.IO;

namespace Grib.Api.Tests
{
    [TestFixture]
    public class Get
    {

        [Test, Timeout(2000)]
        public void TestGetCounts()
        {
            using GribFile file = new GribFile(Settings.GRIB);
            Assert.IsTrue(file.MessageCount > 0);
            foreach (var msg in file)
            {
                Assert.AreNotEqual(msg.DataPointsCount, 0);
                Assert.AreNotEqual(msg.ValuesCount, 0);
                Assert.AreEqual(msg.ValuesCount, msg["numberOfCodedValues"].AsInt());
                Assert.IsTrue(msg["numberOfCodedValues"].IsReadOnly);
                Assert.AreEqual(msg.DataPointsCount, msg.ValuesCount + msg.MissingCount);
            }
        }

        [Test, Timeout(2000)]
        public void TestGetVersion()
        {
            Regex re = new Regex(@"^(\d+\.){2}\d+$");
            Assert.IsTrue(re.IsMatch(GribEnvironment.GribApiVersion));
        }

        [Test, Timeout(2000)]
        public void TestGetNativeType()
        {
            using GribFile file = new GribFile(Settings.REG_LATLON_GRB1);
            using var msg = file.First();
            Assert.AreEqual(msg["packingType"].NativeType, GribValueType.String);
            Assert.AreEqual(msg["longitudeOfFirstGridPointInDegrees"].NativeType, GribValueType.Double);
            Assert.AreEqual(msg["numberOfPointsAlongAParallel"].NativeType, GribValueType.Int);
            Assert.AreEqual(msg["values"].NativeType, GribValueType.DoubleArray);


            // TODO: test other types
        }

        [Test, Timeout(2000)]
        public void TestDumpNoError()
        {
            using GribFile file = new GribFile(Settings.REG_LATLON_GRB1);
            using var msg = file.First();
            msg.Dump();
        }

        [Test, Timeout(2000)]
        public void TestCanConvertToDegress()
        {
            using GribFile file = new GribFile(Settings.REDUCED_LATLON_GRB2);
            using var msg = file.First();

            // true
            //  Assert.IsTrue(msg["latitudeOfFirstGridPointInDegrees"].CanConvertToDegrees);
            Assert.IsTrue(msg["latitudeOfFirstGridPoint"].CanConvertToDegrees);
            Assert.IsTrue(msg["longitudeOfFirstGridPointInDegrees"].CanConvertToDegrees);
            Assert.IsTrue(msg["longitudeOfFirstGridPoint"].CanConvertToDegrees);
            Assert.IsTrue(msg["latitudeOfLastGridPointInDegrees"].CanConvertToDegrees);
            Assert.IsTrue(msg["latitudeOfLastGridPoint"].CanConvertToDegrees);
            Assert.IsTrue(msg["jDirectionIncrement"].CanConvertToDegrees);
            Assert.IsTrue(msg["iDirectionIncrement"].CanConvertToDegrees);

            // false
            Assert.IsFalse(msg["numberOfPointsAlongAParallel"].CanConvertToDegrees);
            Assert.IsFalse(msg["numberOfPointsAlongAParallelInDegrees"].CanConvertToDegrees);
            Assert.IsFalse(msg["numberOfPointsAlongAMeridian"].CanConvertToDegrees);
            Assert.IsFalse(msg["numberOfPointsAlongAMeridianInDegrees"].CanConvertToDegrees);
            Assert.IsFalse(msg["packingType"].CanConvertToDegrees);
        }

        [Test, Timeout(2000)]
        public void TestGetGrib2()
        {
            double delta = 0.1d;

            using GribFile file = new GribFile(Settings.REDUCED_LATLON_GRB2);
            using var msg = file.First();

            // "InDegrees" is a magic token that converts coordinate double values to degrees
            // explicit degree conversion via key name
            double latitudeOfFirstGridPointInDegrees = msg["latitudeOfFirstGridPoint"].AsDouble();
            Assert.AreEqual(latitudeOfFirstGridPointInDegrees, 90, delta);

            // degree conversion via accessor
            double longitudeOfFirstGridPointInDegrees = msg["longitudeOfFirstGridPoint"].AsDouble();
            Assert.AreEqual(longitudeOfFirstGridPointInDegrees, 0, delta);

            // degree conversion via accessor
            double latitudeOfLastGridPointInDegrees = msg["latitudeOfLastGridPoint"].AsDouble();
            Assert.AreEqual(latitudeOfLastGridPointInDegrees, -90, delta);

            // degree conversion via accessor
            double longitudeOfLastGridPointInDegrees = msg["longitudeOfLastGridPoint"].AsDouble();
            Assert.AreEqual(longitudeOfLastGridPointInDegrees, 360, .5);

            // degree conversion via accessor
            double jDirectionIncrementInDegrees = msg["jDirectionIncrement"].AsDouble();
            Assert.AreEqual(jDirectionIncrementInDegrees, 0.36, delta);

            // degree conversion via accessor
            double iDirectionIncrementInDegrees = msg["iDirectionIncrement"].AsDouble();
            Assert.AreEqual(iDirectionIncrementInDegrees, -1.0E+100, delta);

            int numberOfPointsAlongAParallel = msg["numberOfPointsAlongAParallel"].AsInt();
            Assert.AreEqual(numberOfPointsAlongAParallel, 0x7FFFFFFF);

            int numberOfPointsAlongAMeridian = msg["numberOfPointsAlongAMeridian"].AsInt();
            Assert.AreEqual(numberOfPointsAlongAMeridian, 501);

            string packingType = msg["packingType"].AsString();
            Assert.AreEqual("grid_simple", packingType);
        }

        [Test, Timeout(5000)]
        public void TestSelect()
        {
            var files = new[] { Settings.PNG_COMPRESSION, Settings.COMPLEX_GRID, Settings.PACIFIC_WIND, Settings.REDUCED_LATLON_GRB2, Settings.REG_GAUSSIAN_SURFACE_GRB2, Settings.SPHERICAL_PRESS_LVL };

            var l = files.Select(x => new FileInfo(x)).Select(x => new GribFile(x.FullName)).OrderBy(x => x.First().Time).ToList();
            Assert.IsTrue(l.Any());
        }

        //[Test, Timeout(500000)]
        //public void TestDs ()
        //{
        // //   using (var fs = File.AppendText("C:\\Users\\eric\\ds_vals.txt"))
        //  //  using (var fs = File.AppendText("C:\\Users\\eric\\raw_ex.csv"))
        //   //     weird
        //   // using (var file = new GribFile(Settings.WEIRD))
        //    using (var file = new GribFile(Settings.RAW_FILE_BAD_ETO))
        //    {
        //        using var msg = file.First();
        //        //foreach (var k in msg)
        //        //{
        //        //    Console.WriteLine("{0}: {1}", k.Key, k.AsString());
        //        //}
        //        msg.WriteValuesToCsv("C:\\Users\\eric\\raw_ex.csv");
        //    }
        //}

        [Test, Timeout(5000)]
        public void TestGetParallel()
        {
            var files = new[] { Settings.TIME, Settings.COMPLEX_GRID, Settings.PACIFIC_WIND, Settings.REDUCED_LATLON_GRB2, Settings.REG_GAUSSIAN_SURFACE_GRB2, Settings.GRIB };

            Parallel.ForEach(files, (path, s) =>
            {
                using var file = new GribFile(path);
                Parallel.ForEach(file, (msg, s2) =>
                {
                    if (!msg.HasBitmap)
                        return;

                    try
                    {
                        foreach (var v in msg.GridCoordinateValues)
                        {
                            Assert.AreNotEqual(double.NaN, v.Latitude);
                            Assert.AreNotEqual(double.NaN, v.Longitude);
                            Assert.AreNotEqual(double.NaN, v.Value);
                        }
                    }
                    catch (GribApiException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(msg.ParameterShortName);
                        Console.WriteLine(path);
                        Assert.Fail();
                    }
                });
            });
        }

        [Test, Timeout(2000)]
        public void TesNearest()
        {
            using var file = new GribFile(Settings.REDUCED_LATLON_GRB2);
            Assert.IsTrue(file.MessageCount > 0);
            foreach (var msg in file)
            {
                var val = msg.GridCoordinateValues.First();
                var nearest = msg.FindNearestCoordinates(val);
                Assert.IsTrue(nearest.Length == 4);
                foreach (var n in nearest)
                {
                    Assert.IsTrue(n.Value.Latitude >= 80 && n.Value.Latitude < 82);
                }
            }
        }

        [TestFixture]
        public class IterateValues
        {
            [Test, Timeout(2000)]
            public void TestIterateKeyValuePairs()
            {
                using var file = new GribFile(Settings.SPHERICAL_PRESS_LVL);
                Assert.IsTrue(file.MessageCount > 0);
                Assert.IsTrue(file.First().Any());
            }

            [Test, Timeout(2000)]
            public void TestIterateLatLong()
            {
                using var file = new GribFile(Settings.REDUCED_LATLON_GRB2);
                Assert.IsTrue(file.MessageCount > 0);
                foreach (var msg in file)
                {
                    int c = msg.Count();

                    msg.Namespace = "geography";

                    Assert.AreNotEqual(c, msg.Count());

                    Assert.IsTrue(msg.GridCoordinateValues.Any());
                }
            }
        }

        [Test, Timeout(2000)]
        public void TestTime()
        {
            using GribFile file = new GribFile(Settings.TIME);
            Assert.IsTrue(file.MessageCount > 0);

            int diff = 0;
            int i = 0;

            foreach (var msg in file)
            {
                if (i++ == 0)
                { continue; }

                diff = msg["P2"].AsInt();
                Assert.IsTrue(diff > 0);
                var t = msg.ReferenceTime.AddHours(diff);
                Assert.AreNotEqual(msg.ReferenceTime, t);
                Assert.AreEqual(t, msg.Time);
            }
            Assert.IsTrue(i > 2);
        }

    }
}
