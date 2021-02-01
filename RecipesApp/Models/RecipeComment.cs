using System;
using System.Collections.Generic;

#nullable disable

namespace RecipesApp.Models
{
    public partial class RecipeComment
    {
        public int RecipeCommentId { get; set; }
        public int RecipeId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
