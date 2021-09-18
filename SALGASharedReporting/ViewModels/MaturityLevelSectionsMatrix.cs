using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SALGASharedReporting.ViewModels
{
    public class MaturityLevelSectionsMatrixRow
    {
        public String[] Elements { get; set; }

        public MaturityLevelSectionsMatrixRow()
        {
            Elements =  new string[5];
        }
    }

    public class MaturityLevelSectionsMatrix
    {

        public List<MaturityLevelSectionsMatrixRow> Rows { get; set; }

        public void Build(List<IGrouping<int, MaturityLevelSectionViewModel>> groupedLevelSections)
        {
            var tmpSections = groupedLevelSections.OrderBy(x => x.Key).ToList();
            int rowNo = 0;
            int ElementNo = 0;

            foreach (var sectionGrping in tmpSections)
            {
                foreach (var section in sectionGrping)
                {
                    if (Rows.Count <= rowNo)
                        Rows.Add(new MaturityLevelSectionsMatrixRow());

                    Rows[rowNo].Elements[ElementNo] = section.CategoryName;
                    rowNo++;
                }
                ElementNo++;
                rowNo = 0;
            }

        }

        public MaturityLevelSectionsMatrix()
        {
            Rows = new List<MaturityLevelSectionsMatrixRow>();
        }
    }
}
