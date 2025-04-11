using MyApiTemplateCleanArchi.Application.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiTemplateCleanArchi.Application.Commands.Auth
{
    public record RegisterCommand(string Username, string Password) : IRequest<string>;
}
