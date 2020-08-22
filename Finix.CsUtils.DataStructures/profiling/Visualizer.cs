using System.IO;
using System.Net.Mime;
using System;
using System.Linq;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Collections.Generic;
using SixLabors.Fonts;
using System.Drawing.Text;
using Path = SixLabors.ImageSharp.Drawing.Path;
using FontCollection = SixLabors.Fonts.FontCollection;

namespace Finix.CsUtils.DataStructures.Profiling
{
    public class Visualizer
    {
        private IBrush foregroundBrush;

        private ShapeGraphicsOptions shapeGraphicsOptions;

        private int edgeThickness;

        private float layerHeight;

        private float nodeHeight, nodeSegmentWidth;

        private int pageMargin;

        private List<Drawable> drawables;

        private PointF furthestPoint;

        private Font font;

        private Visualizer()
        {
            var fontDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts);

            var dir = new DirectoryInfo(fontDir);
            var arialFile = dir.EnumerateFiles("arial.ttf").First();

            var fonts = new FontCollection();
            var arial = fonts.Install(arialFile.FullName);

            shapeGraphicsOptions = new ShapeGraphicsOptions();
            foregroundBrush = new SolidBrush(Color.Black);
            edgeThickness = 1;
            font = arial.CreateFont(12);

            nodeHeight = 24;
            nodeSegmentWidth = 50;
            layerHeight = nodeHeight * 3;

            pageMargin = 12;

            drawables = new List<Drawable>();
        }

        private void EnsurePoints(Image image, params PointF[] points)
        {
            var maxX = points.Max(point => point.X);
            var maxY = points.Max(point => point.Y);

            image.Mutate(ctx => {

                var w = (int) MathF.Ceiling(MathF.Max(image.Width, maxX + pageMargin));
                var h = (int) MathF.Ceiling(MathF.Max(image.Height, maxY + pageMargin));

                var center = new Point((w - image.Width) / 2, 0);
                var source = new Rectangle(Point.Empty, image.Size());
                var target = new Rectangle(center, image.Size());

                ctx.Resize(w, h, KnownResamplers.Bicubic, source, target, false);
            });
        }

        private void DrawLine(params PointF[] segments)
        {
            var prev = segments.First();
            var path = new Path(segments.Skip(1).Select(p => {
                var segment = new LinearLineSegment(prev, p);
                prev = p;

                return segment;
            }));

            drawables.Add(new PathDrawer { Path = path });
        }

        private void DrawText(PointF point, string text)
        {
            drawables.Add(new TextDrawer {
                Location = point,
                Text = text
            });
        }

        private void DrawRect(RectangleF rect)
        {
            var (x, y, w, h) = rect;

            var p1 = new PointF(x, y);
            var p2 = new PointF(x + w, y);
            var p3 = new PointF(x + w, y + h);
            var p4 = new PointF(x, y + h);

            DrawLine(p1, p2, p3, p4, p1);
        }

        private void DrawCell(RectangleF rect, string text)
        {
            DrawRect(rect);
            DrawText(new PointF(rect.X, rect.Y) + new PointF(2, 2), text);
        }

        private int DecidePosition(int parent_pos, int child, int children, int depth)
        {
            children++;

            // var parent_slots = Math.Pow(children, (depth - 1));
            // var slots = parent_slots * children;

            return parent_pos * children + child;
        }

        private void DrawNode<TKey, TValue>(int arity, int depth, int maxDepth, int parent_pos, PointF parent, IBTreeNode<TKey, TValue> node)
        {
            // if (depth == 0)
            //     arity = 2;

            var a = arity; // depth == 0 ? 2 :
            var pa = node.Owner?.ArityIndex ?? 0;
            var child = 0;

            var pos = 0;

            float DecideX()
            {
                pos = DecidePosition(parent_pos, child, a, depth);

                var x = pos * nodeSegmentWidth;

                // if (depth == 0)
                    return x;

                // return x - (parent_pos * nodeSegmentWidth / 2); // (maxDepth - depth) * (a + 1);
            }

            var point = new PointF(DecideX(), depth * layerHeight);

            DrawLine(parent, point);

            foreach (var value in node.Values)
            {
                var rect = new RectangleF(DecideX(), point.Y, nodeSegmentWidth, nodeHeight);

                if (value.IsAvailable)
                {
                    var txt = $"{value.Key}: {value.Value}";

                    DrawCell(rect, txt);
                }

                var p = new PointF(rect.Left, rect.Bottom);

                // var off = (xOff - a / 2);

                if (value.LesserNode != null)
                    DrawNode(arity, depth + 1, maxDepth, pos, p, value.LesserNode);

                child++;
            }
        }

        private Image Commit()
        {
            if (drawables.Count == 0)
                throw new InvalidOperationException("Must first add at least one node");

            var x = (int) Math.Ceiling(drawables.Max(d => d.FurthestPoint(this).X));
            var y = (int) Math.Ceiling(drawables.Max(d => d.FurthestPoint(this).Y));

            var w = x + pageMargin * 2;
            var h = y + pageMargin * 2;

            var center = new Point(pageMargin, pageMargin);

            var image = new Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(w, h);

            image.Mutate(ctx => {

                ctx.Fill(Color.White);

                foreach (var drawable in drawables)
                {
                    drawable.Draw(this, center, ctx);
                }
            });

            return image;
        }

        public static void Visualize<TKey, TValue>(IBTree<TKey, TValue> tree, string fileName = "tree")
        {
            var v = new Visualizer();
            v.DrawNode(tree.Arity, 0, tree.Depth, 0, PointF.Empty, tree.RootNode);

            using var fileStream = File.Open($"{fileName}.bmp", FileMode.Create);
            v.Commit().SaveAsBmp(fileStream);
        }

        private abstract class Drawable
        {
            public abstract PointF FurthestPoint(Visualizer v);

            public abstract void Draw(Visualizer v, PointF center, IImageProcessingContext context);
        }

        private class TextDrawer : Drawable
        {
            public string Text;
            public PointF Location;

            public override void Draw(Visualizer v, PointF center, IImageProcessingContext context)
            {
                context.DrawText(Text, v.font, v.foregroundBrush, Location + center);
            }

            public override PointF FurthestPoint(Visualizer v)
            {
                return PointF.Empty;

                var len = Text.Length * v.font.EmSize;
                var h = v.font.LineHeight;

                return Location.X > 0
                    ? Location + new PointF(len, h)
                    : Location + new PointF(0, h);
            }
        }

        private class PathDrawer : Drawable
        {
            public IPath Path;

            public override void Draw(Visualizer v, PointF center, IImageProcessingContext context)
            {
                var path = Path.Translate(center);
                context.Draw(v.shapeGraphicsOptions, v.foregroundBrush, v.edgeThickness, new PathCollection(path));
            }

            public override PointF FurthestPoint(Visualizer v)
            {
                var x = Math.Max(MathF.Abs(Path.Bounds.Left), MathF.Abs(Path.Bounds.Right));
                var y = Math.Max(MathF.Abs(Path.Bounds.Top), MathF.Abs(Path.Bounds.Bottom));

                return new PointF(x, y);
            }
        }
    }
}
