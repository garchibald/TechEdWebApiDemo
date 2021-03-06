﻿//
// Grant Archibald (c) 2012
//
// See Licence.txt
//
using System;
using System.ComponentModel.DataAnnotations;

namespace TechEdWebApiDemo.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        public string FirstMidName { get; set; }

        public string LastName { get; set; }

        public DateTime? Joined { get; set; }

        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
    }
}