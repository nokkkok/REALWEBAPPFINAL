using Microsoft.AspNetCore.Mvc; // 1. บรรทัดนี้ต้องมีเสมอ

namespace WEBPROJECT.Controllers // 2. เปลี่ยนชื่อตามโปรเจกต์คุณ
{
    public class DashboardController : Controller // 3. ต้องสืบทอดจาก Controller
    {
        // นี่คือ Action (หน้าเว็บ 1 หน้า)
        public IActionResult Index() 
        {
            return View(); // 4. คำสั่งนี้จะไปหาไฟล์ Views/Dashboard/Index.cshtml ให้เอง
        }
    }
}