
using System.Collections.Generic;

namespace XP_Scrum_Grupp2.Models
{
    public class MeetingTimeInvited
    {
        public int Id { get; set; }
        public string InvitedId { get; set; }
        public int TimesId { get; set; }
        public int MeetingId { get; set; }

        public virtual MeetingInvited Invited { get; set; }
        public virtual MeetingTimes Times { get; set; }
    }

    public class MeetTimeInvModel
    {
        public List<MeetingTimes> ListOfTimes { get; set; }
        public List<MeetingTimeInvited> ListOfChosenTimes { get; set; }
    }
}