﻿using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class Course
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Text { get; set; }

        public ICollection<Group>? Groups { get; set; }

        public List<LecturerCourse>? LecturerCourse { get; } = new();
    }
}
