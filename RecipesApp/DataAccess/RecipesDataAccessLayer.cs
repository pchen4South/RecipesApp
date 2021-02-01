using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace RecipesApp.DataAccess
{
    public class RecipeDataAccessLayer
    {
        _RELXContext db = new _RELXContext();

        #region Recipes
        public IEnumerable<Recipe> GetAllRecipes()
        {
            try
            {
                return db.Recipes
                    .Include(x => x.User)
                    .Include(x => x.RecipeRatings)
                    .Include(x => x.UserFavorites)
                    .Where(x => x.Published == true).ToList();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Recipe> GetFilteredRecipes(int type)
        {
            try
            {
                var recipes = db.Recipes
                    .Include(x => x.User)
                    .Include(x => x.RecipeRatings)
                    .Include(x => x.UserFavorites)
                    .Where(x => x.Published == true).ToList();



                if (type == (int)Enums.eFilterOptions.MyFavorite)
                {
                    foreach (var r in recipes)
                    {
                        r.CheckIfIsFavorite(1);
                    }
                    recipes = recipes.Where(x => x.isLoggedInUserFavorite == true).ToList();
                }

                return recipes;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Recipe> GetUserRecipes(int UserID)
        {
            try
            {
                return db.Recipes
                        .Include(x => x.User)
                        .Include(x => x.RecipeRatings)
                        .Where(x => x.UserId == UserID && x.Published == true)
                        .ToList();
            }
            catch
            {
                throw;
            }
        }

        public Recipe GetRecipeByID(int RecipeID)
        {
            try
            {
                return db.Recipes
                    .Include(x => x.User)
                    .Include(x => x.RecipeRatings)
                    .Include(x => x.Steps)
                    .Include(x => x.Ingredients)
                    .ThenInclude(x => x.UnitOfMeasurement)
                    .Where(x => x.RecipeId == RecipeID)
                    .First();
            }
            catch
            {
                throw;
            }
        }

        public bool ToggleRecipeAsFavorite(int UserId, int RecipeId)
        {
            Recipe recipe = db.Recipes.Find(RecipeId);
            UserFavorite fav = db.UserFavorites.Where(x => x.UserId == UserId && x.RecipeId == RecipeId).FirstOrDefault();

            if (fav == null)
            {
                fav = new UserFavorite
                {
                    CreatedDate = DateTime.Now,
                    UserId = UserId,
                    RecipeId = RecipeId
                };
                db.UserFavorites.Add(fav);
                db.SaveChanges();
                return true;
            }
            else
            {
                if (fav.DeletedDate != null)
                {
                    fav.DeletedDate = null;
                }
                else
                {
                    fav.DeletedDate = DateTime.Now;
                }
                db.Entry(fav).State = EntityState.Modified;
                db.SaveChanges();

                recipe.CheckIfIsFavorite(1);
                return fav.DeletedDate == null ? true : false;
            }
        }


        public int AddRecipe(Recipe recipe)
        {
            try
            {
                db.Recipes.Add(recipe);
                db.SaveChanges();
                return recipe.RecipeId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        //Get the details of a particular recipe    
        public Recipe GetRecipe(int RecipeID)
        {
            try
            {
                Recipe rec = db.Recipes.Find(RecipeID);
                return rec;
            }
            catch
            {
                throw;
            }
        }

        public int UpdateRecipe(Recipe recipe)
        {
            try
            {
                db.Entry(recipe).State = EntityState.Modified;
                db.SaveChanges();
                return recipe.RecipeId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        #endregion

        #region Steps
        public int AddStep(Step step)
        {
            try
            {
                db.Steps.Add(step);
                db.SaveChanges();
                return step.StepId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public IEnumerable<Step> GetAllStepsForRecipe(int RecipeId)
        {
            try
            {
                return db.Steps.Where(x => x.RecipeId == RecipeId).ToList();
            }
            catch
            {
                throw;
            }
        }

        public void DeleteStep(int StepId)
        {
            try
            {
                Step step = db.Steps.Find(StepId);
                db.Steps.Remove(step);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Ingredients
        public int AddIngredient(Ingredient ingredient)
        {
            try
            {
                db.Ingredients.Add(ingredient);
                db.SaveChanges();
                return ingredient.IngredientId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public void DeleteIngredient(int IngredientId)
        {
            try
            {
                Ingredient ingredient = db.Ingredients.Find(IngredientId);
                db.Ingredients.Remove(ingredient);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Ratings
        public int AddRating(RecipeRating rating)
        {
            try
            {
                db.RecipeRatings.Add(rating);
                db.SaveChanges();
                return rating.RecipeRatingId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        #endregion

        #region UnitsOfMeasurement
        public IEnumerable<UnitOfMeasurement> GetAllUnitsOfMeasurement(){
            try
            {
                return db.UnitOfMeasurements.ToList(); 
            }
            catch {
                throw;
            }
        }
        public UnitOfMeasurement GetUnitOfMeasurementById(int UnitOfMeasurementId){
            try
            {
                return db.UnitOfMeasurements.Find(UnitOfMeasurementId); 
            }
            catch {
                throw;
            }
        }
        #endregion


    }
}
