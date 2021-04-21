using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contoso.Core;
using Contoso.Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Contoso.Infra.Common
{

    public abstract class BaseRepo<TEntity, TData> : BaseRepo<TData>, IRepo<TEntity>
        where TData : BaseEntityData, IEntityData, new()
    {
        protected abstract TEntity toEntity(TData d);
        protected abstract TData toData(TEntity e);
        protected BaseRepo(DbContext c = null, DbSet<TData> s = null) : base(c, s) { }
        public new TEntity EntityInDb => toEntity(base.EntityInDb);
        public async new Task<List<TEntity>> Get() => (await base.Get()).Select(toEntity).ToList();
        public async new Task<TEntity> Get(int? id) => toEntity(await base.Get(id));
        public async Task<bool> Delete(TEntity e) => await Delete(toData(e));
        public async Task<bool> Add(TEntity e) => await Add(toData(e));
        public async Task<bool> Update(TEntity e) => await Update(toData(e));
        public new TEntity GetById(int? id) => toEntity(base.GetById(id));
    }


    public abstract class BaseRepo<T> : IRepo<T> where T : BaseEntityData, IEntityData, new()
    {
        private readonly DbSet<T> dbSet;
        private readonly DatabaseFacade db;
        public string ErrorMessage { get; protected set; }
        public T EntityInDb { get; protected set; }
        protected BaseRepo(DbContext c = null, DbSet<T> s = null)
        {
            dbSet = s;
            db = c?.Database;
        }
        public async Task<List<T>> Get() => await dbSet.AsNoTracking().ToListAsync();
        public async Task<T> Get(int? id)
        {
            if (id is null) return null;
            if (dbSet is null) return null;
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<bool> Delete(T obj)
        {
            var isOk = await isEntityOk(obj, ErrorMessages.ConcurrencyOnDelete);
            if (isOk) dbSet.Remove(obj);
            return isOk;
        }
        public async Task<bool> Add(T obj)
        {
            var isOk = await isEntityOk(obj, true);
            if (isOk) await dbSet.AddAsync(obj);
            return isOk;
        }
        public async Task<bool> Update(T obj)
        {
            var isOk = await isEntityOk(obj, ErrorMessages.ConcurrencyOnEdit);
            if (isOk) dbSet.Update(obj);
            return isOk;
        }
        public T GetById(int? id) => Get(id).GetAwaiter().GetResult();
        internal static bool byteArrayCompare(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2)
            => a1.SequenceEqual(a2);
        private bool errorMessage(string msg)
        {
            ErrorMessage = msg;
            return false;
        }
        internal async Task<bool> isEntityOk(T obj,
            string concurrencyErrorMsg)
        {
            return await isEntityOk(obj, false)
                   && isCorrectVersion(obj, concurrencyErrorMsg);
        }
        private async Task<bool> isEntityOk(T obj, bool isNewItem)
        {
            if (obj is null) return errorMessage("Item is null");
            if (dbSet is null) return errorMessage("DbSet is null");
            //DONE 10.4.01 - siin saab redigeerimise juures vea.
            //probleem selles, et ID baasklassis ja ID tuletatud klassis
            //on valesti tehtud.  Vaata kommentaare
            // DONE 10.04.02 ja DONE 10.04.03
            EntityInDb = await Get(obj.Id);
            return (EntityInDb is null) == isNewItem
                   || errorMessage(
                       isNewItem
                           ? $"Item with id = <{obj.Id}> already in database"
                           : $"No item with id = <{obj.Id}> in database");
        }
        internal bool isCorrectVersion(T obj,
            string concurrencyErrorMsg)
        {
            return byteArrayCompare(obj?.RowVersion, EntityInDb?.RowVersion)
                   || errorMessage(concurrencyErrorMsg);
        }
    }
}



