using EM.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EM.Repository;

namespace EM.Repository
{
    public abstract class RepositorioAbstrato<T> where T : IEntidade
    {
        public virtual void Add(T objeto) { }
        public virtual void Remove(T objeto) { }
        public virtual void Update(T objeto) { }
        public virtual IEnumerable<T> GetAll()
        {
            return null;
        }
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return null;
        }
    }
}    
