using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WebView2.Utility;

namespace WebView2.UserControls;

public class ArcShape : Shape
{
    /// <summary>
    /// Identified the Angle dependency property
    /// </summary>
    public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
        "Angle",
        typeof(int),
        typeof(ArcShape),
        new PropertyMetadata(0, ShapePropertyChanged)
    );

    /// <summary>
    /// Identified the Angle dependency property
    /// </summary>
    public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
        "Thickness",
        typeof(int),
        typeof(ArcShape),
        new PropertyMetadata(5, ShapePropertyChanged)
    );

    public ArcShape()
    {
        Stretch = Stretch.None;
    }

    public int Angle
    {
        get => (int)GetValue(AngleProperty);
        set => SetValue(AngleProperty, value);
    }

    public int Thickness
    {
        get => (int)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }

    /// <summary>
    /// Listen for angle and thickness changes, then repaint
    /// </summary>
    private static void ShapePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // trigger repaint
        ((ArcShape)d).InvalidateVisual();
    }

    protected override Size MeasureOverride(Size constraint)
    {
        if (double.IsPositiveInfinity(constraint.Width) || double.IsPositiveInfinity(constraint.Height)) {
            return Size.Empty;
        }

        // we will size ourselves to fit the available space
        return constraint;
    }

    protected override Geometry DefiningGeometry
    {
        get
        {
            // ArcSegment is kinda quirky and can't draw a full circle. Draw 2x half arc to make a circle.
            return Angle == 360
                ? ArcUtil.CreateCircleGeometry(DesiredSize.Width, DesiredSize.Height, Thickness)
                : ArcUtil.CreateArcGeometry(DesiredSize.Width, DesiredSize.Height, Thickness, Angle);
        }
    }
}