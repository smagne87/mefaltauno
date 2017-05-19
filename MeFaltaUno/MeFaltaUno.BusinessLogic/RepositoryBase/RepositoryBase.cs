using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace MeFaltaUno.BusinessLogic
{
    public class RepositoryBase<TObject, DbCtxt> : IRepositoryBase<TObject>
            where TObject : class
            where DbCtxt : DbContext
    {
        protected DbCtxt Context;
        private bool shareContext = false;


        public RepositoryBase(DbCtxt context)
        {
            Context = context;
            shareContext = true;
        }

        protected DbSet<TObject> DbSet
        {
            get
            {
                return Context.Set<TObject>();
            }
        }

        public void Dispose()
        {
            if (shareContext && (Context != null))
                Context.Dispose();
        }

        public virtual IQueryable<TObject> GetAll()
        {
            return DbSet.AsQueryable();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable<TObject>();
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public virtual TObject GetById(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual TObject Create(TObject TObject)
        {
            var newEntry = DbSet.Add(TObject);
            if (!shareContext)
                Context.SaveChanges();
            return newEntry;
        }

        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public virtual int Update(TObject TObject)
        {
            var entry = Context.Entry(TObject);
            DbSet.Attach(TObject);
            entry.State = EntityState.Modified;
            if (!shareContext)
                return Context.SaveChanges();
            return 0;
        }

        public virtual int Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
            if (!shareContext)
                return Context.SaveChanges();
            return 0;
        }

        public virtual IQueryable<TObject> Filter<Key>(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() :
                DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) :
                _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public virtual int Delete(TObject t)
        {
            DbSet.Remove(t);
            if (!shareContext)
                return Context.SaveChanges();
            return 0;
        }

        public virtual int Delete(params object[] keys)
        {
            TObject t = DbSet.Find(keys);
            DbSet.Remove(t);
            if (!shareContext)
                return Context.SaveChanges();
            return 0;
        }
    }
}