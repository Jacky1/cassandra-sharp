﻿// cassandra-sharp - high performance .NET driver for Apache Cassandra
// Copyright (c) 2011-2013 Pierre Chalamet
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace CassandraSharp.CQLPoco
{
    using System.Runtime.Serialization;
    using CassandraSharp.Extensibility;

    internal class InstanceBuilder<T> : IInstanceBuilder
    {
        private static readonly WriteAccessor<T> _accessor = new WriteAccessor<T>();

        private T _instance;

        public InstanceBuilder()
        {
            _instance = _accessor.CreateInstance();
        }

        public bool Set(IColumnSpec columnSpec, object data)
        {
            string colName = columnSpec.Name;
            if (_accessor.Set(ref _instance, colName, data))
            {
                return true;
            }

            if (colName.Contains("_"))
            {
                colName = colName.Replace("_", "");
                return _accessor.Set(ref _instance, colName, data);
            }

            return false;
        }

        public object Build()
        {
            IDeserializationCallback cb = _instance as IDeserializationCallback;
            if (null != cb)
            {
                cb.OnDeserialization(null);
            }

            return _instance;
        }
    }
}