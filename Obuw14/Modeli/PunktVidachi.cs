using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obuw14.Modeli
{
    [Table("PunktiVidachi")]
    public class PunktVidachi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Index { get; set; }
        public string Gorod { get; set; }
        public string Ulica { get; set; }
        public string Nomer { get; set; }
        public virtual ICollection<Zakaz> Zakazi { get; set; }
        [NotMapped]
        public string Adres => $"{Index} {Gorod} {Ulica} {Nomer}";
    }
}
