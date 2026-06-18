namespace SkyFlow.Config
{
    public static class ConnectionString
    {
        public static string Value =>
            "Server=localhost\\SQLEXPRESS;Database=SkyFlowDB;Integrated Security=True;TrustServerCertificate=True;";

        // If  doesn't work, try this one instead:
        // public static string Value => 
        //     "Server=.\\SQLEXPRESS;Database=SkyFlowDB;Integrated Security=True;TrustServerCertificate=True;";
    }
}