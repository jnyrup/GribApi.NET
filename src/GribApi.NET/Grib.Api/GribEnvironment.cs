// Copyright 2017 Eric Millin
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Grib.Api.Interop;
using Grib.Api.Interop.SWIG;
using Grib.Api.Interop.Util;
using System;
using System.Diagnostics;
using System.IO;

namespace Grib.Api
{
    /// <summary>
    /// Interface for configuring GribApi.NET's runtime environment.
    /// </summary>
    /// <remarks>
    /// See also https://software.ecmwf.int/wiki/display/GRIB/Environment+variables.
    /// </remarks>
    public static class GribEnvironment
    {
#pragma warning disable IDE0052 // TODO: not sure if we can remove this?
        private static readonly AutoRef _libHandle;
#pragma warning restore IDE0052 
        private static readonly object _initLock = new object();


        static GribEnvironment()
        {
            if (Initialized)
            { return; }

            lock (_initLock)
            {
                Initialized = true;

                if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("GRIB_API_NO_ABORT")))
                {
                    NoAbort = true;
                }

                string definitions = Environment.GetEnvironmentVariable("GRIB_DEFINITION_PATH");
                if (string.IsNullOrWhiteSpace(definitions))
                {
                    GribEnvironmentLoadHelper.TryFindDefinitions(out definitions);
                }

                AssertValidEnvironment(definitions);

                _libHandle = GribEnvironmentLoadHelper.BootStrapLibrary();
                GribApiNative.HookGribExceptions();

                SamplesPath = definitions.Remove(definitions.LastIndexOf("definitions")) + "samples";
                DefinitionsPath = definitions;

                // grib_api discourages enabling multi-fields because they do not use them regularly and do not feel they
                // are well-tested, however leaving them disabled has caused considerable confusion among users. 
                GribContext.Default.EnableMultipleFieldMessages = true;
            }
        }

        /// <summary>
        /// Hook to initialize GribApi.NET. In very rare cases, you may need to call this method directly
        /// to ensure the native libraries are bootstrapped.
        /// </summary>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        public static void Init()
        {
            // empty hook for static ctor
        }

        /// <summary>
        /// Asserts the valid environment.
        /// </summary>
        /// <exception cref="GribApiException">
        /// GribEnvironment::DefinitionsPath must be a valid path. If you're using ASP.NET or NUnit, this exception is usually caused by shadow copying. Please see GribApi.NET's documentation for help.
        /// or
        /// Could not locate 'definitions/boot.def'.
        /// </exception>
        private static void AssertValidEnvironment(string definitions)
        {
            string[] paths = definitions.Split(new[] { ';' });
            string existingPath = "";
            bool exists = false;

            foreach (string path in paths)
            {
                existingPath = path;
                exists = Directory.Exists(path);

                if (exists)
                { break; }
            }

            if (!exists)
            {
                throw new GribApiException("GribEnvironment::DefinitionsPath must be a valid path. Please see GribApi.NET's documentation for help. Path: " + DefinitionsPath);
            }

            if (!File.Exists(Path.Combine(existingPath, "boot.def")))
            {
                throw new GribApiException("Could not locate 'definitions/boot.def'.");
            }
        }

        /// <summary>
        /// Sets an env variable and notifies the C runtime to update its values.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="val">The value.</param>
        private static void PutEnvVar(string name, string val)
        {
            Environment.SetEnvironmentVariable(name, val, EnvironmentVariableTarget.Process);
            Win32._putenv_s(name, val);

            Debug.Assert(Environment.GetEnvironmentVariable(name) == val);
        }

        /// <summary>
        /// Gets or sets the JPEG dump path. This is primarily useful during debugging
        /// </summary>
        /// <value>
        /// The JPEG dump path.
        /// </value>
        public static string JpegDumpPath
        {
            get => Environment.GetEnvironmentVariable("GRIB_DUMP_JPG_FILE");
            set => PutEnvVar("GRIB_DUMP_JPG_FILE", value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not grib_api should abort on an assertion failure or error log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no abort]; otherwise, <c>false</c>.
        /// </value>
        public static bool NoAbort
        {
            get => Environment.GetEnvironmentVariable("GRIB_API_NO_ABORT") == "1";
            set => SetNoAbort(value);
        }

        private static void SetNoAbort(bool value)
        {
            string val = value ? "1" : "0";
            PutEnvVar("GRIB_API_NO_ABORT", val);

            val = value ? "0" : "1";
            PutEnvVar("GRIB_API_FAIL_IF_LOG_MESSAGE", val);
        }

        /// <summary>
        /// Gets or sets the location of grib_api's definitions directory. By default, it is located at Grib.Api/definitions.
        /// </summary>
        /// <value>
        /// The definitions path.
        /// </value>
        public static string DefinitionsPath
        {
            get => _definitions;
            set => GribApiNative.SetDefaultDefinitionsPath(value);
        }
        private static readonly string _definitions = "";


        /// <summary>
        /// Gets or sets the location of grib_api's samples directory. By default, it is located  next to Grib.Api/definitions.
        /// </summary>
        /// <value>
        /// The definitions path.
        /// </value>
        public static string SamplesPath
        {
            get => Environment.GetEnvironmentVariable("GRIB_SAMPLES_PATH") + "";
            set => PutEnvVar("GRIB_SAMPLES_PATH", value);
        }

        /// <summary>
        /// Gets the grib_api version wrapped by this library.
        /// </summary>
        /// <value>
        /// The grib API version.
        /// </value>
        public static string GribApiVersion => GetGribApiVersion();

        private static string GetGribApiVersion()
        {
            if (!Initialized)
            {
                Init();
            }

            string version = GribApiProxy.GribGetApiVersion().ToString();
            string major = version[0].ToString();
            string minor = int.Parse(version.Substring(1, 2)).ToString();
            string patch = int.Parse(version.Substring(3, 2)).ToString();

            return string.Join(".", major, minor, patch);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GribEnvironment"/> is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        private static bool Initialized { get; set; }
    }
}
