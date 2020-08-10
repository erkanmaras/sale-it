// Copyright (c) Arch team. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using SaleIt.Collections;
using SaleIt.Data.Extensions;
using SaleIt.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SaleIt.Data
{
    /// <summary>
    /// Represents a default generic repository implements the <see cref="IRepository"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityFrameworkRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public EntityFrameworkRepository(DbContext dbContext)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.DbSet = this.DbContext.Set<TEntity>();
        }

        protected DbContext DbContext { get; }
        protected DbSet<TEntity> DbSet { get; }

        ///// <summary>
        ///// Gets all entities. This method is not recommended
        ///// </summary>
        ///// <returns>The <see cref="IQueryable{TEntity}"/>.</returns>
        //public IQueryable<TEntity> GetAll()
        //{
        //    return DbSet;
        //}

        /// <summary>
        /// Gets all entities. This method is not recommended
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>true</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{T}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>Ex: This method defaults to a read-only, no-tracking query.</remarks>
        public IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }
        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, order by delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>>? predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                int pageIndex = 0,
                                                int pageSize = 20,
                                                bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedList(pageIndex, pageSize);
            }

            return query.ToPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, order by delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                           int pageIndex = 0,
                                                           int pageSize = 20,
                                                           bool disableTracking = true,
                                                           CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }

            return query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TResult}"/> based on a predicate,  order by delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TResult}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual IPagedList<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                         Expression<Func<TEntity, bool>>? predicate = null,
                                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                         int pageIndex = 0,
                                                         int pageSize = 20,
                                                         bool disableTracking = true)
            where TResult : class
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).ToPagedList(pageIndex, pageSize);
            }

            return query.Select(selector).ToPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate,  order by delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                                    Expression<Func<TEntity, bool>>? predicate = null,
                                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                                    int pageIndex = 0,
                                                                    int pageSize = 20,
                                                                    bool disableTracking = true,
                                                                    CancellationToken cancellationToken = default(CancellationToken))
            where TResult : class
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }

            return query.Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }

        /// <summary>
        /// Gets the first or default entity based on a predicate,  order by delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>>? predicate = null,
                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                         bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }

            return query.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the first or default entity based on a predicate,  order by delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        [return: MaybeNull]
        public virtual TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>>? predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                  bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).FirstOrDefault();
            }

            return query.Select(selector).FirstOrDefault();
        }

         
        public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>>? predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                  bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).Select(selector).FirstOrDefaultAsync();
            }

            return await query.Select(selector).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{TEntity}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public virtual IQueryable<TEntity> FromSql(string sql, params object[] parameters) => DbSet.FromSqlRaw(sql, parameters);

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The found entity or null.</returns>
        public virtual TEntity Find(params object[] keyValues) => DbSet.Find(keyValues);

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>A <see cref="Task{TEntity}" /> that represents the asynchronous insert operation.</returns>
        public virtual ValueTask<TEntity> FindAsync(params object[] keyValues) => DbSet.FindAsync(keyValues);

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{TEntity}"/> that represents the asynchronous find operation. The task result contains the found entity or null.</returns>
        public virtual ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken) => DbSet.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// Gets the count based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return DbSet.Count();
            }

            return DbSet.Count(predicate);
        }

        /// <summary>
        /// Gets async the count based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await DbSet.CountAsync();
            }

            return await DbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Gets the long count based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual long LongCount(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return DbSet.LongCount();
            }

            return DbSet.LongCount(predicate);
        }

        /// <summary>
        /// Gets async the long count based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await DbSet.LongCountAsync();
            }

            return await DbSet.LongCountAsync(predicate);
        }

        /// <summary>
        /// Gets the max based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        ///  /// <param name="selector"></param>
        /// <returns>decimal</returns>
        [return: MaybeNull]
        public virtual T Max<T>(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, T>> selector = null)
        {
            if (predicate == null)
            {
                return DbSet.Max(selector);
            }

            return DbSet.Where(predicate).Max(selector);
        }

        /// <summary>
        /// Gets the async max based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        ///  /// <param name="selector"></param>
        /// <returns>decimal</returns>
        public virtual async Task<T> MaxAsync<T>(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, T>>? selector = null)
        {
            if (predicate == null)
            {
                return await DbSet.MaxAsync(selector);
            }

            return await DbSet.Where(predicate).MaxAsync(selector);
        }

        /// <summary>
        /// Gets the min based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        [return: MaybeNull]
        public virtual T Min<T>(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, T>> selector = null)
        {
            if (predicate == null)
            {
                return DbSet.Min(selector);
            }

            return DbSet.Where(predicate).Min(selector);
        }

        /// <summary>
        /// Gets the async min based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        public virtual async Task<T> MinAsync<T>(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, T>>? selector = null)
        {
            if (predicate == null)
            {
                return await DbSet.MinAsync(selector);
            }

            return await DbSet.Where(predicate).MinAsync(selector);
        }

        /// <summary>
        /// Gets the average based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        public virtual decimal Average(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, decimal>> selector = null)
        {
            if (predicate == null)
            {
                return DbSet.Average(selector);
            }

            return DbSet.Where(predicate).Average(selector);
        }

        /// <summary>
        /// Gets the async average based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        public virtual async Task<decimal> AverageAsync(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, decimal>>? selector = null)
        {
            if (predicate == null)
            {
                return await DbSet.AverageAsync(selector);
            }

            return await DbSet.Where(predicate).AverageAsync(selector);
        }

        /// <summary>
        /// Gets the sum based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        public virtual decimal Sum(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, decimal>> selector = null)
        {
            if (predicate == null)
            {
                return DbSet.Sum(selector);
            }

            return DbSet.Where(predicate).Sum(selector);
        }

        /// <summary>
        /// Gets the async sum based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        public virtual async Task<decimal> SumAsync(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, decimal>>? selector = null)
        {
            if (predicate == null)
            {
                return await DbSet.SumAsync(selector);
            }

            return await DbSet.Where(predicate).SumAsync(selector);
        }

        /// <summary>
        /// Gets the exists based on a predicate.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<TEntity, bool>>? selector = null)
        {
            if (selector == null)
            {
                return DbSet.Any();
            }

            return DbSet.Any(selector);
        }
        /// <summary>
        /// Gets the async exists based on a predicate.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? selector = null)
        {
            if (selector == null)
            {
                return await DbSet.AnyAsync();
            }

            return await DbSet.AnyAsync(selector);
        }
        /// <summary>
        /// Inserts a new entity synchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public virtual TEntity Add(TEntity entity)
        {
            return DbSet.Add(entity).Entity;
        }

        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public virtual void Add(params TEntity[] entities) => DbSet.AddRange(entities);

        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public virtual void Add(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public virtual ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbSet.AddAsync(entity, cancellationToken);

            // Shadow properties?
            //var property = _dbContext.Entry(entity).Property("Created");
            //if (property != null) {
            //property.CurrentValue = DateTime.Now;
            //}
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous insert operation.</returns>
        public virtual Task AddAsync(params TEntity[] entities) => DbSet.AddRangeAsync(entities);

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public virtual Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken)) => DbSet.AddRangeAsync(entities, cancellationToken);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);

        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Update(params TEntity[] entities) => DbSet.UpdateRange(entities);

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Update(IEnumerable<TEntity> entities) => DbSet.UpdateRange(entities);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Remove(TEntity entity) => DbSet.Remove(entity);

        /// <summary>
        /// Removes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        public virtual void Remove(object id)
        {
            // using a stub entity to mark for deletion
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = DbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            PropertyInfo? property = null;
            if (key != null)
            {
                property = typeInfo.GetProperty(key.Name);
            }

            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                DbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = DbSet.Find(id);
                if (entity != null)
                {
                    Remove(entity);
                }
            }
        }

        /// <summary>
        /// Removes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Remove(params TEntity[] entities) => DbSet.RemoveRange(entities);

        /// <summary>
        /// Removes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Remove(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);

        ///// <summary>
        ///// Gets all entities. This method is not recommended
        ///// </summary>
        ///// <returns>The <see cref="IQueryable{TEntity}"/>.</returns>
        //public async Task<IList<TEntity>> GetAllAsync()
        //{
        //    return await DbSet.ToListAsync();
        //}

        /// <summary>
        /// Gets all entities. This method is not recommended
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>true</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>Ex: This method defaults to a read-only, no-tracking query.</remarks>
        public async Task<IList<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Change entity state for patch method on web api.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// /// <param name="state">The entity state.</param>
        public void ChangeEntityState(TEntity entity, EntityState state)
        {
            DbContext.Entry(entity).State = state;
        }
    }
}
