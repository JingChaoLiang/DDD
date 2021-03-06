﻿using CDomain.IUnitOfWork;
using System.Threading.Tasks;

namespace CInfrastruct.UnitOfWork
{
    /// <summary>
    /// 工作单元,这里的提交才会将数据更新到数据库,之所以加这个,是考虑到跨库/表事务
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        protected readonly Context.Context _db;

        public UnitOfWork(Context.Context db)
        {
            _db = db;
        }

        public async Task<int> CommitAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task<int> CommitByTransactionAsync()
        {
            var result = 0;
            using (var transaction = _db.Database.BeginTransaction())
            {
                result = await _db.SaveChangesAsync();
                transaction.Commit();
            }
            return result;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
