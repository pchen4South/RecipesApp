using System;
using System.Collections.Generic;

#nullable disable

namespace RecipesApp.Models
{
    public partial class UserFavorite
    {
        public int UserFavoriteId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual User User { get; set; }
    }
}
