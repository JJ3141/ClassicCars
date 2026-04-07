namespace ClassicCars.Areas.Admin.ViewModels
{
    public class UserListItemViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Roles { get; set; }
        public string SelectedRole { get; set; }
        public List<string> AllRoles { get; set; }
    }
}