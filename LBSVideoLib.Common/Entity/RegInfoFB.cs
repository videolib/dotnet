using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBFVideoLib.Common.Entity
{

    public class RegInfoFB
    {
        public string RegDate
        {
            get; set;
        }

        public string LoginEmail
        {
            get; set;
        }
        public string Password
        {
            get; set;
        }

        public string SchoolName
        {
            get; set;
        }

        public string SchoolCode
        {
            get; set;
        }

        public string Session
        {
            get; set;
        }


        public List<ClassFB> Classes
        {
            get; set;
        }
    }


    public class ClassFB
    {

        public string Name
        {
            get; set;
        }

        public List<SeriesFB> Series
        {
            get; set;
        }
    }
    public class SeriesFB
    {

        public string Name
        {
            get; set;
        }
        public List<SubjectFB> Subjects;
    }

    public class SubjectFB
    {
        public string Name
        {
            get; set;
        }
        public List<BookFB> Books;
    }

    public class BookFB
    {
        public string Name
        {
            get; set;
        }

    }
}
