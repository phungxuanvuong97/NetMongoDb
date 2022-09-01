﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public static class ThrowHelper
    {
        public static void ArgumentException(string message, string? paramName)
        {
            throw new ArgumentException(message, paramName);
        }

        public static void ArgumentNullException(string? paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        public static void KeyNotFoundException(string? message = null)
        {
            throw new KeyNotFoundException(message);
        }

        public static void InvalidOperationException(string? message = null)
        {
            throw new InvalidOperationException(message);
        }

        public static void InvalidCastException(string? message = null)
        {
            throw new InvalidCastException(message);
        }

        public static void NotSupportedException(string? message = null)
        {
            throw new NotSupportedException(message);
        }
    }
}
