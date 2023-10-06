using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UsuarioApi.Data.Dtos;
using UsuarioApi.Models;

namespace UsuarioApi.Service
{
    public class UsuarioService
    {
        private IMapper _mapper;
        private UserManager<Usuario> _useManeger;
        private SignInManager<Usuario> _signInManager;
        private TokenSercice _tokenService;

        public UsuarioService(IMapper mapper,
                       UserManager<Usuario> useManeger,
                       SignInManager<Usuario> signInManager,
                       TokenSercice tokenService)
        {
            _mapper = mapper;
            _useManeger = useManeger;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task Cadastra(CreateUsuarioDto dto)
        {
            Usuario usuario = _mapper.Map<Usuario>(dto);

            IdentityResult resultado = await
                _useManeger.CreateAsync(usuario, dto.Password);

            if (!resultado.Succeeded)
            {
                throw new Exception("Não foi possível cadastrar o usuário");
            }
        }

        public async Task<string> Login(LoginUsuarioDto dto)
        {
            var resultado = await
            _signInManager.PasswordSignInAsync
            (dto.Username, dto.Password, false, false);

            if(!resultado.Succeeded)
            {
                throw new ApplicationException("Não foi possível logar");
            }

            var usuario = _signInManager
                .UserManager
                .Users
                .FirstOrDefault(user => user.NormalizedUserName == dto.Username.ToUpper());

            var token = _tokenService.GerarToken(usuario);

            return token;
        }
    }
}
