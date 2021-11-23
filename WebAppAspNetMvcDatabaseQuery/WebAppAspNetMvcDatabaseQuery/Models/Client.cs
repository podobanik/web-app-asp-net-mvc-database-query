using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppAspNetMvcDatabaseQuery.Models
{
    public class Client
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Имя клиента
        /// </summary>
        [Required]
        [Display(Name = "Имя клиента", Order = 5)]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия клиента
        /// </summary>
        [Required]
        [Display(Name = "Фамилия клиента", Order = 10)]
        public string Surname { get; set; }

        /// <summary>
        /// Возраст клиента
        /// </summary>
        [Required]
        [Display(Name = "Возраст клиента", Order = 20)]
        public int Age { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>

        [Display(Name = "Дата рождения", Order = 30)]
        public DateTime? Birthday { get; set; }
    }
}