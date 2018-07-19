using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBFVideoLib.Common
{
  public  class ThumbnailHelper
    {
        const string EnglishSubjectString = "english";
        const string HindiSubjectString = "hindi";
        const string EVSSubjectString = "evs";
        const string MathsSubjectString = "maths";

        public static string GetThumbnailFileName(string subjectString)
        {
            if (subjectString.ToLower().Contains(EnglishSubjectString))
            {
                return "Subjects_English.png";
            }
            else if (subjectString.ToLower().Contains(HindiSubjectString))
            {
                return "Subjects_Hindi.png";
            }
            else if (subjectString.ToLower().Contains(EVSSubjectString))
            {
                return "Subjects_EVS.png";
            }
            else if (subjectString.ToLower().Contains(MathsSubjectString))
            {
                return "Subjects_Maths.png";
            }
            return "Subjects_English.png";
        }
    }
}
