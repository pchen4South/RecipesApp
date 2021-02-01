using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipesApp.DataAccess;
using RecipesApp.Models;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace RecipesApp.Controllers
{
    public class APIController : Controller
    {
        private RecipeDataAccessLayer RecipesData = new RecipeDataAccessLayer();

        #region Recipes
        public IActionResult GetAllRecipes() {
            var recipes = RecipesData.GetAllRecipes();
            return Json(JsonConvert.SerializeObject(recipes));
        }

        [HttpPost]
        public IActionResult SubmitRecipe(int RecipeId, string title, string authorComments, IFormFile recipeImage)
        {
            try
            {
                Recipe recipe = RecipesData.GetRecipeByID(RecipeId);
                recipe.Title = title;
                recipe.AuthorComments = authorComments;
                recipe.Published = true;

                if (recipeImage != null)
                {

                    //Set Key Name
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(recipeImage.FileName);

                    //Get url To Save
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", ImageName);

                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        recipeImage.CopyTo(stream);
                        recipe.RecipeImagePath = ImageName;
                    }
                }

                RecipesData.UpdateRecipe(recipe);
                return Redirect($"/Recipe/Detail/{recipe.RecipeId}");
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = $"{e.Message}" });
            }
        }

        [HttpPost]
        public IActionResult RateRecipe(int RecipeId, int Rating)
        {
            try
            {
                RecipeRating rating = new RecipeRating
                {
                    RecipeId = RecipeId,
                    Rating = Rating,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                RecipesData.AddRating(rating);
                Recipe recipe = RecipesData.GetRecipeByID(RecipeId);

                return Json(new { success = true, rating = recipe.Rating, votes = recipe.RatingCount });
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = $"{e.Message}" });
            }

        }
        [HttpPost]
        public JsonResult ToggleRecipeAsFavorite(int RecipeID)
        {
            try
            {
                //hardcoded user id
                int userId = 1;
                var toggledValue = RecipesData.ToggleRecipeAsFavorite(userId, RecipeID);
                return Json(new { success = true, toggled = toggledValue });
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = $"{e.Message}" });
            }
        }
        #endregion

        #region Ingredients
        [HttpPost]
        public JsonResult CreateIngredient(int RecipeID, string Ingredient, decimal Quantity, int UnitOfMeasurementId)
        {
            try
            {
                UnitOfMeasurement unit = RecipesData.GetUnitOfMeasurementById(UnitOfMeasurementId);
                Ingredient ingredient = new Ingredient
                {
                    RecipeId = RecipeID,
                    Name = Ingredient,
                    Quantity = Quantity,
                    UnitOfMeasurement = unit
                };
                
                RecipesData.AddIngredient(ingredient);

                return Json(new { success = true, ingredient = Ingredient, quantity = Quantity, unit = unit.Abbreviation, newid = ingredient.IngredientId });
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = $"{e.Message}" });
            }
        }

        [HttpPost]
        public JsonResult DeleteIngredient(int IngredientId)
        {
            try
            {
                RecipesData.DeleteIngredient(IngredientId);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = $"{e.Message}" });
            }
        }

        #endregion

        #region Steps
        [HttpPost]
        public JsonResult CreateStep(int RecipeId, string Instructions)
        {
            try
            {
                int prevSteps = RecipesData.GetAllStepsForRecipe(RecipeId).Count();

                Step step = new Step
                {
                    RecipeId = RecipeId,
                    Instructions = Instructions,
                    SortOrder = prevSteps + 1
                };
                
                RecipesData.AddStep(step);

                return Json(new { success = true, instructions = Instructions, sortOrder = prevSteps + 1, newid = step.StepId });
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = $"{e.Message}" });
            }
        }

        public JsonResult DeleteStep(int StepId)
        {
            try
            {
                RecipesData.DeleteStep(StepId);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = $"{e.Message}" });
            }
        }
        #endregion

    }
}

