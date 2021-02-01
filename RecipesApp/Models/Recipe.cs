using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RecipesApp.Models
{
    public partial class Recipe
    {
        _RELXContext db = new _RELXContext();
        public Recipe()
        {
            Ingredients = new HashSet<Ingredient>();
            RecipeComments = new HashSet<RecipeComment>();
            RecipeRatings = new HashSet<RecipeRating>();
            Steps = new HashSet<Step>();
            UserFavorites = new HashSet<UserFavorite>();
        }

        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string RecipeImagePath { get; set; }
        public string AuthorComments { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Published { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<RecipeComment> RecipeComments { get; set; }
        public virtual ICollection<RecipeRating> RecipeRatings { get; set; }
        public virtual ICollection<Step> Steps { get; set; }
        public virtual ICollection<UserFavorite> UserFavorites { get; set; }

        public decimal Rating
        {
            get
            {
                var sum = RecipeRatings.Sum(x => x.Rating);
                if (RatingCount > 0)
                {
                    return (decimal)sum / RatingCount;
                }

                return 0;
            }
        }

        public decimal RatingCount {
            get
            {
                return RecipeRatings.Count();
            }
        }

        public bool isLoggedInUserFavorite = false;
        public void CheckIfIsFavorite(int UserID)
        {
            var user = db.UserFavorites.Where(x => x.RecipeId == RecipeId && x.UserId == UserID && x.DeletedDate == null).FirstOrDefault();
            isLoggedInUserFavorite = user != null ? true : false;
        }
    }
}
