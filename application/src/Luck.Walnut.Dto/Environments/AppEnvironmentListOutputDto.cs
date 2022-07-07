using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Walnut.Dto.Environments
{
    public class AppEnvironmentListOutputDto
    {

        public string Id { get; set; }= default!;

        public string ApplicationId { get; set; }= default!;


        public string EnvironmentName { get; set; } = default!;
    }
}
