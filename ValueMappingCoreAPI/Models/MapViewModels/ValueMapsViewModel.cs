using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Models.MapViewModels
{
    public class ValueMapsViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "OPSystem")]
        public string System { get; set; }

        [Required]
        [Display(Name = "Valuation")]
        //[Remote(action: "VerifyVF", controller: "ValueMaps")]
        public string ValuationFunction { get; set; }

        [Required]
        [Display(Name = "Threshold")]
        public double? Threshold { get; set; }

        public DateTime? TransdateTime { get; set; }
    }
}
