using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeProj.Models
{
    public class CreateRecipe
    {
        public string Name { get; set; }

        public List<Category> Categories { get; set; }
    }
    public class Category
    {
        public string Name { get; set; }

        public List<CreateIngredients> Ingredients { get; set; }
    }
    

    public class CreateIngredients
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public bool Selected { get; set; }
    }
    
}
