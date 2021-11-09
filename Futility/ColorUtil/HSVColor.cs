using System;
using UnityEngine;

namespace FUtility
{
    public struct HSVColor
    {
        private const float TRESHOLD = 0.1f;

        public float h;
        public float s;
        public float v;
        public float a;

        public Color RGB
        {
            get
            {
                Color color = Color.HSVToRGB(h, s, v);
                color.a = a;
                return color;
            }
        }

        public override string ToString()
        {
            return string.Format("HSVA({0:F3}, {1:F3}, {2:F3}, {3:F3})", h, s, v, a);
        }

        public Color Pure => Color.HSVToRGB(h, 1f, 1f);
        public Color Desat => new HSVColor(0, 0, v);

        public HSVColor(Color RGBColor)
        {
            Color.RGBToHSV(RGBColor, out h, out s, out v);
            a = RGBColor.a;
        }

        public HSVColor(float h, float s, float v)
        {
            this.h = h;
            this.s = s;
            this.v = v;
            a = 1f;
        }

        public HSVColor(float h, float s, float v, float a) : this(h, s, v)
        {
            this.a = a;
        }

        public static implicit operator Vector4(HSVColor c)
        {
            return new Vector4(c.h, c.s, c.v, c.a);
        }

        public static implicit operator HSVColor(Vector4 v)
        {
            return new HSVColor(v.x, v.y, v.z, v.w);
        }

        public static implicit operator Color(HSVColor hsv)
        {
            return hsv.RGB;
        }

        public static implicit operator HSVColor(Color rgb)
        {
            return rgb.HSV();
        }

        public static bool operator ==(HSVColor lhs, HSVColor rhs)
        {
            // probably not what i need, i think i need to check for the floats separately, but for now whatever
            return Mathf.Abs(Vector4.Distance(lhs, rhs)) < TRESHOLD;
        }

        public static bool operator !=(HSVColor lhs, HSVColor rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (other is Color)
                return Mathf.Abs(Vector4.Distance((Color)other, RGB)) < TRESHOLD;
            if (other is HSVColor)
                return Mathf.Abs(Vector4.Distance((HSVColor)other, this)) < TRESHOLD;
            return false;
        }

        public override int GetHashCode() => GetHashCode();

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return h;
                    case 1:
                        return s;
                    case 2:
                        return v;
                    case 3:
                        return a;
                    default:
                        throw new IndexOutOfRangeException("Invalid index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        h = value;
                        break;
                    case 1:
                        s = value;
                        break;
                    case 2:
                        v = value;
                        break;
                    case 3:
                        a = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid index!");
                }
            }
        }
    }
}