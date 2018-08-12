﻿namespace ZigNet.Services.DTOs
{
    public class SuiteName
    {
        public int SuiteID { get; set; }
        public string Name { get; set; }
        public string ApplicationNameAbbreviation { get; set; }
        public string EnvironmentNameAbbreviation { get; set; }

        public string GetName()
        {
            return string.Format("{0} {1} ({2})", ApplicationNameAbbreviation, Name, EnvironmentNameAbbreviation);
        }

        public string GetNameGrouped()
        {
            return ApplicationNameAbbreviation + " " + EnvironmentNameAbbreviation;
        }
    }
}
