using System;
using System.Collections.Generic;
using System.Text;

namespace WebInvoiceDatabase
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString = null;
        public string ConnectionString { get { return this._connectionString; } }
        public BaseRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }
    }
}
