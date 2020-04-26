using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace FunctionPlotter.Helpers
{
    public sealed class Exporter
    {
        public static void ExportAsCsv(string filePath, List<PointF> points)
        {
            var csv = new StringBuilder();

            foreach (var pointF in points)
            {
                var newLine = $"{pointF.X},{pointF.Y}";
                csv.AppendLine(newLine);
            }

            File.WriteAllText(filePath, csv.ToString());
        }
    }
}
