using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SALGASharedReporting.ViewModels
{
    public class OverallMaturityLevels
    {
        public String MunicipalityName { get; set; }

        public int Level1Points { get; set; }
        public int Level2Points { get; set; }
        public int Level3Points { get; set; }
        public int Level4Points { get; set; }
        public int NoCategories { get; set; }

        public int Level1Percent
        {
            get
            {
                return NoCategories!=0 ? Convert.ToInt32((double)Level1Points / NoCategories * 100) :0;
            }
        }
        public int Level2Percent
        {
            get
            {
                return NoCategories != 0 ? Convert.ToInt32((double)Level2Points / NoCategories * 100) : 0;
            }
        }
        public int Level3Percent
        {
            get
            {
                return NoCategories != 0 ? Convert.ToInt32((double)Level3Points / NoCategories * 100) : 0;
            }
        }
        public int Level4Percent
        {
            get
            {
                return NoCategories != 0 ? Convert.ToInt32((double)Level4Points / NoCategories * 100) : 0;
            }
        }
        public int MaturityLevel
        {
            get
            {
                int OverrallLevel = 0;
                if (NoCategories > 0)
                {

                    if (Level1Percent >= 75)
                    {
                        OverrallLevel = 1;
                        if (Level2Percent >= 75)
                        {
                            OverrallLevel = 2;
                            if (Level3Percent >= 75)
                            {
                                OverrallLevel = 3;
                                if (Level4Percent >= 75)
                                    OverrallLevel = 4;
                            }
                        }
                    }
                }


                return OverrallLevel;
            }

        }
    }
}
