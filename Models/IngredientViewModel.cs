using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeProj.Models;

namespace RecipeProj.Models
{
    public class IngredientViewModel
    {
        public List<RecipeProj.Models.Ingredient> AvailableIngredients { get; set; }
    }
}