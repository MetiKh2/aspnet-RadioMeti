using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.Interfaces
{
    public interface IMessageSender
    {
        public Task SendEmailAsync(string email, string title, string text, bool isHTML = false);
    }
}
