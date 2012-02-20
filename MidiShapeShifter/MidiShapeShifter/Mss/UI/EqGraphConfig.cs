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
        /// <summary>
        /// Configures the appearance of an existing ZedGraphControl to be used as an equation 
        /// graph.
        /// </summary>
        public static void ConfigureEqGraph(ZedGraphControl eqGraph)
        {
            // get a reference to the GraphPane
            GraphPane pane = eqGraph.GraphPane;

            pane.Border.IsVisible = false;

            pane.Title.IsVisible = false;

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
        public static LineItem CreateEqCurve(string curveLabel)
        {
            LineItem eqCurve = new LineItem(curveLabel);
            eqCurve.Color = Color.FromArgb(0,151,217);
            eqCurve.Label.IsVisible = false;
            eqCurve.Symbol.IsVisible = false;

            //Setting IsSmooth to true will cause ZedGraphs to attempt to draw a smooth line 
            //connecting all points.
            eqCurve.Line.IsSmooth = true;
            //SmoothTension is a value from 0 to 1 that determines how smooth ZedGraphs tries to 
            //make the line. Lowering this value below 0.3 makes the like Y = X start to look bad.
            eqCurve.Line.SmoothTension = 0.3F;


            eqCurve.Line.Width = 3;

            eqCurve.Line.Fill = new Fill(Color.FromArgb(240, 240, 240), Color.FromArgb(203, 230, 242));

            return eqCurve;
        }
    }
}
