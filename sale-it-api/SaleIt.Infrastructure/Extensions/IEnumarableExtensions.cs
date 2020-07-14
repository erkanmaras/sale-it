using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SaleIt.Infrastructure.Extensions
{
  public static class IEnumarableExtensions
    {
        /// <summary>
        /// Determines whether an IEnumerable contains any item
        /// </summary>
        /// <param name="enumerable">the IEnumerable</param>
        /// <returns>false if enumerable is null or contains no items</returns>
        public static bool HasItem([NotNullWhen(true)] this IEnumerable? enumerable)
        {
            if (enumerable == null)
            {
                return false;
            }

            try
            {
                var enumerator = enumerable.GetEnumerator();
                if (enumerator != null && enumerator.MoveNext())
                {
                    return true;
                }
            }
            catch
            {
                // ignored
            }

            return false;
        }
    }
}
