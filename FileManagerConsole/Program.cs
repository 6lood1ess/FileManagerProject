using FileManagerConsole.Controllers;

namespace FileManagerConsole {
  public class Program {
    static void Main() {
      Console.Title = "Файловый Менеджер";

      FileManagerController controller = new FileManagerController();
      controller.Run();
    }
  }
}
