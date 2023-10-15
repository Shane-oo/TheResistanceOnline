using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheResistanceOnline.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserSettingsController: ApiControllerBase
{
}
