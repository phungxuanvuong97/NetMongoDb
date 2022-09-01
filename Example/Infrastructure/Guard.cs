using Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Guard
    {
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue NotNull<TValue>(TValue? target,
            [CallerArgumentExpression("target")] string? parameterName = null) where TValue : class
        {
            if (target == null)
            {
                ThrowHelper.ArgumentNullException(parameterName);
                return default!;
            }

            return target;
        }

    }
}
