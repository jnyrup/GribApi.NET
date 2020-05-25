﻿using System.Linq;
using NUnit.Framework;

namespace Grib.Api.Tests
{
    [TestFixture]
    public class Set
    {
        [Test, Timeout(2000)]
        public void TestSetBitmapMissing()
        {
            using GribFile file = new GribFile(Settings.REG_LATLON_GRB1);
            using var msg = file.First();
            int missing = 3333;
            // set the value used to represent missing data
            msg.MissingValue = missing;
            Assert.AreEqual(msg.MissingValue, missing);
            Assert.IsFalse(msg.HasBitmap);

            int numVals = 10;
            double[] vals = new double[numVals];

            for (int i = 0; i < numVals; i++)
            {
                vals[i] = missing;
            }

            msg.HasBitmap = true;
            msg.SetValues(vals);
            msg.Values(out double[] vals2);

            for (int i = 0; i < numVals; i++)
            {
                Assert.AreEqual(vals[i], vals2[i]);
                Assert.AreEqual(missing, vals2[i]);
            }

            missing = 9898;
            msg.MissingValue = missing;
            msg.Values(out vals);

            for (int i = 0; i < numVals; i++)
            {
                Assert.AreEqual(missing, vals[i]);
            }
        }

        [Test, Timeout(2000)]
        public void TestSetBitmap()
        {
            using GribFile file = new GribFile(Settings.REG_LATLON_GRB1);
            Assert.IsTrue(file.MessageCount > 0);
            using var msg = file.First();

            int numVals = 10;
            double[] vals = new double[numVals];
            double val = 42;

            for (int i = 0; i < numVals; i++)
            {
                vals[i] = val;
            }

            msg.HasBitmap = true;
            msg.SetValues(vals);
            msg.Values(out double[] vals2);

            for (int i = 0; i < numVals; i++)
            {
                Assert.AreEqual(vals[i], vals2[i]);
                Assert.AreEqual(val, vals2[i]);
            }
        }

        [Test, Timeout(2000)]
        public void TestPrecision()
        {
            using var file = new GribFile(Settings.REG_GAUSSIAN_SURFACE_GRB2);
            using var msg = file.First();
            Assert.AreEqual(msg["bitsPerValue"].AsInt(), 14);
            msg.DecimalPrecision = 4;
            Assert.AreEqual(msg["bitsPerValue"].AsInt(), 20);
        }
    }
}
