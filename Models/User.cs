using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YaroshenkoShop.Models
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}