﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetMongoDb.Caching
{
    /// <summary>
    /// Cache interface
    /// </summary>
    public interface ICache
    {
        bool Exists(string key);
        T Get<T>(string key);
        T Set<T>(string key, T value);
        T GetOrSet<T>(string key, Func<T> valueBuilder);
        void Remove(string key);
        void Clear();
        int Count();
    }
}
