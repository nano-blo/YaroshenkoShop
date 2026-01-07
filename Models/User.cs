using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YaroshenkoShop.Models
{
    public class User : IdentityUser<int>
    {
        [Column("имя")]
        [Display(Name = "Имя")]
        [StringLength(100, ErrorMessage = "Имя не может быть длиннее 100 символов")]
        public string Name { get; set; }

        [NotMapped]
        public string CustomRole { get; set; } = "User";

        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<BuyHistory> BuyHistory { get; set; }
    }
}