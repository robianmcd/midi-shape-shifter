using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using ZedGraph;

namespace MidiShapeShifter.Mss.UI
{
    /// <summary>
    /// This class is responsible for configuring the appearance of the equation graph 
    /// (used in PluginEditorView).
    /// </summary>
    public static class EqGraphConfig
    {
        private static readonly Color FILL_COLOR = Color.FromArgb(212, 235, 244); //H198 S13 V96
        private static readonly Color LINE_COLOR = Color.FromArgb(119, 189, 216); //H198 S45 V85
        private static readonly Color HIGHLIGHT_LINE_COLOR = Color.FromArgb(44, 140, 224); //H207 S75 V88

        /// <summary>
        /// Configures the appearance of an existing ZedGraphControl to be used as an equation 
        /// graph.
        /// </summary>
        public static void ConfigureEqGraph(ZedGraphControl eqGraph)
        {
            // get a reference to the GraphPane
            GraphPane pane = eqGraph.GraphPane;

            //Specifies how far away (in pixels) a click must be from an exsisting point to create a 
            //new point instead of dragging the exsisting point.
            ZedGraph.GraphPane.Default.NearestTol = 7;

            pane.Border.IsVisible = false;
            pane.Legend.IsVisible = false;
            pane.Title.IsVisible = false;
            pane.Margin.Bottom = 60;
            pane.Margin.Left = 30;

            pane.XAxis.Title.IsVisible = false;
            pane.XAxis.Scale.FontSpec.Size = 18;
            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = 1;
            pane.XAxis.Scale.MinorStep = 1;
            pane.XAxis.Scale.MajorStep = 1;

            pane.YAxis.Title.IsVisible = false;
            pane.YAxis.Scale.FontSpec.Size = 18;
            //The min and max values in the Y axis must be slightly outside of the range 0 to 1 so 
            //that lines at 0 and 1 are visible.
            pane.YAxis.Scale.Min = -0.005;
            pane.YAxis.Scale.Max = 1.01;
            pane.YAxis.Scale.MinorStep = 1;
            pane.YAxis.Scale.MajorStep = 1;
            pane.YAxis.MajorGrid.IsZeroLine = false;
        }

        /// <summary>
        ///     Creates and configures the appreadances of a LineItem for an equation curve.
        /// </summary>
        /// <param name="curveLabel">
        ///     Label for the LineItem being created. This can later be used to identify it.
        /// </param>
        /// <returns></returns>
        public static LineItem CreateEqCurve(string curveLabel, bool highlight)
        {
            LineItem eqCurve = new LineItem(curveLabel);
            if (highlight)
            {
                eqCurve.Color = HIGHLIGHT_LINE_COLOR;
                eqCurve.Line.Width = 2.5f;
            } 
            else
            {
                eqCurve.Color = LINE_COLOR;
                eqCurve.Line.Width = 2;
            }
            eqCurve.Label.IsVisible = false;
            eqCurve.Symbol.IsVisible = false;

            //Setting IsSmooth to true will cause ZedGraphs to attempt to draw a smooth line 
            //connecting all points.
            eqCurve.Line.IsSmooth = true;
            //SmoothTension is a value from 0 to 1 that determines how smooth ZedGraphs tries to 
            //make the line. Lowering this value below 0.3 makes the like Y = X start to look bad.
            eqCurve.Line.SmoothTension = 0.3F;


            eqCurve.Line.IsAntiAlias = true;
            eqCurve.Line.Fill = new Fill(FILL_COLOR);

            return eqCurve;
        }

        public static LineItem CreadControlPointsCurve(string curveLabel)
        {
            LineItem pointsCurve = new LineItem(curveLabel);

            //Configure appearance
            pointsCurve.Symbol = new Symbol(SymbolType.Circle, Color.Transparent);
            pointsCurve.Symbol.Fill = new Fill(LINE_COLOR, HIGHLIGHT_LINE_COLOR);
            pointsCurve.Symbol.Fill.Type = FillType.GradientByColorValue;
            pointsCurve.Symbol.Fill.SecondaryValueGradientColor = Color.Empty;
            pointsCurve.Symbol.Fill.RangeMin = 0;
            pointsCurve.Symbol.Fill.RangeMax = 1;
            pointsCurve.Symbol.Fill.RangeDefault = 0;
            pointsCurve.Symbol.Fill.IsVisible = true;
            pointsCurve.Symbol.Size = 17;
            pointsCurve.Symbol.IsAntiAlias = true;
            pointsCurve.Line.IsVisible = false;

            return pointsCurve;
        }
    }
}
