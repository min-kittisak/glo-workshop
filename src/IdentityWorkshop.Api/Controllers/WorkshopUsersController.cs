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

    /// <summary>โหลดรายละเอียดผู้ใช้งานจาก User ID</summary>
    /// <param name="userId">รหัสผู้ใช้งานรูปแบบ GUID</param>
    /// <param name="cancellationToken">Token สำหรับยกเลิกคำขอ</param>
    /// <response code="200">พบข้อมูลผู้ใช้งานและส่งรายละเอียดกลับ</response>
    /// <response code="400">User ID ไม่ถูกต้องหรือเป็นค่า Guid.Empty</response>
    /// <response code="404">ไม่พบผู้ใช้งานตาม User ID</response>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(WorkshopApiResponse<UserDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkshopApiResponse<UserDetailDto>>> GetById(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // 5.8.2.2 / LAB 4: Controller -> Service -> Repository สำหรับ Detail Endpoint
        var user = await _userService.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(new WorkshopApiResponse<UserDetailDto>(
            user,
            "โหลดข้อมูลผู้ใช้งานสำเร็จ",
            HttpContext.TraceIdentifier));
    }
}
