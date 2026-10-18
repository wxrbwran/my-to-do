namespace MyToDo.Desktop.Models
{
    public class MenuBar
    {
        public int Index { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TargetView { get; set; } = string.Empty;
    }

    public class IndexTaskBar
    {
        public string Icon { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Color { get; set; } = "#1E88E5";
        public string Target { get; set; } = string.Empty;
        public int MainWindowMenuIndex { get; set; }
    }

    public static class AppSession
    {
        public static string Username { get; set; } = "小然";
        public static string Account { get; set; } = "admin";
        public static bool IsLoggedIn { get; set; } = false;
    }
}
