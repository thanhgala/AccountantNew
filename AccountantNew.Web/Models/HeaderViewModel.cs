using System.Collections.Generic;

namespace AccountantNew.Web.Models
{
    public class HeaderViewModel
    {
        public IEnumerable<NewCategoryViewModel> ListCategory { get; set; }

        public IEnumerable<ApplicationUserViewModel> ListUserFocus { get; set; }

        public IEnumerable<NewViewModel> ListNew { get; set; }

        public IEnumerable<NewViewModel> ListNotification { get; set; }

        public IEnumerable<FocusNotificationViewModel> ListFocus { get; set; }
    }
}