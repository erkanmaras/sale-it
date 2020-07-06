
using SaleIt.Reflection.Emitter;

namespace SaleIt.Reflection.Extensions
{
    /// <summary>
    /// Extension methods for working with types.
    /// </summary>
    public static class ValueTypeExtensions
    {
        ///<summary>
        /// Returns a wrapper <see cref="ValueTypeBox"/> instance if <paramref name="obj"/> 
        /// is a value type.  Otherwise, returns <paramref name="obj"/>.
        ///</summary>
        ///<param name="obj">An object to be examined.</param>
        ///<returns>A wrapper <seealso cref="ValueTypeBox"/> instance if <paramref name="obj"/>
        /// is a value type, or <paramref name="obj"/> itself if it's a reference type.</returns>
        public static object WrapIfValueType( this object obj )
        {
            return obj.GetType().IsValueType ? new ValueTypeBox( obj ) : obj;
        }

        ///<summary>
        /// Returns a wrapped object if <paramref name="obj"/> is an instance of <see cref="ValueTypeBox"/>.
        ///</summary>
        ///<param name="obj">An object to be "erased".</param>
        ///<returns>The object wrapped by <paramref name="obj"/> if the latter is of type <see cref="ValueTypeBox"/>.  Otherwise,
        /// return <paramref name="obj"/>.</returns>
        public static object UnwrapIfWrapped( this object obj )
        {
            var holder = obj as ValueTypeBox;
            return holder == null ? obj : holder.Value;
        }

        /// <summary>
        /// Determines whether <paramref name="obj"/> is a wrapped object (instance of <see cref="ValueTypeBox"/>).
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>Returns true if <paramref name="obj"/> is a wrapped object (instance of <see cref="ValueTypeBox"/>).</returns>
        public static bool IsWrapped( this object obj )
        {
            return obj as ValueTypeBox != null;
        }
    }
}