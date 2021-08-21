using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Shared.Messages
{
    public class ReportResultMessageCommand
    {
        public string ReportUUID { get; set; }
        public List<ReportContent> ReportContent { get; set; }

        public ReportResultMessageCommand()
        {
            ReportContent = new List<ReportContent>();
        }
    }
    public class ReportContent
    {
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }

}
