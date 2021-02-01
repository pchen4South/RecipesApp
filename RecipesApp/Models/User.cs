using System;
using System.Collections.Generic;

#nullable disable

namespace RecipesApp.Models
{
    public partial class User
    {
        public User()
        {
            Recipes = new HashSet<Recipe>();
            UserFavorites = new HashSet<UserFavorite>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<UserFavorite> UserFavorites { get; set; }
    }
}
