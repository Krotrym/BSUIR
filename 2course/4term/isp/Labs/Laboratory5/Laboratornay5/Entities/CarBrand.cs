using SQLite;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Laboratornay5
{
    [Table("Cars")]
    public class CarBrand
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { set; get; }
        public string Name { set; get; }
    }
}
