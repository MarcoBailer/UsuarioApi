﻿using Microsoft.AspNetCore.Authorization;

namespace UsuarioApi.Authorization
{
    public class IdadeAuthorization : AuthorizationHandler<IdadeMinima>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IdadeMinima requirement)
        {
            var dataNascimentoClaim = context.User.FindFirst(claim => claim.Type == "DataNascimento");

            if (dataNascimentoClaim == null)
            {
                return Task.CompletedTask;
            }

            var dataNascimento = Convert.ToDateTime(dataNascimentoClaim.Value);

            var idade = DateTime.Today.Year - dataNascimento.Year;

            if(dataNascimento > DateTime.Today.AddYears(-idade))
            {
                idade--;
            }

            if (idade >= requirement.Idade)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }

    }
}
