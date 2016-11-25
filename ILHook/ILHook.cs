namespace ILHook
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///     Hooks and replaces methods with another method.
    /// </summary>
    public static class ILHook
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this program is running in 32-bit enviroment.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this program is running in 32-bit enviroment; otherwise, <c>false</c>.
        /// </value>
        private static bool Is32Bit => IntPtr.Size == 4;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Replaces the method with another method.
        /// </summary>
        /// <param name="target">The target method.</param>
        /// <param name="replacement">The replacement method.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ReplaceMethod(this MethodInfo target, MethodInfo replacement)
        {
            // Force methods to be compiled by JIT (just in case)
            RuntimeHelpers.PrepareMethod(target.MethodHandle);
            RuntimeHelpers.PrepareMethod(replacement.MethodHandle);

            unsafe
            {
                if (Is32Bit)
                {
                    var replacementPointer = (int*)replacement.MethodHandle.Value.ToPointer() + 2;
                    var targetPointer = (int*)target.MethodHandle.Value.ToPointer() + 2;

#if DEBUG
                    *(int*)((byte*)*targetPointer + 1) = (int)(byte*)*replacementPointer + 5
                                                         + *(int*)((byte*)*replacementPointer + 1)
                                                         - ((int)(byte*)*targetPointer + 5);
#else
                    *targetPointer = *replacementPointer;
#endif
                }
                else
                {
                    var replacementPointer = (long*)replacement.MethodHandle.Value.ToPointer() + 1;
                    var targetPointer = (long*)target.MethodHandle.Value.ToPointer() + 1;
#if DEBUG
                    *(int*)((byte*)*targetPointer + 1) = (int)(byte*)*replacementPointer + 5
                                                         + *(int*)((byte*)*replacementPointer + 1)
                                                         - ((int)(byte*)*targetPointer + 5);
#else
                    *targetPointer = *replacementPointer;
#endif
                }
            }
        }

        #endregion
    }
}