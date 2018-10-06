using System;
using System.ComponentModel;
using System.Globalization;

namespace CMDotNet.UnitTests
{
    [TypeConverter(typeof(MyNumConverter))]
    public class MyNum
    {
        public int Num { get; set; }

        public class MyNumConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return new MyNum { Num = int.Parse(value.ToString()) };
            }
        }
    }
}
