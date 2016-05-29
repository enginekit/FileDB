﻿using System;
namespace Numeria.IO
{
    internal static class StringExtensions
    {
        public static byte[] ToBytes(this string str, int size)
        {
            if (string.IsNullOrEmpty(str))
                return new byte[size];
            

            //fixed size
            var buffer = new byte[size];
            var strbytes = System.Text.Encoding.UTF8.GetBytes(str);
            Array.Copy(strbytes, buffer, size > strbytes.Length ? strbytes.Length : size);

            return buffer;
        }
    }
}
