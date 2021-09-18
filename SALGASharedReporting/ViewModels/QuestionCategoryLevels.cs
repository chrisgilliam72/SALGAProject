using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGASharedReporting.ViewModels
{
    public class QuestionCategoryLevels
    {

        public int Level1
        {
            get
            {
                if (NoQuestionsLevel1 > 0)
                {
                    int percent = Convert.ToInt32(QuestionsAnsweredLevel1 / NoQuestionsLevel1 * 100);
                    return percent >= 75 ? 1 : 0;
                }
                else
                    return 0;
            }
        }
        public int Level2
        {
            get
            {
                if (NoQuestionsLevel2>0)
                {
                    int percent = Convert.ToInt32(QuestionsAnsweredLevel2 / NoQuestionsLevel2 * 100);
                    return percent >= 75 ? 1 : 0;
                }
                return 0;
            }
        }
        public int Level3
        {
            get
            {
                if (NoQuestionsLevel3 > 0)
                {
                    int percent = Convert.ToInt32(QuestionsAnsweredLevel3 / NoQuestionsLevel3 * 100);
                    return percent >= 75 ? 1 : 0;
                }
                else
                    return 0;
            }
        }
        public int Level4
        {
            get
            {
                if (NoQuestionsLevel4 > 0)
                {
                    int percent = Convert.ToInt32(QuestionsAnsweredLevel4 / NoQuestionsLevel4 * 100);
                    return percent >= 75 ? 1 : 0;
                }
                else
                    return 0;
            }
        }

        public int Level1Percentage
        {
            get
            {
                if (NoQuestionsLevel1 > 0)
                {
                    int percent = Convert.ToInt32(QuestionsAnsweredLevel1 / NoQuestionsLevel1 * 100);
                    return percent;
                }
                return 0;
            }
        }
        public int Level2Percentage
        {
            get
            {
                if (NoQuestionsLevel2 > 0)
                {
                    int percent = Convert.ToInt32(QuestionsAnsweredLevel2 / NoQuestionsLevel2 * 100);
                    return percent;
                }
                return 0;
            }
        }
        public int Level3Percentage
        {
            get
            {
                if (NoQuestionsLevel3 > 0)
                {
                    int percent = Convert.ToInt32(QuestionsAnsweredLevel3 / NoQuestionsLevel3 * 100);
                    return percent;
                }
                return 0;
            }
        }
        public int Level4Percentage
        {
            get
            {
                if (NoQuestionsLevel4 > 0)
                {
                    int percent = Convert.ToInt32(QuestionsAnsweredLevel4 / NoQuestionsLevel4 * 100);
                    return percent;
                }
                return 0;
            }
        }

        public double NoQuestionsLevel1 { get; set; }
        public double NoQuestionsLevel2 { get; set; }
        public double NoQuestionsLevel3 { get; set; }
        public double NoQuestionsLevel4 { get; set; }

        public double QuestionsAnsweredLevel1 { get; set; }
        public double QuestionsAnsweredLevel2 { get; set; }
        public double QuestionsAnsweredLevel3 { get; set; }
        public double QuestionsAnsweredLevel4 { get; set; }

        public int MaturityLevel
        {
            get
            {
                int OverrallLevel = 0;           

                if (Level1Percentage >= 75)
                {
                    OverrallLevel = 1;
                    if (Level2Percentage >= 75)
                    {
                        OverrallLevel = 2;
                        if (Level3Percentage >= 75)
                        {
                            OverrallLevel = 3;
                            if (Level4Percentage >= 75)
                                OverrallLevel = 4;
                        }
                    }
                }

                return OverrallLevel;
            }

        }
    }
}
