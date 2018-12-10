﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CLMS.Framework.Utilities
{
    public class DataAccessContext<T>
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        public Dictionary<Expression<Func<T, object>>, bool> SortByColumnName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}