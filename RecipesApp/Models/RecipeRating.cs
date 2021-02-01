using System;
using System.Collections.Generic;

#nullable disable

namespace RecipesApp.Models
{
    public partial class RecipeRating
    {
        public int RecipeRatingId { get; set; }
        public int RecipeId { get; set; }
        public decimal Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
