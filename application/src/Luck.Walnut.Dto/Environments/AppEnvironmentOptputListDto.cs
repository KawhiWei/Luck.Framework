using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Walnut.Dto.Environments
{
    public class AppEnvironmentOptputListDto
    {

        public string Id { get; set; }

        public string ApplicationId { get; set; }


        public string EnvironmentName { get; set; }

        public string? AppId { get; set; }
    }
}
