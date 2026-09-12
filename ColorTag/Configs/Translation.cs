namespace ColorTag.Configs
{
    public sealed class Translation
    {
        public string DontHavePermissions { get; set; } = "You dont have permissions %permission%";
        public string NotFoundInDataBase { get; set; } = "Your settings were not found, please use the (colortag set (colors)) command";
        public string OtherNotFound { get; set; } = "Player not found";
        public string ColorLimit { get; set; } = "Max colors count: %limit%";
        public string InvalidColor { get; set; } = "Invalid color! %arg% cant be added! %colors%";
        public string KillDataBase { get; set; } = "Date base has been killed";
        public string Successfull { get; set; } = "Your colors updated! Colors: %current%";
        public string SuccessfullDeleted { get; set; } = "Player with ID %userid% has been deleted";
    }
}
