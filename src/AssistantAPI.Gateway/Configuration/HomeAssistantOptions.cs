namespace AssistantAPI.Gateway.Configuration
{
    public class HomeAssistantOptions
    {

        public HomeAssistantOptions()
        {
            Address = "localhost:8123";
        }
        public string Address {get;set;}
    }
}