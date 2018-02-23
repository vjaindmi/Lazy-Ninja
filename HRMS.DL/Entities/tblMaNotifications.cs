using HRMS.DL.AccountModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.Entities
{
    [Table("tblMaNotifications")]
    public class tblMaNotifications : BaseClass
    {
        [Key]
        public Int64 Id { get; set; }
        public string NotificationType { get; set; }
        public string Details { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public string SenderName { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsOpened { get; set; }
        public Int64 DetailsId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
