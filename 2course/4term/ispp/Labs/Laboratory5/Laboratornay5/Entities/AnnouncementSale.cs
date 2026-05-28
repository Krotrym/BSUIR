using SQLite;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Laboratornay5
{
    [Table("Annoucements")]
    public class Annoucement
    {
        [PrimaryKey, AutoIncrement, Indexed]
        [Column("Id")]
        public int AnnoucementId { get; set; }
        public string Title { get; set; }

        [Indexed]
        public int CarId { get; set; }
    }
}
