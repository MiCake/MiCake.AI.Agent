/*
 * Value Object Template for .NET
 * 
 * Variables:
 *   ${namespace} - Project namespace
 *   ${vo_name} - Value object class name (PascalCase)
 *   ${vo_base} - Base class for value objects
 *   ${properties} - Property definitions
 *   ${constructor_params} - Constructor parameters
 *   ${equality_members} - Properties to include in equality
 */

using System;
using System.Collections.Generic;
using ${namespace}.Domain.Shared;

namespace ${namespace}.Domain.ValueObjects
{
    /// <summary>
    /// ${vo_description}
    /// </summary>
    public sealed class ${vo_name} : ${vo_base}
    {
        #region Properties
        
        ${properties}
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// Creates a new <see cref="${vo_name}"/> instance.
        /// </summary>
        public ${vo_name}(${constructor_params})
        {
            // Validate invariants
            ${validations}
            
            // Set properties
            ${assignments}
        }
        
        #endregion
        
        #region Equality
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            ${equality_members}
        }
        
        #endregion
        
        #region Transformation Methods
        
        ${transformation_methods}
        
        #endregion
        
        #region Factory Methods
        
        ${factory_methods}
        
        #endregion
        
        public override string ToString() => $"${to_string_format}";
    }
}

/*
 * Property Template (readonly):
 * 
 * /// <summary>
 * /// ${property_description}
 * /// </summary>
 * public ${property_type} ${property_name} { get; }
 */

/*
 * Equality Component Template:
 * 
 * yield return ${property_name};
 */

/*
 * Transformation Method Template:
 * 
 * /// <summary>
 * /// Creates a new ${vo_name} with updated ${property_name}.
 * /// </summary>
 * public ${vo_name} With${property_name}(${property_type} new${property_name})
 * {
 *     return new ${vo_name}(${other_properties}, new${property_name});
 * }
 */

/*
 * Validation Template:
 * 
 * if (string.IsNullOrWhiteSpace(${param_name}))
 *     throw new ArgumentException("${param_name} cannot be empty.", nameof(${param_name}));
 * 
 * if (${param_name} < 0)
 *     throw new ArgumentOutOfRangeException(nameof(${param_name}), "${param_name} cannot be negative.");
 */
