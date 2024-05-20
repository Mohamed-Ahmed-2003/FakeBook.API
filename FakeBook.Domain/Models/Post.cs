using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FakeBook.Domain.Models
{
    public class Post
    {
        public int ID { get; set; }
        public required string Text { get; set; }
        //public string Author { get; set; }
        public DateTime Timestamp { get; set; }
        public int Likes { get; set; }
        //public string PrivacySettings { get; set; }
        //public List<string> Attachments { get; set; }

        ////public List<Comment> Comments { get; set; }

        ////// Constructor
        ////public Post()
        ////{
        ////    // Initialize the list property
        ////    Attachments = new List<string>();
        ////    Comments = new List<Comment>();
        ////}
    }
}
