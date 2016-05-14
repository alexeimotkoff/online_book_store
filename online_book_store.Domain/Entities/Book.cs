using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace online_book_store.Domain.Entities
{
    public class Book
    {
        [HiddenInput(DisplayValue=false)]
        public int BookId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название книги")]
        public string Name { get; set; }

        [Display(Name = "Автор")]
        [Required(ErrorMessage = "Пожалуйста, введите автора книги")]
        public string Author { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание для книги")]
        public string Description { get; set; }

        [Display(Name = "Жанр")]
        [Required(ErrorMessage = "Пожалуйста, укажите жанр книги")]
        public string Category { get; set; }

        [Display(Name = "Цена (руб)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }
        public byte[] ImageData { get; set; } // картинка побайтово
        public string ImageMimeType { get; set; } //расширение картинки (формат)
    }
}
