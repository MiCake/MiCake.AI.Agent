/*
 * Entity Template for .NET
 * 
 * Variables:
 *   ${namespace} - Project namespace
 *   ${entity_name} - Entity class name (PascalCase)
 *   ${id_type} - ID type (default: Guid)
 *   ${entity_base} - Base class for entities
 *   ${properties} - Property definitions
 *   ${constructor_params} - Constructor parameters
 *   ${methods} - Business methods
 */

using System;
using System.Collections.Generic;
using ${namespace}.Domain.Shared;

namespace ${namespace}.Domain.Entities
{
    /// <summary>
    /// ${entity_description}
    /// </summary>
    public class ${entity_name} : ${entity_base}<${id_type}>
    {
        #region Properties
        
        ${properties}
        
        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Creates a new <see cref="${entity_name}"/> instance.
        /// </summary>
        public ${entity_name}(${constructor_params})
        {
            // Validate invariants
            ${constructor_validations}
            
            // Set properties
            ${constructor_assignments}
        }
        
        /// <summary>
        /// Private constructor for ORM.
        /// </summary>
        private ${entity_name}() { }
        
        #endregion
        
        #region Business Methods
        
        ${methods}
        
        #endregion
        
        #region Domain Events
        
        ${domain_events}
        
        #endregion
    }
}

/*
 * Property Template:
 * 
 * /// <summary>
 * /// ${property_description}
 * /// </summary>
 * public ${property_type} ${property_name} { get; private set; }
 */

/*
 * Method Template:
 * 
 * /// <summary>
 * /// ${method_description}
 * /// </summary>
 * public ${return_type} ${method_name}(${method_params})
 * {
 *     // Validate preconditions
 *     ${method_validations}
 *     
 *     // Perform operation
 *     ${method_body}
 *     
 *     // Raise domain event if needed
 *     AddDomainEvent(new ${event_name}(this));
 * }
 */

/*
 * Validation Template:
 * 
 * if (${condition})
 * {
 *     throw new ArgumentException("${message}", nameof(${param_name}));
 * }
 */
