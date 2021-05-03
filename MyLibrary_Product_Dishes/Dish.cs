using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary_Product_Dishes
{
    /// <summary>
    /// Простой класс без заморочек хранит набор продуктов и список рецептов
    /// </summary>
    public class Dish
    {
        public string NameDish { get; set; }
        /// <summary>
        /// Список рецептов
        /// </summary>
        public List<Dish> ListDishes { get; set; }

        /// <summary>
        /// Спсок продуктов
        /// </summary>
        public List<Product> products { get; set; }


        public Dish()
        {

        }


        /// <summary>
        /// Метод создает список продуктов и рецептов
        /// </summary>
        public void makeMenu()
        {
            products = new List<Product>();
            products.Add(new Product() { NameProduct = "Картошка" });
            products.Add(new Product() { NameProduct = "Колбаса" });
            products.Add(new Product() { NameProduct = "Рис" });
            products.Add(new Product() { NameProduct = "Мясо" });
            products.Add(new Product() { NameProduct = "Соль" });
            products.Add(new Product() { NameProduct = "Сахар" });
            products.Add(new Product() { NameProduct = "Вода" });
            products.Add(new Product() { NameProduct = "Хлеб" });
            products.Add(new Product() { NameProduct = "Сыр" });
            products.Add(new Product() { NameProduct = "Постное масло" });




            ListDishes = new List<Dish>();
            ListDishes.Add(new Dish() {NameDish="Бутерброд" });
            ListDishes.Add(new Dish() {NameDish="Жареная картошка" });
            ListDishes.Add(new Dish() {NameDish="Плов" });
        }

      /// <summary>
      /// Метод пригтовления бутерброда. true если продукты соответсвуют рецепту
      /// </summary>
      /// <param name="products"></param>
        public bool Buterbrod(List<Product> products)
        {
            

            var res = products.Where(p => p.NameProduct == "Колбаса" || p.NameProduct == "Сыр" || p.NameProduct == "Хлеб");
         

            if (res.Count() == 3)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Метод пригтовления Жфренной картошки. возвращает true если продукты соответсвуют рецепту
        /// </summary>
        /// <param name="products"></param>
        public bool FrenchFries(List<Product> products)
        {

            var res = products.Where(p => p.NameProduct == "Картошка" || p.NameProduct == "Соль" || p.NameProduct == "Постное масло");

            if (res.Count() == 3)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Метод пригтовления Плова. возвращает true если продукты соответсвуют рецепту
        /// </summary>
        /// <param name="products"></param>
        public bool Plov(List<Product> products)
        {

            var res = products.Where(p => p.NameProduct == "Рис" || p.NameProduct == "Мясо" || p.NameProduct == "Соль" || p.NameProduct== "Вода");

            if (res.Count() == 4)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



    }
}
