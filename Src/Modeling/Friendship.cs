﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RT.Util;
using RT.Util.Drawing;
using RT.Util.ExtensionMethods;
using RT.Util.Geometry;
using RT.Util.Serialization;

namespace KtaneStuff.Modeling
{
    using static Md;

    static class Friendship
    {
        sealed class PonyInfo
        {
            public string Filename = null;
            public string Name = null;
            public string Color = null;
            public string NewFilename = null;
        }

        sealed class SymbolInfo
        {
            public int X;
            public int Y;
            public int Symbol;
            public bool IsRowSymbol;
        }

        public static void GenerateModels()
        {
            File.WriteAllText(@"D:\c\KTANE\Friendship\Assets\Models\ModelScreen.obj", GenerateObjFile(Screen(), "Screen"));
            File.WriteAllText(@"D:\c\KTANE\Friendship\Assets\Models\ModelScreenFrame.obj", GenerateObjFile(ScreenFrame(), "ScreenFrame"));
            File.WriteAllText(@"D:\c\KTANE\Friendship\Assets\Models\ModelSelectorFrame.obj", GenerateObjFile(SelectorFrame(), "SelectorFrame"));
            File.WriteAllText(@"D:\c\KTANE\Friendship\Assets\Models\ModelSelectorBtn.obj", GenerateObjFile(SelectorBtn(), "SelectorBtn"));
            File.WriteAllText(@"D:\c\KTANE\Friendship\Assets\Models\ModelSelectorCylinder.obj", GenerateObjFile(SelectorCylinder(), "SelectorCylinder"));
            File.WriteAllText(@"D:\c\KTANE\Friendship\Assets\Models\ModelSubmitBtn.obj", GenerateObjFile(SubmitBtn(), "SubmitBtn"));
            File.WriteAllText(@"D:\c\KTANE\Friendship\Assets\Models\ModelTriangle.obj", GenerateObjFile(Triangle(), "Triangle"));
        }

        public static void GenerateRawBytes()
        {
            File.WriteAllText(@"D:\c\KTANE\Friendship\Assets\FriendshipSymbols.cs", $@"
namespace Friendship {{
    static class FriendshipSymbols {{
        public static byte[][] RawBytes = new byte[][] {{
            {Enumerable.Range(0, 56).Select(i => $@"new byte[] {{{File.ReadAllBytes($@"D:\c\KTANE\Friendship\Manual\img\Friendship Symbol {i:00}.png").JoinString(",")}}}").JoinString(",\r\n            ")}
        }};
    }}
}}");
        }

        public static void SimulateFriendship()
        {
            var ponyNames = new[] { "Aloe Blossom", "Amethyst Star", "Apple Cinnamon", "Apple Fritter", "Babs Seed", "Berry Punch", "Big McIntosh", "Bulk Biceps", "Cadance", "Carrot Top", "Celestia", "Cheerilee", "Cheese Sandwich", "Cherry Jubilee", "Coco Pommel", "Coloratura", "Daisy", "Daring Do", "Derpy", "Diamond Tiara", "Double Diamond", "Filthy Rich", "Granny Smith", "Hoity Toity", "Lightning Dust", "Lily", "Lotus Blossom", "Luna", "Lyra", "Maud Pie", "Mayor Mare", "Moon Dancer", "Night Light", "Nurse Redheart", "Octavia Melody", "Rose", "Screwball", "Shining Armor", "Silver Shill", "Silver Spoon", "Silverstar", "Spoiled Rich", "Starlight Glimmer", "Sunburst", "Sunset Shimmer", "Suri Polomare", "Thunderlane", "Time Turner", "Toe Tapper", "Tree Hugger", "Trenderhoof", "Trixie", "Trouble Shoes", "Twilight Velvet", "Twist", "Vinyl Scratch" };

            var attempts = 0;
            var colHits = 0;
            var rowHits = 0;
            var hits = 0;
            for (int attempt = 0; attempt < 10000; attempt++)
            {
                attempts++;
                tryAgain:

                // 13 × 9
                var allowed = @"
#########XXXX
#########XXXX
##########XXX
###########XX
#############
XX###########
XXX##########
XXXX#########
XXXX#########".Replace("\r", "").Substring(1).Split('\n').Select(row => row.Reverse().Select(ch => ch == '#').ToArray()).ToArray();

                var friendshipSymbols = new List<SymbolInfo>();
                var available = Enumerable.Range(0, 56).ToList();
                var rowSymbols = 0;
                var colSymbols = 0;

                for (var cix = 0; cix < 6; cix++)
                {
                    // Choose a coordinate to place the next friendship symbol.
                    var coords = allowed.SelectMany((row, yy) => row.Select((b, xx) => b ? new { X = xx, Y = yy } : null)).Where(inf => inf != null).ToList();
                    if (coords.Count == 0)
                        goto tryAgain;

                    var coord = coords[Rnd.Next(0, coords.Count)];
                    var x = coord.X;
                    var y = coord.Y;
                    allowed[y][x] = false;

                    // Make sure that future friendship symbols won’t overlap with this one.
                    for (var xx = -2; xx < 3; xx++)
                        for (var yy = -2; yy < 3; yy++)
                            if (y + yy >= 0 && x + xx >= 0 && y + yy < allowed.Length && x + xx < allowed[y + yy].Length)
                                allowed[y + yy][x + xx] = false;

                    // Choose a friendship symbol.
                    var fsIx = Rnd.Next(0, available.Count);
                    var fs = available[fsIx];
                    available.RemoveAt(fsIx);

                    // Remove the other friendship symbol from consideration that represents the same row/column as this one
                    available.RemoveAll(ix => (ix / 14) % 2 == (fs / 14) % 2 && ix / 14 != fs / 14 && ix % 14 == 13 - (fs % 14));

                    // Determine whether this is a row or column symbol.
                    var isRowSymbol = (fs / 14) % 2 != 0;
                    if (isRowSymbol)
                    {
                        rowSymbols++;
                        if (rowSymbols == 3)
                            // If we now have 3 row symbols, remove all the other row symbols from consideration.
                            available.RemoveAll(ix => (ix / 14) % 2 != 0);
                    }
                    else
                    {
                        colSymbols++;
                        if (colSymbols == 3)
                            // If we now have 3 column symbols, remove all the other column symbols from consideration.
                            available.RemoveAll(ix => (ix / 14) % 2 == 0);
                    }

                    friendshipSymbols.Add(new SymbolInfo { X = x, Y = y, IsRowSymbol = isRowSymbol, Symbol = fs });
                }

                // Which column and row symbols should the expert disregard?
                var disregardCol = friendshipSymbols.Where(s => !s.IsRowSymbol && !friendshipSymbols.Any(s2 => s2 != s && s2.X == s.X)).OrderBy(s => s.X).FirstOrDefault();
                var firstCol = friendshipSymbols.Where(s => !s.IsRowSymbol).OrderBy(s => s.X).FirstOrDefault();
                if (disregardCol != firstCol)
                    colHits++;

                var disregardRow = friendshipSymbols.Where(s => s.IsRowSymbol && !friendshipSymbols.Any(s2 => s2 != s && s2.Y == s.Y)).OrderByDescending(s => s.Y).FirstOrDefault();
                var firstRow = friendshipSymbols.Where(s => s.IsRowSymbol).OrderByDescending(s => s.Y).FirstOrDefault();
                if (disregardRow != firstRow)
                    rowHits++;

                if (disregardCol != firstCol || disregardRow != firstRow)
                    hits++;
            }

            Console.WriteLine($"Probability that the special rule applies at all: {(double) hits / attempts * 100:0.#}%");
            Console.WriteLine($"Probability that the special rule applies to the column symbols: {(double) colHits / attempts * 100:0.#}%");
            Console.WriteLine($"Probability that the special rule applies to the row symbols: {(double) rowHits / attempts * 100:0.#}%");
        }

        public static void RenderFriendshipSymbols()
        {
            var ponies = ClassifyJson.DeserializeFile<PonyInfo[]>(@"D:\temp\ponies.json");

            Enumerable.Range(0, ponies.Length).ParallelForEach(4, i =>
            {
                var pony = ponies[i];
                pony.NewFilename = $"Friendship Symbol {i:00}.png";
                var color = Color.FromArgb(Convert.ToInt32(pony.Color.Substring(0, 2), 16), Convert.ToInt32(pony.Color.Substring(2, 2), 16), Convert.ToInt32(pony.Color.Substring(4, 2), 16));
                var newCutieMark = GraphicsUtil.MakeSemitransparentImage(200, 200, g => { g.SetHighQuality(); }, g =>
                {
                    g.Clear(color);
                    var bmp = new Bitmap(pony.Filename);
                    g.DrawImage(bmp, GraphicsUtil.FitIntoMaintainAspectRatio(bmp.Size, new Rectangle(20, 20, 160, 160)));
                }, g =>
                {
                    g.Clear(Color.Transparent);
                    g.FillEllipse(Brushes.Black, 1, 1, 197, 197);
                });
                var tmp = $@"D:\c\KTANE\Friendship\Manual\img\tmp_{pony.NewFilename}";
                var final = $@"D:\c\KTANE\Friendship\Manual\img\{pony.NewFilename}";
                newCutieMark.Save(tmp, ImageFormat.Png);
                CommandRunner.Run("pngcr", tmp, final).OutputNothing().Go();
                File.Delete(tmp);
                lock (ponies)
                    Console.WriteLine("Saved " + pony.NewFilename);
            });
        }

        public static void RenderHtmlTable()
        {
            var ponies = ClassifyJson.DeserializeFile<PonyInfo[]>(@"D:\temp\ponies.json");
            Clipboard.SetText(ponies.Select(pony => $@"""{pony.Name.CLiteralEscape()}""").JoinString(", "));

            Ut.Assert(ponies.Length % 4 == 0);
            var top = ponies.Subarray(0, ponies.Length / 4);
            var right = ponies.Subarray(ponies.Length / 4, ponies.Length / 4);
            var bottom = ponies.Subarray(ponies.Length / 4 * 2, ponies.Length / 4).Reverse().ToArray();
            var left = ponies.Subarray(ponies.Length / 4 * 3, ponies.Length / 4).Reverse().ToArray();

            var imgCell = Ut.Lambda((PonyInfo[] arr, int i) => $@"<th><img class='fs' src='img/{arr[i].NewFilename}'>");

            var numCols = ponies.Length / 4;
            var table = $@"
            <table class='friendship'>
                <tr><th>{Enumerable.Range(0, ponies.Length / 4).Select(i => imgCell(top, i)).JoinString()}<th></tr>
                {Enumerable.Range(0, ponies.Length / 4).Select(i => $@"<tr>{imgCell(left, i)}[[]]{imgCell(right, i)}</tr>").JoinString()}
                <tr><th>{Enumerable.Range(0, ponies.Length / 4).Select(i => imgCell(bottom, i)).JoinString()}<th></tr>
            </table>
            ";
            var pos = table.IndexOf("[[]]");
            var tableStart = table.Substring(0, pos) + $"<td colspan='{numCols}' rowspan='{numCols}'>";
            var tableEnd = table.Substring(pos + "[[]]".Length).Replace("[[]]", "");

            var htmlFile = @"D:\c\KTANE\Friendship\Manual\Friendship.html";
            var html = File.ReadAllText(htmlFile);
            html = Regex.Replace(html, @"(?<=<!--##\{-->).*(?=<!--##\}-->)", tableStart, RegexOptions.Singleline);
            html = Regex.Replace(html, @"(?<=<!--###\{-->).*(?=<!--###\}-->)", tableEnd, RegexOptions.Singleline);
            File.WriteAllText(htmlFile, html);
        }

        private static MeshVertexInfo[] bpa(double x, double y, double z, Normal befX, Normal afX, Normal befY, Normal afY) { return new[] { pt(x, y, z, befX, afX, befY, afY) }; }
        private static Pt[] bpa(double x, double y, double z) { return new[] { pt(x, y, z) }; }

        private static IEnumerable<VertexInfo[]> Screen()
        {
            const double into = .05;
            return Ut.NewArray(
                // Bottom right
                bpa(-1 + into, 0, -.5 + into),

                // Top right
                quarterCircle(.5, into).Reverse().Select(p => pt(-1 + p.X, 0, 1 - p.Z)),

                // Top left
                bpa(1 - into, 0, 1 - into),

                // Bottom left
                quarterCircle(.5, into).Reverse().Select(p => pt(1 - p.X, 0, -.5 + p.Z)),

                null
            ).Where(x => x != null).SelectMany(x => x).SelectConsecutivePairs(true, (p1, p2) => new[] { pt(0, 0, .25), p1, p2 }.Select(p => new VertexInfo(p, pt(0, 1, 0), new PointD((p.X + 1) / 2, (p.Z + .5) / 1.5))).ToArray());
        }

        private static IEnumerable<VertexInfo[]> ScreenFrame()
        {
            var f = .0075;
            var h = .025;
            return CreateMesh(true, true,
                Bézier(p(.1, 0), p(.1, f), p(.1 - h + f, h), p(.1 - h, h), 20).Select((p, first, last) => new BevelPoint(p.X, p.Y, first || last ? Normal.Mine : Normal.Average, first || last ? Normal.Mine : Normal.Average)).Concat(
                Bézier(p(h, h), p(h - f, h), p(0, f), p(0, 0), 20).Select((p, first, last) => new BevelPoint(p.X, p.Y, first || last ? Normal.Mine : Normal.Average, first || last ? Normal.Mine : Normal.Average)))
                .Select(bi => Ut.NewArray(
                    // Bottom right
                    bpa(-1 + bi.Into, bi.Y, -.5 + bi.Into, bi.Before, bi.After, Normal.Mine, Normal.Mine),

                    // Top right
                    quarterCircle(.5, bi.Into).Reverse().Select((p, first, last) => pt(-1 + p.X, bi.Y, 1 - p.Z, bi.Before, bi.After, first || last ? Normal.Mine : Normal.Average, first || last ? Normal.Mine : Normal.Average)),

                    // Top left
                    bpa(1 - bi.Into, bi.Y, 1 - bi.Into, bi.Before, bi.After, Normal.Mine, Normal.Mine),

                    // Bottom left
                    quarterCircle(.5, bi.Into).Reverse().Select((p, first, last) => pt(1 - p.X, bi.Y, -.5 + p.Z, bi.Before, bi.After, first || last ? Normal.Mine : Normal.Average, first || last ? Normal.Mine : Normal.Average)),

                    null
                ).Where(x => x != null).SelectMany(x => x).ToArray()).ToArray());
        }

        private static IEnumerable<Pt> quarterCircle(double r, double i)
        {
            const int steps = 16;
            var startAngle = Math.Asin(i / (r + i)) * 180 / Math.PI;
            var angleSweep = 90 - 2 * startAngle;
            return Enumerable.Range(0, steps)
                .Select(s => startAngle + s * angleSweep / (steps - 1))
                .Select(θ => pt((r + i) * cos(θ), 0, (r + i) * sin(θ)));
        }

        private static IEnumerable<VertexInfo[]> SelectorFrame()
        {
            var f = .01;
            var h = .02;
            return CreateMesh(true, true,
                Bézier(p(.05, 0), p(.05, f), p(.05 - h + f, h), p(.05 - h, h), 20).Select((p, first, last) => new BevelPoint(p.X, p.Y, first || last ? Normal.Mine : Normal.Average, first || last ? Normal.Mine : Normal.Average)).Concat(
                Bézier(p(h, h), p(h - f, h), p(0, f), p(0, 0), 20).Select((p, first, last) => new BevelPoint(p.X, p.Y, first || last ? Normal.Mine : Normal.Average, first || last ? Normal.Mine : Normal.Average)))
                .Select(bi => Ut.NewArray(
                    // Bottom right
                    bpa(-1 + bi.Into, bi.Y, -1 + bi.Into, bi.Before, bi.After, Normal.Mine, Normal.Mine),

                    // Top right
                    bpa(-1 + bi.Into, bi.Y, -.55 - bi.Into, bi.Before, bi.After, Normal.Mine, Normal.Mine),

                    // Top left
                    bpa(.5 - bi.Into, bi.Y, -.55 - bi.Into, bi.Before, bi.After, Normal.Mine, Normal.Mine),

                    // Bottom left
                    bpa(.5 - bi.Into, bi.Y, -1 + bi.Into, bi.Before, bi.After, Normal.Mine, Normal.Mine),

                    null
                ).Where(x => x != null).SelectMany(x => x).ToArray()).ToArray());
        }

        private static IEnumerable<VertexInfo[]> SelectorBtn()
        {
            var f = .04;
            var h = .05;
            return CreateMesh(false, true,
                new BevelPoint(0, 0, Normal.Mine, Normal.Mine).Concat(
                Bézier(p(.12, 0), p(.12, f), p(.12 - h + f, h), p(.12 - h, h), 20).Select((p, first, last) => new BevelPoint(p.X, p.Y, first || last ? Normal.Mine : Normal.Average, first || last ? Normal.Mine : Normal.Average)))
                .Concat(new BevelPoint(0, h, Normal.Mine, Normal.Mine))
                .Select(bi => Enumerable.Range(0, 3)
                .Select(i => 360 * i / 3 + 90)
                .Select(angle => pt(bi.Into * cos(angle), bi.Y, bi.Into * sin(angle), bi.Before, bi.After, Normal.Mine, Normal.Mine))
                .ToArray()
            ).ToArray());
        }

        private static IEnumerable<VertexInfo[]> SubmitBtn()
        {
            var f = .03;
            var h = .05;
            var steps = 72;
            return CreateMesh(false, true,
                new BevelPoint(.00, .00, Normal.Mine, Normal.Mine).Concat(
                Bézier(p(.2, 0), p(.2, f), p(.2 - h + f, h), p(.2 - h, h), 20).Select((p, first, last) => new BevelPoint(p.X, p.Y, first || last ? Normal.Mine : Normal.Average, first || last ? Normal.Mine : Normal.Average)))
                .Concat(new BevelPoint(0, h, Normal.Mine, Normal.Mine))
                .Select(bi => Enumerable.Range(0, steps).Select(i => 360d * i / steps)
                .Select(angle => pt(bi.Into * cos(angle), bi.Y, bi.Into * sin(angle), bi.Before, bi.After, Normal.Average, Normal.Average)
            ).ToArray()).ToArray());
        }

        private static IEnumerable<VertexInfo[]> SelectorCylinder()
        {
            var radius = .3;
            var stemRadius = .05;

            var prelim = Ut.NewArray(
                new BevelPoint(1.3 - 0, stemRadius, Normal.Mine, Normal.Mine),
                new BevelPoint(.8 - .32, stemRadius, Normal.Mine, Normal.Theirs),
                new BevelPoint(.8 - .34, stemRadius, Normal.Theirs, Normal.Mine),
                new BevelPoint(.8 - .42, radius, Normal.Mine, Normal.Theirs),
                new BevelPoint(.8 - .44, radius, Normal.Mine, Normal.Mine),

                new BevelPoint(-1.3 + .44, radius, Normal.Mine, Normal.Mine),
                new BevelPoint(-1.3 + .42, radius, Normal.Theirs, Normal.Mine),
                new BevelPoint(-1.3 + .34, stemRadius, Normal.Mine, Normal.Theirs),
                new BevelPoint(-1.3 + .32, stemRadius, Normal.Theirs, Normal.Mine),
                new BevelPoint(-1.3 + 0, stemRadius, Normal.Mine, Normal.Mine)
            );

            return CreateMesh(false, true, prelim
                .Select(bi => Enumerable.Range(0, 7)
                    .Select(i => new { One = (i - 1d / 14 + .5) * 360d / 7, Two = (i + 1d / 14 + .5) * 360d / 7 })
                    .SelectMany(angles => Ut.NewArray(
                        pt(bi.Into, bi.Y * cos(angles.One), bi.Y * sin(angles.One), bi.Before, bi.After, Normal.Mine, Normal.Theirs),
                        pt(bi.Into, bi.Y * cos(angles.Two), bi.Y * sin(angles.Two), bi.Before, bi.After, Normal.Theirs, Normal.Mine)
                    )).ToArray())
                .ToArray());
        }

        private static IEnumerable<Pt[]> Triangle()
        {
            yield return Enumerable.Range(0, 3)
                .Select(i => 360 * i / 3 + 90)
                .Select(angle => pt(.2 * cos(angle), 0, .2 * sin(angle)))
                .Reverse()
                .ToArray();
        }
    }
}
