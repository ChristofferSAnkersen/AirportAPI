﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirportAPI.Entities.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        [MaxLength(255)]
        public string AircraftType { get; set; }

        [Required]
        [MaxLength(255)]
        public string FromLocation { get; set; }

        [Required]
        [MaxLength(255)]
        public string ToLocation { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
