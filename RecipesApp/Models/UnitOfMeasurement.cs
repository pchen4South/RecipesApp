using System;
using System.Collections.Generic;

#nullable disable

namespace RecipesApp.Models
{
    public partial class UnitOfMeasurement
    {
        public UnitOfMeasurement()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        public int UnitOfMeasurementId { get; set; }
        public string Unit { get; set; }
        public string Abbreviation { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}
