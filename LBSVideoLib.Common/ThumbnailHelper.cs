using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBFVideoLib.Common
{
  public  class ThumbnailHelper
    {
        const string Class1String = "class 1";
        const string Class2String = "class 2";
        const string Class3String = "class 3";
        const string Class4String = "class 4";
        const string Class5String = "class 5";        
        
        const string EnglishSubjectString = "english";
        const string HindiSubjectString = "hindi";
        const string EVSSubjectString = "evs";
        const string MathsSubjectString = "maths";

        public static string GetThumbnailFileName(string classString, string subjectString)
        {
            if (Class1String.ToLower().Contains(classString) && EnglishSubjectString.ToLower().Contains(subjectString))
            {
                return "English Cover Class 1st.jpg";
            }
            if (Class2String.ToLower().Contains(classString) && EnglishSubjectString.ToLower().Contains(subjectString))
            {
                return "English Cover Class 2nd.jpg";
            }
            if (Class3String.ToLower().Contains(classString) && EnglishSubjectString.ToLower().Contains(subjectString))
            {
                return "English Cover Class 3rd.jpg";
            }
            if (Class4String.ToLower().Contains(classString) && EnglishSubjectString.ToLower().Contains(subjectString))
            {
                return "English Cover Class 4th.jpg";
            }
            else if (Class5String.ToLower().Contains(classString) && EnglishSubjectString.ToLower().Contains(subjectString))
            {
                return "English Cover Class 5th.jpg";
            }


            else if (Class1String.ToLower().Contains(classString) && HindiSubjectString.ToLower().Contains(subjectString))
            {
                return "Hindi Cover Class 1st.jpg";
            }
            if (Class2String.ToLower().Contains(classString) && HindiSubjectString.ToLower().Contains(subjectString))
            {
                return "Hindi Cover Class 2nd.jpg";
            }
            if (Class3String.ToLower().Contains(classString) && HindiSubjectString.ToLower().Contains(subjectString))
            {
                return "Hindi Cover Class 3rd.jpg";
            }
            if (Class4String.ToLower().Contains(classString) && HindiSubjectString.ToLower().Contains(subjectString))
            {
                return "Hindi Cover Class 4th.jpg";
            }
            else if (Class5String.ToLower().Contains(classString) && HindiSubjectString.ToLower().Contains(subjectString))
            {
                return "Hindi Cover Class 5th.jpg";
            }


            else if (subjectString.ToLower().Contains(EnglishSubjectString))
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
