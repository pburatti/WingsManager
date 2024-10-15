using System;
using System.Collections.Generic;
using System.Text;

namespace WingsManager.Model.Managers.Entity
{
    public class Product
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? TypeId { get; set; }
        public string TypeCode { get; set; }
        public string CCCode { get; set; }
    }
}
