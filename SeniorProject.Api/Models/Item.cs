using SeniorProject.Api.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeniorProject.Api.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int LinkId { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public int StockAmount { get; set; }

        public int SlotId { get; set; }

        public int ShelfId { get; set; }

        public int AisleId { get; set; }

        public int SectionId { get; set; }

        public int DepartmentId { get; set; }

        public int LowerDepartmentId { get; set; }


        public string Shelf { get; set; }
        public string Aisle { get; set; }
        public string Section { get; set; }
        public string Department { get; set; }
        public string LowerDepartment { get; set; }
        public string Slot { get; set; }

        public SpoonProductInformation ProductInformation { get; set; }
    }

    public class ShoppingListItem
    {
        public int LinkId { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public int StockAmount { get; set; }

        public int SlotId { get; set; }

        public int ShelfId { get; set; }
 
        public int AisleId { get; set; }

        public int SectionId { get; set; }

        public int DepartmentId { get; set; }
        public string Shelf { get; set; }
        public string Aisle { get; set; }
        public string Section { get; set; }
        public string Department { get; set; }
        public string LowerDepartment { get; set; }
        public string Slot { get; set; }

        public int LowerDepartmentId { get; set; }

        public int ItemQuantity { get; set; }
    }


    // This is what is returned from Spoonacular
    public class ProductInformation
    {
        public int id { get; set; }

        public string result { get; set; }

        public string title { get; set; }

        public decimal price { get; set; }

        public decimal likes { get; set; }

        public string[] badges { get; set; }

        public string[] important_badges { get; set; }

        public string serving_size { get; set; }

        public string number_of_servings { get; set; }

        public decimal spoonacular_score { get; set; }

        public string[] breadcrumbs { get; set; }

        public string generated_text { get; set; }

        public decimal ingredientCount { get; set; }

        public string[] images { get; set; }

        public decimal calories { get; set; }
        public string fat { get; set; }
        public string protein { get; set; }

        public string carbs { get; set; }

        // ProductNutrition nutrition { get; set; }

        public string ingredientList { get; set; }

    }

    // This is what is returned from Spoonacular
    public class SpoonProductInformation
    {
        public int id { get; set; }

        public string title { get; set; }

        public decimal price { get; set; }

        public decimal likes { get; set; }

        public string[] badges { get; set; }

        public string[] important_badges { get; set; }

        public string nutrition_widget { get; set; }

        public string serving_size { get; set; }
        public string number_of_servings { get; set; }

        public decimal spoonacular_score { get; set; }

        public string[] breadcrumbs { get; set; }

        public string generated_text { get; set; }

        public decimal ingredientCount { get; set; }

        public string[] images { get; set; }

    }
}
