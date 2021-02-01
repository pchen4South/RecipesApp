using System;
using System.Collections.Generic;

#nullable disable

namespace RecipesApp.Models
{
    public partial class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual UnitOfMeasurement UnitOfMeasurement { get; set; }
    }
}
