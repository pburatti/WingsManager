namespace WingsManager.Model.Configurations
{
    public class AppConfiguration
    {
        public AppConfigDatabaseConnections DatabaseConnections { get; set; }
        public EmailConfig EmailConfig { get; set; }
    }
    public class AppConfigDatabaseConnections
    {
        public string WebInvoice { get; set; }
    }

    public class EmailConfig
    {
      public string ServerSmtp { get; set; }
      public string Port{ get; set; }
      public string From { get; set; }
      public string To { get; set; }
    }

}
