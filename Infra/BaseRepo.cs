using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace Infra
{
    public abstract class BaseRepo<T> : IRepo<T> where T : class
    {
        private DbSet<T> set;
        private DatabaseFacade db;

        protected BaseRepo(DbContext c = null, DbSet<T> s = null)
        {
            set = s;
            db = c ?.Database;
        }

        public Task<List<T>> Get() => throw new NotImplementedException();
        public Task<T> Get(int?id) => throw new NotImplementedException();
        public Task Delete(int?id) => throw new NotImplementedException();
        public Task Add(T obj) => throw new NotImplementedException();
        public Task Update(T obj) => throw new NotImplementedException();
        public T GetById (int?id) => throw new NotImplementedException();
    }

    
    
    
}
