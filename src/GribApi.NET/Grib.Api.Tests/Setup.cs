using NUnit.Framework;
using System;
using System.Threading;

namespace Grib.Api.Tests
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void OnSetup()
        {
            if (Environment.GetEnvironmentVariable("_GRIB_BREAK") == "1")
            {
                Console.WriteLine("Breaking on start...");
                using var mre = new ManualResetEvent(false);

                // after attaching nunit-agent, put a breakpoint here
                while (!mre.WaitOne(250))
                    ;
            }
            Console.WriteLine("Testing with grib_api v{0}", GribEnvironment.GribApiVersion);
        }

        public static void GribContext_OnLog(int lvl, string msg)
        {
            Console.WriteLine(string.Format("Lvl {0}: {1}", lvl, msg));
        }
    }
}
