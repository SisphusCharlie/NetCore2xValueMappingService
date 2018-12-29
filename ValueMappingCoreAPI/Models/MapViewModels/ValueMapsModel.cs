using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Models.MapViewModels
{
    public class ValueMapsModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "OPSystem")]
        public int System { get; set; }

        [Required]
        [Display(Name = "Valuation")]
        public string ValuationFunction { get; set; }

        [Required]
        [Display(Name = "Threshold")]
        public double? Threshold { get; set; }

        public DateTime? TransdateTime { get; set; }
    }
}
