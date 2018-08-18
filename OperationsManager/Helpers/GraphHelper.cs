using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Helpers
{
    public static class GraphHelper
    {
        public static MvcHtmlString GetMaxMarksGraphData(List<GraphRecords> graphRecords)
        {
            string dataString = "[";
            for(int i=0;i<graphRecords.Count;i++)
            {
                if(i==0)
                {
                    dataString = dataString + graphRecords[i].ClassHighest;
                }
                else
                {
                    dataString = dataString + "," + graphRecords[i].ClassHighest;
                }
            }
            dataString = dataString + "]";
            return new MvcHtmlString(dataString);
        }

        public static MvcHtmlString GetAvgMarksGraphData(List<GraphRecords> graphRecords)
        {
            string dataString = "[";
            for (int i = 0; i < graphRecords.Count; i++)
            {
                if (i == 0)
                {
                    dataString = dataString + graphRecords[i].ClassAverage;
                }
                else
                {
                    dataString = dataString + "," + graphRecords[i].ClassAverage;
                }
            }
            dataString = dataString + "]";
            return new MvcHtmlString(dataString);
        }

        public static MvcHtmlString GetObtainedMarksGraphData(List<GraphRecords> graphRecords)
        {
            string dataString = "[";
            for (int i = 0; i < graphRecords.Count; i++)
            {
                if (i == 0)
                {
                    dataString = dataString + graphRecords[i].CurrentMarks;
                }
                else
                {
                    dataString = dataString + "," + graphRecords[i].CurrentMarks;
                }
            }
            dataString = dataString + "]";
            return new MvcHtmlString(dataString);
        }

        public static MvcHtmlString GetSubjectLabels(List<GraphRecords> graphRecords)
        {
            string dataString = "[";
            for (int i = 0; i < graphRecords.Count; i++)
            {
                if (i == 0)
                {
                    dataString = dataString + "\"" + graphRecords[i].SubjectName + "\"";
                }
                else
                {
                    dataString = dataString + ",\"" + graphRecords[i].SubjectName + "\"";
                }
            }
            dataString = dataString + "]";
            return new MvcHtmlString(dataString);
        }
    }
}