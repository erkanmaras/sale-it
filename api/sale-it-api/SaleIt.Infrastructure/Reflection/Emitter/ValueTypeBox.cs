
using System;

namespace SaleIt.Reflection.Emitter
{
    /// <summary>
    /// A wrapper for value type.  Must be used in order for Reflection to 
    /// work with value type such as struct.
    /// </summary>
    internal class ValueTypeBox
    {
        /// <summary>
        /// Creates a wrapper for <paramref name="value"/> value type.  The wrapper
        /// can then be used with Reflection.
        /// </summary>
        /// <param name="value">The value type to be wrapped.  
        /// Must be a derivative of <code>ValueType</code>.</param>
        public ValueTypeBox( object value )
        {
            Value = (ValueType) value;
        }

        /// <summary>
        /// The actual struct wrapped by this instance.
        /// </summary>
        public ValueType Value { get; set; }
    }
}