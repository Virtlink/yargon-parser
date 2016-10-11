using System;

namespace Slang
{
    /// <summary>
    /// The entry point class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The entry point method.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        private static void Main(string[] args)
        {
            #region Contract
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            #endregion

            throw new NotImplementedException();
        }
    }
}
