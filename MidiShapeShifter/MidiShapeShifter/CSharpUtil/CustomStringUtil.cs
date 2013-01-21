using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MidiShapeShifter.CSharpUtil
{
    public static class CustomStringUtil
    {
        public static string CreateStringWithMaxWidth(string inStr, int maxPixelWidth, Font font)
        {
            Size strSize = TextRenderer.MeasureText(inStr, font);
            if (strSize.Width <= maxPixelWidth)
            {
                return inStr;
            }
            else
            {
                int numCharsInOutString = 0;
                string ellipse = "...";
                int stringWidth = TextRenderer.MeasureText(ellipse, font).Width;

                while (stringWidth < maxPixelWidth)
                {
                    numCharsInOutString++;
                    string curString = inStr.Substring(0, numCharsInOutString) + ellipse;
                    stringWidth = TextRenderer.MeasureText(curString, font).Width;
                }
                numCharsInOutString--;

                if (numCharsInOutString < 0)
                {
                    return "";
                }
                else
                {
                    return inStr.Substring(0, numCharsInOutString) + ellipse;                    
                }
            }
            
        }
    }
}
