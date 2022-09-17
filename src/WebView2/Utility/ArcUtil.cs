using System;
using System.Windows;
using System.Windows.Media;

namespace WebView2.Utility;

public static class ArcUtil
{
    public static PathGeometry CreateCircleGeometry(double width, double height, int thickness)
    {
        Point center      = new(width / 2, height / 2);
        Point top         = new(center.X, 0);
        Point innerTop    = new(center.X, thickness);
        Point bottom      = new(center.X, height);
        Point innerBottom = new(center.X, height - thickness);
            
        // create segments
        var psc = new PathSegmentCollection {
            CreateOuterArc(bottom, width, height, true),
            CreateOuterArc(top, width, height, true),
            new LineSegment(innerTop, false),
            CreateInnerArc(innerBottom, width, height, thickness, true),
            CreateInnerArc(innerTop, width, height, thickness, true),
        };

        // create geometry
        return new PathGeometry(new PathFigureCollection { new(top, psc, true) });
    }
        
    public static PathGeometry CreateArcGeometry(double width, double height, int thickness, double angle)
    {
        double radius = width / 2;
        Point  center = new(width / 2, height / 2);

        // center top + thickness from the top
        var startLine = new Point(center.X, thickness);
        var startArc  = new Point(center.X, 0);
        var largeArc  = angle > 180;

        // rotate angle
        angle = 180 - angle;

        // calculate inner and outer arc points
        var outer = CreateArcPoint(center, radius, angle);
        var inner = CreateArcPoint(center, radius - thickness, angle);

        // create segments
        var psc = new PathSegmentCollection {
            new LineSegment(startArc, false),
            CreateOuterArc(outer, width, height, largeArc),
            new LineSegment(inner, false),
            CreateInnerArc(startLine, width, height, thickness, largeArc)
        };

        // create geometry
        return new PathGeometry(new PathFigureCollection { new(startLine, psc, true) });
    }

    public static Point CreateArcPoint(Point center, double radius, double angle)
    {
        var x = center.X + radius * Math.Sin(Math.PI * 2 * angle / 360);
        var y = center.Y + radius * Math.Cos(Math.PI * 2 * angle / 360);

        // create point with 2 decimal precision
        return new Point(Math.Round(100 * x) / 100, Math.Round(100 * y) / 100);
    }

    public static ArcSegment CreateOuterArc(Point end, double width, double height, bool largeArc)
    {
        return new ArcSegment(end, new Size(width / 2, height / 2), 0, largeArc, SweepDirection.Clockwise, false);
    }

    public static ArcSegment CreateInnerArc(Point end, double width, double height, int thickness, bool largeArc)
    {
        var size = new Size(width / 2 - thickness, height / 2 - thickness);
        return new ArcSegment(end, size, 0, largeArc, SweepDirection.Counterclockwise, false);
    }
}