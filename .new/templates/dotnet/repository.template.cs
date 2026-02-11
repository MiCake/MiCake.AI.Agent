/*
 * Repository Template for .NET
 * 
 * Variables:
 *   ${namespace} - Project namespace
 *   ${entity_name} - Entity class name
 *   ${entity_name_plural} - Plural form for DbSet
 *   ${id_type} - ID type (default: Guid)
 *   ${db_context} - DbContext class name
 *   ${custom_methods} - Additional query methods
 */

// ============================================
// Interface (Domain Layer)
// ============================================

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ${namespace}.Domain.Entities;

namespace ${namespace}.Domain.Repositories
{
    /// <summary>
    /// Repository interface for <see cref="${entity_name}"/> aggregate.
    /// </summary>
    public interface I${entity_name}Repository
    {
        /// <summary>
        /// Finds an entity by its identifier.
        /// </summary>
        Task<${entity_name}?> FindByIdAsync(${id_type} id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        Task AddAsync(${entity_name} entity, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        void Update(${entity_name} entity);
        
        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        void Remove(${entity_name} entity);
        
        /// <summary>
        /// Checks if an entity exists by its identifier.
        /// </summary>
        Task<bool> ExistsAsync(${id_type} id, CancellationToken cancellationToken = default);
        
        ${custom_method_interfaces}
    }
}

// ============================================
// Implementation (Infrastructure Layer)
// ============================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ${namespace}.Domain.Entities;
using ${namespace}.Domain.Repositories;
using ${namespace}.Infrastructure.Data;

namespace ${namespace}.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for <see cref="${entity_name}"/> aggregate.
    /// </summary>
    public class ${entity_name}Repository : I${entity_name}Repository
    {
        private readonly ${db_context} _context;
        
        public ${entity_name}Repository(${db_context} context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<${entity_name}?> FindByIdAsync(${id_type} id, CancellationToken cancellationToken = default)
        {
            return await _context.${entity_name_plural}
                ${include_statements}
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
        
        public async Task AddAsync(${entity_name} entity, CancellationToken cancellationToken = default)
        {
            await _context.${entity_name_plural}.AddAsync(entity, cancellationToken);
        }
        
        public void Update(${entity_name} entity)
        {
            _context.${entity_name_plural}.Update(entity);
        }
        
        public void Remove(${entity_name} entity)
        {
            _context.${entity_name_plural}.Remove(entity);
        }
        
        public async Task<bool> ExistsAsync(${id_type} id, CancellationToken cancellationToken = default)
        {
            return await _context.${entity_name_plural}
                .AnyAsync(e => e.Id == id, cancellationToken);
        }
        
        ${custom_method_implementations}
    }
}

/*
 * Custom Method Interface Template:
 * 
 * /// <summary>
 * /// ${method_description}
 * /// </summary>
 * Task<${return_type}> ${method_name}Async(${params}, CancellationToken cancellationToken = default);
 */

/*
 * Custom Method Implementation Template:
 * 
 * public async Task<${return_type}> ${method_name}Async(${params}, CancellationToken cancellationToken = default)
 * {
 *     return await _context.${entity_name_plural}
 *         .Where(e => ${condition})
 *         ${projection}
 *         .ToListAsync(cancellationToken);
 * }
 */

/*
 * Include Statement Template:
 * 
 * .Include(e => e.${navigation_property})
 */
