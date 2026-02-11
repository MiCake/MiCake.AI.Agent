"""
Entity Template for Python

Variables:
    ${module} - Module name
    ${entity_name} - Entity class name (PascalCase)
    ${entity_name_snake} - Entity name in snake_case
    ${id_type} - ID type (default: UUID)
    ${entity_base} - Base class for entities
    ${properties} - Property definitions
    ${methods} - Business methods
"""

from dataclasses import dataclass, field
from typing import List, Optional
from uuid import UUID, uuid4
from datetime import datetime

from ${module}.domain.shared.entity import ${entity_base}
from ${module}.domain.events import *


@dataclass
class ${entity_name}(${entity_base}[${id_type}]):
    """
    ${entity_description}
    """
    
    # ============================================
    # Properties
    # ============================================
    
    ${properties}
    
    # ============================================
    # Factory Methods
    # ============================================
    
    @classmethod
    def create(cls, ${factory_params}) -> "${entity_name}":
        """
        Creates a new ${entity_name} instance.
        
        Args:
            ${factory_args_doc}
            
        Returns:
            ${entity_name}: The created entity.
            
        Raises:
            ValueError: If validation fails.
        """
        # Validate invariants
        ${factory_validations}
        
        entity = cls(
            id=${id_assignment},
            ${factory_assignments}
        )
        
        # Register creation event
        entity._register_domain_event(${entity_name}CreatedEvent(entity))
        
        return entity
    
    # ============================================
    # Business Methods
    # ============================================
    
    ${methods}
    
    # ============================================
    # Invariant Validation
    # ============================================
    
    def _validate_invariants(self) -> None:
        """Validates entity invariants."""
        ${invariant_validations}


# ============================================
# Property Template:
# 
# ${property_name}: ${property_type}
# ============================================

# ============================================
# Method Template:
# 
# def ${method_name}(self, ${method_params}) -> ${return_type}:
#     """
#     ${method_description}
#     
#     Args:
#         ${method_args_doc}
#         
#     Returns:
#         ${return_description}
#         
#     Raises:
#         ${raises_doc}
#     """
#     # Validate preconditions
#     ${method_validations}
#     
#     # Perform operation
#     ${method_body}
#     
#     # Raise domain event if needed
#     self._register_domain_event(${event_name}(self))
# ============================================

# ============================================
# Validation Template:
# 
# if not ${param_name}:
#     raise ValueError("${param_name} cannot be empty")
# 
# if ${condition}:
#     raise ValueError("${message}")
# ============================================
