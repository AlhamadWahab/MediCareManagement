namespace DomainLayer.Interfaces.Bases_
{
    public interface IService<T> where T : class
    {
        /// <summary>
        /// Asynchronously retrieves all entities of type T.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a collection of entities of type T.</returns>
        public Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Asynchronously retrieves an entity of type T by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the entity of type T if found.</returns>
        public Task<T> GetByIdAsync(Guid id);
        /// <summary>
        /// Asynchronously adds a new entity of type T to the data store.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the added entity of type T.</returns>
        public Task<T> AddAsync(T entity);
        /// <summary>
        /// Asynchronously updates an existing entity of type T in the data store.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to update.</param>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated entity of type T.</returns>
        public Task<T> UpdateAsync(Guid id, T entity);
        /// <summary>
        /// Asynchronously deletes an entity of type T from the data store by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the deleted entity of type T.</returns>
        public Task<T> DeleteByIdAsync(Guid id);
    }
}
