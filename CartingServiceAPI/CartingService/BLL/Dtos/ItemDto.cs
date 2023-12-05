﻿using System.ComponentModel.DataAnnotations;

namespace CartingService.BLL.Dtos
{
    public class ItemDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Money { get; set; }
        [Required]
        public int Quantity { get; set; }

        public string Image { get; set; }
        [Required]
        public Guid CartId { get; set; }
    }
}
