using IdentityWorkshop.Api.Contracts;
using IdentityWorkshop.Application.Users;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWorkshop.Api.Controllers;

[ApiController]
[Route("api/v1/workshop-users")]
public sealed class WorkshopUsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<WorkshopUsersController> _logger;

    public WorkshopUsersController(UserService userService, ILogger<WorkshopUsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>ค้นหาผู้ใช้งานในชุดข้อมูล Workshop ตาม Username หรือ Display Name</summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(WorkshopApiResponse<IReadOnlyList<UserListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkshopApiResponse<IReadOnlyList<UserListItemDto>>>> Search(
        [FromQuery] string? q,
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        // 5.8.2.2 / LAB 1: เปิด Solution และตรวจเส้นทาง Controller -> Service
        // 5.8.2.2 / LAB 6: เรียก API สำเร็จ/ผิดพลาดและติดตาม Correlation ID จาก Log
        _logger.LogInformation("Searching workshop users with term {SearchTerm} and limit {Limit}", q, limit);

        var users = await _userService.SearchAsync(new UserSearchQuery(q, limit), cancellationToken);
        return Ok(new WorkshopApiResponse<IReadOnlyList<UserListItemDto>>(
            users,
            "ค้นหาข้อมูลผู้ใช้งานสำเร็จ",
            HttpContext.TraceIdentifier));
    }

    // 5.8.2.2 / LAB 4: ให้ผู้เรียนสร้าง GET api/v1/workshop-users/{userId:guid}
    // จากรูปแบบ Controller/Service/Repository ที่มีอยู่ แล้วเพิ่ม 200/404 ใน API contract
}
