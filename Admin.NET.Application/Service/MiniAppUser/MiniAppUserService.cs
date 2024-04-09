using Admin.NET.Core.Service;
using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Microsoft.AspNetCore.Http;
using Furion.DataEncryption;
namespace Admin.NET.Application;
/// <summary>
/// 小程序用户服务
/// </summary>
[AllowAnonymous]
[ApiDescriptionSettings(Order = 600)]
public class MiniAppUserService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly SysCacheService _sysCacheService;
    private readonly SysOnlineUserService _sysOnlineUserService;
    private readonly SysConfigService _sysConfigService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public MiniAppUserService(SqlSugarRepository<SysUser> sysUserRep, SysCacheService sysCacheService, SysOnlineUserService sysOnlineUserService, SysConfigService sysConfigService, IHttpContextAccessor httpContextAccessor)
    {
        _sysUserRep = sysUserRep;
        _sysCacheService = sysCacheService;
        _sysOnlineUserService = sysOnlineUserService;
        _sysConfigService = sysConfigService;
        _httpContextAccessor = httpContextAccessor;
    }
    /// <summary>
    /// 账号密码登录
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>用户名/密码：superadmin/123456</remarks>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Login")]
    public async Task<LoginOutput> Login(MiniAppLoginInput input)
    {
        //// 可以根据域名获取具体租户
        //var host = _httpContextAccessor.HttpContext.Request.Host;

        // 判断密码错误次数（默认5次，缓存30分钟）
        var keyErrorPasswordCount = $"{CacheConst.KeyErrorPasswordCount}{input.Account}";
        var errorPasswordCount = _sysCacheService.Get<int>(keyErrorPasswordCount);
        if (errorPasswordCount >= 5)
            throw Oops.Oh(ErrorCodeEnum.D1027);


        // 账号是否存在
        var user = await _sysUserRep.AsQueryable().Includes(t => t.SysOrg).ClearFilter().FirstAsync(u => u.Account.Equals(input.Account));
        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D0009);

        // 账号是否被冻结
        if (user.Status == StatusEnum.Disable)
            throw Oops.Oh(ErrorCodeEnum.D1017);

        // 租户是否被禁用
        var tenant = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysTenant>>().GetFirstAsync(u => u.Id == user.TenantId);
        if (tenant != null && tenant.Status == StatusEnum.Disable)
            throw Oops.Oh(ErrorCodeEnum.Z1003);

        // 国密SM2解密（前端密码传输SM2加密后的）
        input.Password = CryptogramUtil.SM2Decrypt(input.Password);

        // 密码是否正确
        if (CryptogramUtil.CryptoType == CryptogramEnum.MD5.ToString())
        {
            if (!user.Password.Equals(MD5Encryption.Encrypt(input.Password)))
            {
                _sysCacheService.Set(keyErrorPasswordCount, ++errorPasswordCount, TimeSpan.FromMinutes(30));
                throw Oops.Oh(ErrorCodeEnum.D1000);
            }
        }
        else
        {
            if (!CryptogramUtil.Decrypt(user.Password).Equals(input.Password))
            {
                _sysCacheService.Set(keyErrorPasswordCount, ++errorPasswordCount, TimeSpan.FromMinutes(30));
                throw Oops.Oh(ErrorCodeEnum.D1000);
            }
        }

        // 登录成功则清空密码错误次数
        _sysCacheService.Remove(keyErrorPasswordCount);

        return await CreateToken(user);
    }
    /// <summary>
    /// 生成Token令牌
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<LoginOutput> CreateToken(SysUser user)
    {
        // 单用户登录
        await _sysOnlineUserService.SingleLogin(user.Id);

        // 生成Token令牌
        var tokenExpire = await _sysConfigService.GetTokenExpire();
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        {
            { ClaimConst.UserId, user.Id },
            { ClaimConst.TenantId, user.TenantId },
            { ClaimConst.Account, user.Account },
            { ClaimConst.RealName, user.RealName },
            { ClaimConst.AccountType, user.AccountType },
            { ClaimConst.OrgId, user.OrgId },
            { ClaimConst.OrgName, user.SysOrg?.Name },
            { ClaimConst.OrgType, user.SysOrg?.Type },
        }, tokenExpire);

        // 生成刷新Token令牌
        var refreshTokenExpire = await _sysConfigService.GetRefreshTokenExpire();
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);

        // 设置响应报文头
        _httpContextAccessor.HttpContext.SetTokensOfResponseHeaders(accessToken, refreshToken);

        // Swagger Knife4UI-AfterScript登录脚本
        // ke.global.setAllHeader('Authorization', 'Bearer ' + ke.response.headers['access-token']);

        return new LoginOutput
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }



}

