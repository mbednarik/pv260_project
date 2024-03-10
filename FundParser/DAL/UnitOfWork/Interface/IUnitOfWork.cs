﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
    }
}
