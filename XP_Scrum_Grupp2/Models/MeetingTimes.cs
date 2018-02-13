using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class MeetingTimes
    {
        [Key]
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public DateTime Time { get; set; }
        public bool ChosenTime { get; set; }
        public int Votes { get; set; }
        public int FakeId { get; set; }
        public Meeting Meeting { get; set; }
    }
}