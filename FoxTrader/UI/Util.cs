using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FoxTrader.UI
{
    public static class Util
    {
        public static int Round(float c_x)
        {
            return (int)Math.Round(c_x, MidpointRounding.AwayFromZero);
        }

        public static int Ceil(float c_x)
        {
            return (int)Math.Ceiling(c_x);
        }

        public static Rectangle FloatRect(float c_x, float c_y, float c_w, float c_h)
        {
            return new Rectangle(Round(c_x), Round(c_y), Round(c_w), Round(c_h));
        }

        public static Rectangle ScaledRect(Rectangle c_rect, float c_scale)
        {
            return FloatRect(c_rect.X * c_scale, c_rect.Y * c_scale, c_rect.Width * c_scale, c_rect.Height * c_scale);
        }

        public static int Clamp(int c_x, int c_min, int c_max)
        {
            if (c_x < c_min)
            {
                return c_min;
            }

            return c_x > c_max ? c_max : c_x;
        }

        public static float Clamp(float c_x, float c_min, float c_max)
        {
            if (c_x < c_min)
            {
                return c_min;
            }

            return c_x > c_max ? c_max : c_x;
        }

        public static Rectangle ClampRectToRect(Rectangle c_inside, Rectangle c_outside, bool c_clampSize = false)
        {
            if (c_inside.X < c_outside.X)
            {
                c_inside.X = c_outside.X;
            }

            if (c_inside.Y < c_outside.Y)
            {
                c_inside.Y = c_outside.Y;
            }

            if (c_inside.Right > c_outside.Right)
            {
                if (c_clampSize)
                {
                    c_inside.Width = c_outside.Width;
                }
                else
                {
                    c_inside.X = c_outside.Right - c_inside.Width;
                }
            }
            if (c_inside.Bottom > c_outside.Bottom)
            {
                if (c_clampSize)
                {
                    c_inside.Height = c_outside.Height;
                }
                else
                {
                    c_inside.Y = c_outside.Bottom - c_inside.Height;
                }
            }

            return c_inside;
        }

        public static HSV ToHSV(this Color c_color)
        {
            var a_hsv = new HSV();
            var a_max = Math.Max(c_color.R, Math.Max(c_color.G, c_color.B));
            var a_min = Math.Min(c_color.R, Math.Min(c_color.G, c_color.B));

            a_hsv.m_h = c_color.GetHue();
            a_hsv.m_s = (a_max == 0) ? 0 : 1f - (1f * a_min / a_max);
            a_hsv.m_v = a_max / 255f;

            return a_hsv;
        }

        public static Color HSVToColor(float c_h, float c_s, float c_v)
        {
            var a_hi = Convert.ToInt32(Math.Floor(c_h / 60)) % 6;
            var a_f = c_h / 60 - (float)Math.Floor(c_h / 60);

            c_v = c_v * 255;
            var a_va = Convert.ToInt32(c_v);
            var a_p = Convert.ToInt32(c_v * (1 - c_s));
            var a_q = Convert.ToInt32(c_v * (1 - a_f * c_s));
            var a_t = Convert.ToInt32(c_v * (1 - (1 - a_f) * c_s));

            if (a_hi == 0)
            {
                return Color.FromArgb(255, a_va, a_t, a_p);
            }
            if (a_hi == 1)
            {
                return Color.FromArgb(255, a_q, a_va, a_p);
            }
            if (a_hi == 2)
            {
                return Color.FromArgb(255, a_p, a_va, a_t);
            }
            if (a_hi == 3)
            {
                return Color.FromArgb(255, a_p, a_q, a_va);
            }
            if (a_hi == 4)
            {
                return Color.FromArgb(255, a_t, a_p, a_va);
            }

            return Color.FromArgb(255, a_va, a_p, a_q);
        }

        // can't create extension operators
        public static Color Subtract(this Color c_color, Color c_other)
        {
            return Color.FromArgb(c_color.A - c_other.A, c_color.R - c_other.R, c_color.G - c_other.G, c_color.B - c_other.B);
        }

        public static Color Add(this Color c_color, Color c_other)
        {
            return Color.FromArgb(c_color.A + c_other.A, c_color.R + c_other.R, c_color.G + c_other.G, c_color.B + c_other.B);
        }

        public static Color Multiply(this Color c_color, float c_amount)
        {
            return Color.FromArgb(c_color.A, (int)(c_color.R * c_amount), (int)(c_color.G * c_amount), (int)(c_color.B * c_amount));
        }

        public static Rectangle Add(this Rectangle c_rect, Rectangle c_other)
        {
            return new Rectangle(c_rect.X + c_other.X, c_rect.Y + c_other.Y, c_rect.Width + c_other.Width, c_rect.Height + c_other.Height);
        }

        /// <summary>Splits a string but keeps the separators intact (at the end of split parts)</summary>
        /// <param name="c_text">String to split</param>
        /// <param name="c_separator">Separator characters</param>
        /// <returns>Split strings</returns>
        public static string[] SplitAndKeep(string c_text, string c_separator)
        {
            return Regex.Split(c_text, @"(?=[" + c_separator + "])");
        }
    }
}