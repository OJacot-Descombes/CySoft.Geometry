using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace VisualTests
{
    public static class Extensions
    {
        public static void DrawCircle(this Graphics g, Pen pen, Vector2 center, float radius)
        {
            g.DrawEllipse(pen, center.X - radius, center.Y - radius, radius + radius, radius + radius);
        }

        public static void FillCircle(this Graphics g, Brush brush, float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush, centerX - radius, centerY - radius, radius + radius, radius + radius);
        }

        public static PointF ToPointF(this Vector2 vector)
        {
            return new PointF(vector.X, vector.Y);
        }

        public static float Squared(this float value) => value * value;

        // From user ZombieSheep, https://stackoverflow.com/a/5796793/880990
        // It also handles strings like IBMMakeStuffAndSellIt, converting it to IBM Make Stuff And Sell It (IIRC).
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        public static void FillByEnum<TEnum>(this ListControl ctrl)
            where TEnum : Enum
        {
            Array enumArray = Enum.GetValues(typeof(TEnum));
            var values = enumArray
                .Cast<TEnum>()
                .Select(e => new KeyValuePair<TEnum, string>(e, e.ToString().SplitCamelCase()))
                .OrderBy(kvp => kvp.Value);
            var list = new List<KeyValuePair<TEnum, string>>(enumArray.Length);
            list.AddRange(values);
            ctrl.DataSource = list;
            ctrl.DisplayMember = "Value";
            ctrl.ValueMember = "Key";
        }
    }
}
