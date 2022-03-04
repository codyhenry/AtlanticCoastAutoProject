using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeProj.Models
{
    public class Recipe
    {
        [Display(Name ="Recipe Name")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public  List<RecipeProj.Models.Ingredient> Ingredients { get; set; } 
    }
}