using System;
using System.Collections.Generic;

#nullable disable

namespace RecipesApp.Models
{
    public partial class Step
    {
        public int StepId { get; set; }
        public int RecipeId { get; set; }
        public string Instructions { get; set; }
        public int SortOrder { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
