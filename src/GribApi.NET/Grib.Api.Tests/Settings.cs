using System;
using System.IO;

namespace Grib.Api.Tests
{
    internal static class Settings
    {
        private static string TestData => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");

        public static string PACIFIC_WIND => Path.Combine(TestData, "Pacific.wind.7days.grb");
        public static string GRIB => Path.Combine(TestData, "GRIB.grb");
        public static string REG_LATLON_GRB1 => Path.Combine(TestData, "regular_latlon_surface.grib1");
        public static string REDUCED_LATLON_GRB2 => Path.Combine(TestData, "reduced_latlon_surface.grib2");
        public static string OUT_INDEX => Path.Combine(TestData, "out.index");
        public static string OUT_GRIB => Path.Combine(TestData, "out.grb");
        public static string REG_GAUSSIAN_SURFACE_GRB2 => Path.Combine(TestData, "regular_gaussian_surface.grib2");
        public static string REG_GAUSSIAN_MODEL_GRB1 => Path.Combine(TestData, "reduced_gaussian_model_level.grib1");
        public static string SPHERICAL_PRESS_LVL => Path.Combine(TestData, "spherical_pressure_level.grib1");
        public static string PNG_COMPRESSION => Path.Combine(TestData, "MRMS2.grib2");
        public static string COMPLEX_GRID => Path.Combine(TestData, "spectral_complex.grib1");
        public static string BAD => Path.Combine(TestData, "bad.grb");
        public static string EMPTY => Path.Combine(TestData, "empty.grb");
        public static string TIME => Path.Combine(TestData, "time.grb");
        public static string STEREO => Path.Combine(TestData, "polar_stereo.grib2");
        public static string MULTI => Path.Combine(TestData, "Z_NWGD_C_BABJ.GRB2");
        public static string MIXED => Path.Combine(TestData, "mixed.grib");
        public static string WEIRD => Path.Combine(TestData, "weird.grb");
        public static string CONSTANT2 => Path.Combine(TestData, "constant_field.grib2");
        public static string RAW_FILE_BAD_ETO => Path.Combine(TestData, "RawFileBadETOGrib.os");
    }
}
