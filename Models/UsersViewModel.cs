using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azure_ad_address_book.Models
{
    public class UsersViewModel
    {
        public UsersViewModel()
        {
            Users = new List<Microsoft.Graph.User>();
        }

        public List<Microsoft.Graph.User> Users { get; set; }
    }
}
