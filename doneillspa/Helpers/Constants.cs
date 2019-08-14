using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Helpers
{
    public static class Constants
    {
         public static class Strings
         { 
             public static class JwtClaimIdentifiers
             { 
                 public const string Rol = "rol", Id = "id"; 
             } 
 
            public static class Timesheets
            {
                public static class NonChargeableCodes
                {
                    public const string AnnualLeave = "NC1";
                }

                public const string StartTime = "9:30";
                public const string EndTime = "17:30";
            }

 
             public static class JwtClaims
             { 
                public const string Administrator = "Administrator";
                public const string Engineer = "Engineer";
                public const string Apprentice = "Apprentice";
            } 
         } 

    }
}
