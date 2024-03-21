using System.Collections.Immutable;

namespace DotNetExercise5
{
    internal enum VehicleColor
    {
        // Copied from console color, because it should be possible to change independently.
        Black,
        DarkBlue,
        DarkGreen,
        DarkCyan,
        DarkRed,
        DarkMagenta,
        DarkYellow,
        Gray,
        DarkGray,
        Blue,
        Green,
        Cyan,
        Red,
        Magenta,
        Yellow,
        White
    }
    internal static class VehicleColorMethods
    {
        private static ImmutableDictionary<VehicleColor, string> SwedishDictionary { get; } =
            new Dictionary<VehicleColor, string>()
            { {VehicleColor.Black, "Svart"},
              {VehicleColor.DarkBlue, "Mörkblå"},
              {VehicleColor.DarkGreen, "Mörkgrön"},
              {VehicleColor.DarkCyan, "Mörk cyan"},
              {VehicleColor.DarkRed, "Mörkröd"},
              {VehicleColor.DarkMagenta, "Mörk magenta"},
              {VehicleColor.DarkYellow, "Mörkgul"},
              {VehicleColor.Gray, "Grå"},
              {VehicleColor.DarkGray, "Mörkgrå"},
              {VehicleColor.Blue, "Blå"},
              {VehicleColor.Green, "Grön"},
              {VehicleColor.Cyan, "Cyan"},
              {VehicleColor.Red, "Röd"},
              {VehicleColor.Magenta, "Magenta"},
              {VehicleColor.Yellow, "Gul"},
              {VehicleColor.White, "Vit"},
            }.ToImmutableDictionary();
        internal static string Swedish(this VehicleColor color)
        {
            return SwedishDictionary[color];
        }
    }
}
