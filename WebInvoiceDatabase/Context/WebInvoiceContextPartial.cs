using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebInvoiceDatabase.Context
{
    public partial class WebInvoiceContext
    {
        private readonly string _connectionString = null;
        public WebInvoiceContext(string connectionString)
        {
            this._connectionString = connectionString;
        }        
    }
}
