using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeProj.Models
{
    public class CreateViewModel
    {
        public string Name { get; set; }

        public List<Ingredient> ingredients { get; set; }
    }
}