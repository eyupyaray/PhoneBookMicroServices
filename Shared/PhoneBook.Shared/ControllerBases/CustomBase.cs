using Microsoft.AspNetCore.Mvc;
using PhoneBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Shared.ControllerBases
{
    public class CustomBase : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(ProcessResult<T> result)
        {
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
    }
}
