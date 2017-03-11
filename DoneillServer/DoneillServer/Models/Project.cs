﻿//using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoneillServer.Models
{
    //public class Project : IProject
    public class Project
    {
        //public int identifier { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string ContactNumber { get; set; }
        public string Details { get; set; }

        public string getName()
        {
            return Name;
        }

        public string getDetails()
        {
            return Details;
        }

        public string getContactNumber()
        {
            return ContactNumber;
        }

        public DateTime getStartDate()
        {
            return StartDate;
        }
    }
}