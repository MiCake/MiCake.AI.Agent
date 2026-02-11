/**
 * Entity Template for Java
 * 
 * Variables:
 *   ${package} - Package name
 *   ${entity_name} - Entity class name (PascalCase)
 *   ${id_type} - ID type (default: UUID)
 *   ${entity_base} - Base class for entities
 *   ${properties} - Property definitions
 *   ${constructor_params} - Constructor parameters
 *   ${methods} - Business methods
 */

package ${package}.domain.entities;

import java.util.*;
import ${package}.domain.shared.*;
import ${package}.domain.events.*;

/**
 * ${entity_description}
 */
public class ${entity_name} extends ${entity_base}<${id_type}> {
    
    // ============================================
    // Properties
    // ============================================
    
    ${properties}
    
    // ============================================
    // Constructors
    // ============================================
    
    /**
     * Creates a new {@link ${entity_name}} instance.
     */
    public ${entity_name}(${constructor_params}) {
        super(${id_assignment});
        
        // Validate invariants
        ${constructor_validations}
        
        // Set properties
        ${constructor_assignments}
    }
    
    /**
     * Protected constructor for JPA.
     */
    protected ${entity_name}() {
        super();
    }
    
    // ============================================
    // Getters
    // ============================================
    
    ${getters}
    
    // ============================================
    // Business Methods
    // ============================================
    
    ${methods}
    
    // ============================================
    // Domain Events
    // ============================================
    
    ${domain_events}
}

/*
 * Property Template:
 * 
 * private ${property_type} ${property_name};
 */

/*
 * Getter Template:
 * 
 * /**
 *  * Gets the ${property_description}.
 *  */
 * public ${property_type} get${PropertyName}() {
 *     return ${property_name};
 * }
 */

/*
 * Method Template:
 * 
 * /**
 *  * ${method_description}
 *  */
 * public ${return_type} ${method_name}(${method_params}) {
 *     // Validate preconditions
 *     ${method_validations}
 *     
 *     // Perform operation
 *     ${method_body}
 *     
 *     // Raise domain event if needed
 *     registerDomainEvent(new ${event_name}(this));
 * }
 */

/*
 * Validation Template:
 * 
 * Objects.requireNonNull(${param_name}, "${param_name} cannot be null");
 * 
 * if (${condition}) {
 *     throw new IllegalArgumentException("${message}");
 * }
 */
