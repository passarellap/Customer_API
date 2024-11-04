using Poseidon.AuthorizationManager;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnagraficaAPI.Entity.Model
{
    public class AgenziaEntityBase : BaseEntity
    {
        [Required]
        [Column("IDGestione")]
        public int IdAgenzia { get; set; }
    }
}
