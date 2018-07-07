namespace Scada.wpf.Classes.User
{
    public static class UserInfo
    {
        public static model.User InvalidUser = new model.User
        {
            User_ID = 0,
            UserName = "No User",
            Authorization = 0,
            SuperUser = false,
            Email = "",
            PhotoAddress = "",
            Position = "",
            Title = "",
            CardID = "",
            SecondName="",
            Name="",
            Surname=""
        };

    }
    public static class AuthorizatonConverter
    {

        public static string ConvertAuthorizationString(int AuthorizataionLevel)
        {
            switch (AuthorizataionLevel)
            {
                case 0: return "Not Authorized";
                case 1: return "Operator";
                case 2: return "Engineer";
                case 3: return "Administrator";
                default:
                    return "Not Authorized";
            }
        }

        public static int ConvertAuthorizationLevel(string Authorizataion)
        {
            switch (Authorizataion)
            {
                case "Not Authorized": return 0;
                case "Operator": return 1;
                case "Engineer": return 2;
                case "Administrator": return 3;
                default:
                    return 0;
            }
        }
    }

}
